using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions;
using KitchenConnection.BusinessLogic.Helpers.Extensions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.DTOs.Nutrients;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using KitchenConnection.Models.DTOs.Ingredient;

namespace KitchenConnection.BusinessLogic.Services;
public class RecipeService : IRecipeService {
    public readonly IUnitOfWork _unitOfWork;
    public readonly IMapper _mapper;
    public readonly ICacheService _cacheService;
    public readonly IRecipeNutrientsService _nutrientsService;
    private readonly IRecommendationsService _recommendationsService;
    private readonly MessageSender _messageSender;
    public RecipeService(IUnitOfWork unitOfWork, IMapper mapper, IRecipeNutrientsService nutrientsService, IRecommendationsService recommendationsService, MessageSender messageSender, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _nutrientsService = nutrientsService;
        _recommendationsService = recommendationsService;
        _messageSender = messageSender;
        _cacheService = cacheService;
    }

    public async Task<RecipeDTO> Create(Guid userId, RecipeCreateDTO recipeToCreate)
    {
        var recipe = _mapper.Map<Recipe>(recipeToCreate);
        recipe.UserId = userId;

        var tagsToCheck = new List<Tag>();
        recipeToCreate.Tags.ForEach(t => tagsToCheck.Add(new Tag { Name = t.Name }));
        
        recipe.Ingredients.ForEach(x => x.RecipeId = recipe.Id);
        recipe.Instructions.ForEach(x => x.RecipeId = recipe.Id);
        recipe.Tags = await AddTagsToRecipe(recipe, tagsToCheck);
        
        recipe.Cuisine = await _unitOfWork.Repository<Cuisine>().GetById(x => x.Id == recipe.CuisineId).FirstOrDefaultAsync();
        if (recipe.Cuisine is null) throw new RecipeCouldNotBeCreatedException("Cuisine not found!");

        recipe = await _unitOfWork.Repository<Recipe>().Create(recipe);
        _unitOfWork.Complete();

        if (recipe is null) throw new RecipeCouldNotBeCreatedException("Recipe could not be created!");

        // remove self referencing loops that cause big json values
        recipe.Cuisine.Recipes = null!;
        recipe.Ingredients.ForEach(x => x.Recipe = null!);
        recipe.Tags.ForEach(x => x.Recipes = null!);
        recipe.Instructions.ForEach(x => x.Recipe = null!);
        recipe.User.Recipes = null!;

        _messageSender.SendMessage(recipe, "index-recipes"); // send to queue for indexing
        _messageSender.SendMessage(recipe, "created-recipe-email");
        _cacheService.RemoveData("recipes");
        
        var recipeDTO = _mapper.Map<RecipeDTO>(recipe);

        var expirationTime = DateTimeOffset.Now.AddDays(1);

        _cacheService.SetData<RecipeDTO>($"recipe-{recipe.Id}", recipeDTO, expirationTime);

        return recipeDTO;
    }

    private async Task SetScoreOnTags(List<Tag> tags, Guid? userId) {
        foreach (Tag tag in tags) {
            //create a recommendation score
            await _recommendationsService.SetScore(userId, tag.Id);
        }
    }

    public async Task<RecipeDTO> Get(Guid recipeId, Guid? userId)
    {
        var singleRecipe = _cacheService.GetData<RecipeDTO>($"recipe-{recipeId}");
        if (singleRecipe is not null) {
            if(userId is not null && userId != Guid.Empty)
                await SetScoreOnTags(_mapper.Map<List<Tag>>(singleRecipe.Tags), userId);
            return singleRecipe;
        }

        var recipe = await _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(recipe => recipe.Id == recipeId, "User, Ingredients, Instructions, Tags, Cuisine").FirstOrDefaultAsync();

        if (recipe is null) throw new RecipeNotFoundException(recipeId);

        //create a recommendation score for each tag in recipe
        if (userId != null && userId != Guid.Empty) {
            await SetScoreOnTags(recipe.Tags, userId);
        }

        var expirationTime = DateTimeOffset.Now.AddDays(1);

        singleRecipe = _mapper.Map<RecipeDTO>(recipe);

        _cacheService.SetData<RecipeDTO>($"recipe-{recipe.Id}", singleRecipe, expirationTime);

        return singleRecipe;
    }

    public async Task<List<RecipeDTO>> GetAll() {
        var recipesToReturn = _cacheService.GetData<List<RecipeDTO>>("recipes");

        if (recipesToReturn is not null && recipesToReturn.Count > 0)
            return recipesToReturn;

        var recipes = await _unitOfWork.Repository<Recipe>().GetAll()
           .Include(u => u.User)
           .Include(c => c.Cuisine)
           .Include(t => t.Tags)
           .Include(i => i.Ingredients)
           .Include(i => i.Instructions).ToListAsync();

        if (recipes is null || recipes.Count == 0) throw new RecipesNotFoundException();

        recipesToReturn = _mapper.Map<List<RecipeDTO>>(recipes);

        var expiryTime = DateTimeOffset.Now.AddDays(1);

        _cacheService.SetData<List<RecipeDTO>>("recipes", recipesToReturn, expiryTime);

        return recipesToReturn;
    }

    public async Task<RecipeNutrientsDTO> GetRecipeNutrients(Guid recipeId)
    {
        var nutrients = _cacheService.GetData<RecipeNutrientsDTO>($"nutrients-{recipeId}");
        if (nutrients != null)
            return nutrients;

        var recipe = await _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(x => x.Id == recipeId, "User, Ingredients, Instructions, Tags, Cuisine").FirstOrDefaultAsync();

        if (recipe is null) throw new RecipeNotFoundException(recipeId);
        
        nutrients = await _nutrientsService.GetNutrients(_mapper.Map<List<RecipeIngredientDTO>>(recipe.Ingredients));
        
        if(nutrients is null) throw new RecipeNutrientsNotFoundException(recipeId);
        
        nutrients.RecipeId = recipe.Id;

        var expiryTime = DateTimeOffset.Now.AddDays(1);
        _cacheService.SetData<RecipeNutrientsDTO>($"nutrients-{recipeId}", nutrients, expiryTime);
        return nutrients;
    }

    public async Task<RecipeDTO> Update(RecipeUpdateDTO recipeToUpdate, Guid userId)
    {
        var recipe = await _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(x => x.Id == recipeToUpdate.Id && x.UserId == userId, "User, Ingredients, Instructions, Tags, Cuisine").FirstOrDefaultAsync();

        if (recipe == null) throw new RecipeNotFoundException(recipeToUpdate.Id);

        recipe.Name = recipeToUpdate.Name;
        recipe.Description = recipeToUpdate.Description;

        recipe.Ingredients = _mapper.Map<List<RecipeIngredient>>(recipeToUpdate.Ingredients);
        recipe.Ingredients.ForEach(x => { x.RecipeId = recipe.Id; x.Recipe = recipe; });
        
        recipe.Instructions = _mapper.Map<List<RecipeInstruction>>(recipeToUpdate.Instructions);
        recipe.Instructions.ForEach(x => { x.RecipeId = recipe.Id; x.Recipe = recipe; });

        //TODO: Update tags

        recipe.VideoInstructions = recipeToUpdate.VideoInstructions;
        recipe.AudioInstructions = recipeToUpdate.AudioInstructions;
        
        recipe = _unitOfWork.Repository<Recipe>().Update(recipe);
        _unitOfWork.Complete();

        if (recipe is null) throw new RecipeCouldNotBeUpdatedException("Recipe could not be updated!");

        // remove self referencing loops that cause big json values
        recipe.User.Recipes = null!;
        recipe.Ingredients.ForEach(x => x.Recipe = null!);
        recipe.Instructions.ForEach(x => x.Recipe = null!);
        recipe.Tags.ForEach(x => x.Recipes = null!);
        recipe.Cuisine.Recipes = null!;
        
        _messageSender.SendMessage(recipe, "update-recipes"); // send to queue

        var updatedRecipe = _mapper.Map<RecipeDTO>(recipe);
        _cacheService.RemoveData("recipes");
        _cacheService.RemoveData($"recipe-{recipe.Id}");
        _cacheService.RemoveData($"nutrients-{recipe.Id}");
        var expiryTime = DateTimeOffset.Now.AddDays(1);
        _cacheService.SetData<RecipeDTO>($"recipe-{recipe.Id}", updatedRecipe, expiryTime);
        return updatedRecipe;
    }

    public async Task<RecipeDTO> Delete(Guid recipeId, Guid userId)
    {
        var recipe = await _unitOfWork.Repository<Recipe>().GetById(r => r.Id == recipeId && r.UserId == userId).FirstOrDefaultAsync();

        if (recipe == null) throw new RecipeNotFoundException(recipeId);

        _unitOfWork.Repository<Recipe>().Delete(recipe);
        _unitOfWork.Complete();
        _messageSender.SendMessage(new { RecipeId = recipe.Id }, "delete-recipes");

        _cacheService.RemoveData("recipes");
        _cacheService.RemoveData($"recipe-{recipeId}");
        _cacheService.RemoveData($"nutrients-{recipeId}");

        return _mapper.Map<RecipeDTO>(recipe);
    }

    private async Task<List<Tag>> AddTagsToRecipe(Recipe recipe, List<Tag> tagsToCreate)
    {
        var existingTags = await _unitOfWork.Repository<Tag>().GetAll().ToListAsync();

        // Check if any of the tags is missing in database when assigned to the recipe on creation
        // create the ones that are missing, and assign the existing ones accordingly

        var missingTags = tagsToCreate.Except(existingTags, new TagNameComparer()).ToList();
        if (missingTags.Any())
        {
            await _unitOfWork.Repository<Tag>().CreateRange(missingTags);
        }

        existingTags.AddRange(missingTags);

        var tagsToAdd = new List<Tag>();
        existingTags.ForEach(async recipeTag =>
        {
            if (tagsToCreate.Contains(recipeTag, new TagNameComparer()))
            {
                tagsToAdd.Add(recipeTag);

                //create a recommendation score
                await _recommendationsService.SetScore(recipe.UserId, recipeTag.Id);                
            }
        });

        return tagsToAdd;
    }

    public async Task<Guid> GetRecipeCreatorId(Guid id)
    {
        var recipe = await _unitOfWork.Repository<Recipe>().GetByCondition(x => x.Id == id).FirstOrDefaultAsync();

        if (recipe is null) throw new RecipeNotFoundException(id);

        return recipe.UserId;
    }

    public async Task<List<RecipeDTO>> GetPaginated(int page, int pageSize) {
        var recipes = await _unitOfWork.Repository<Recipe>().GetPaginated(page, pageSize)
           .Include(u => u.User)
           .Include(c => c.Cuisine)
           .Include(t => t.Tags)
           .Include(i => i.Ingredients)
           .Include(i => i.Instructions).ToListAsync();
        return _mapper.Map<List<RecipeDTO>>(recipes);
    }
}