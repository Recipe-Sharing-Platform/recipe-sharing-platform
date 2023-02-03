using AutoMapper;
using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.BusinessLogic.Helpers.Extensions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.DTOs.Nutrients;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs.ShoppingCart;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Linq.Expressions;

namespace KitchenConnection.BusinessLogic.Services;
public class RecipeService : IRecipeService {
    public readonly IUnitOfWork _unitOfWork;
    public readonly IMapper _mapper;

    public readonly IRecipeNutrientsService _nutrientsService;
    private readonly IRecommendationsService _recommendationsService;
    public RecipeService(IUnitOfWork unitOfWork, IMapper mapper, IRecipeNutrientsService nutrientsService, IRecommendationsService recommendationsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _nutrientsService = nutrientsService;
        _recommendationsService = recommendationsService;
    }

    public async Task<RecipeDTO> Create(RecipeCreateRequestDTO recipeRequestedToCreate, Guid userId)
    {
        RecipeCreateDTO recipeToCreate = new RecipeCreateDTO(recipeRequestedToCreate, userId);
        var recipe = _mapper.Map<Recipe>(recipeToCreate);

        var tagsToCheck = new List<Tag>();
        recipeToCreate.Tags.ForEach(t => tagsToCheck.Add(new Tag { Name = t.Name }));
        
        recipe.Ingredients.ForEach(x => x.RecipeId = recipe.Id);
        recipe.Instructions.ForEach(x => x.RecipeId = recipe.Id);
        recipe.Tags = await AddTagsToRecipe(recipe, tagsToCheck);
        recipe.Cuisine = await _unitOfWork.Repository<Cuisine>().GetById(x => x.Id == recipe.CuisineId).FirstOrDefaultAsync();

        recipe = await _unitOfWork.Repository<Recipe>().Create(recipe);
        _unitOfWork.Complete();

        return _mapper.Map<RecipeDTO>(recipe);
    }

    public async Task<RecipeDTO> Get(Guid id)
    {
        Expression<Func<Recipe, bool>> expression = x => x.Id == id;
        var recipe = await _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(expression, "User, Ingredients, Instructions, Tags, Cuisine").FirstOrDefaultAsync();

        //create a recommendation score for each tag in recipe
        foreach (Tag t in recipe.Tags)
        {
            //create a recommendation score
            await _recommendationsService.SetScore(recipe.UserId, t.Id);          
        }

        return _mapper.Map<RecipeDTO>(recipe);
    }

    public async Task<List<RecipeDTO>> GetAll() {
        var recipes = await _unitOfWork.Repository<Recipe>().GetAll()
            .Include(u => u.User)
            .Include(c => c.Cuisine)
            .Include(t => t.Tags)
            .Include(i => i.Ingredients)
            .Include(i => i.Instructions).ToListAsync();

        return _mapper.Map<List<RecipeDTO>>(recipes);
    }

    public async Task<RecipeNutrientsDTO> GetRecipeNutrients(Guid recipeId)
    {
        var recipe = await _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(x => x.Id == recipeId, "User, Ingredients, Instructions, Tags, Cuisine").FirstOrDefaultAsync();

        var nutrients = await _nutrientsService.GetNutrients(_mapper.Map<List<RecipeIngredientDTO>>(recipe.Ingredients));
        nutrients.RecipeId = recipe.Id;
        return nutrients;
    }

    public async Task<RecipeDTO> Update(RecipeUpdateDTO recipeToUpdate)
    {
        Expression<Func<Recipe, bool>> expression = x => x.Id == recipeToUpdate.Id;
        var recipe = await _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(expression, "User, Ingredients, Instructions, Tags, Cuisine").FirstOrDefaultAsync();

        if (recipe == null) return null;

        recipe.Name = recipeToUpdate.Name;
        recipe.Description = recipeToUpdate.Description;

        recipe.Ingredients = _mapper.Map<List<RecipeIngredient>>(recipeToUpdate.Ingredients);
        recipe.Ingredients.ForEach(x => { x.RecipeId = recipe.Id; x.Recipe = recipe; });
        
        recipe.Instructions = _mapper.Map<List<RecipeInstruction>>(recipeToUpdate.Instructions);
        recipe.Instructions.ForEach(x => { x.RecipeId = recipe.Id; x.Recipe = recipe; });

        //TODO: Update tags

        recipe.VideoInstructions = recipeToUpdate.VideoInstructions;
        recipe.AudioInstructions = recipeToUpdate.AudioInstructions;
        
        _unitOfWork.Repository<Recipe>().Update(recipe);
        _unitOfWork.Complete();

        return _mapper.Map<RecipeDTO>(recipe);
    }

    public async Task<RecipeDTO> Delete(Guid id)
    {
        var recipe = await _unitOfWork.Repository<Recipe>().GetById(r => r.Id == id).FirstOrDefaultAsync();

        if (recipe == null) return null;

        _unitOfWork.Repository<Recipe>().Delete(recipe);
        _unitOfWork.Complete();

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
 
}