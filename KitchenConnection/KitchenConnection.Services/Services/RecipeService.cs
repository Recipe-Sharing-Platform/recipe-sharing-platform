using AutoMapper;
using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.BusinessLogic.Helpers.Extensions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KitchenConnection.BusinessLogic.Services;
public class RecipeService : IRecipeService {
    public readonly IUnitOfWork _unitOfWork;
    public readonly IMapper _mapper;
    public RecipeService(IUnitOfWork unitOfWork, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RecipeDTO> Create(RecipeCreateDTO recipeToCreate)
    {
        var recipe = _mapper.Map<Recipe>(recipeToCreate);

        var tags = await _unitOfWork.Repository<Tag>().GetAll().ToListAsync();
        var tagsToCheck = new List<Tag>();
        recipeToCreate.Tags.ForEach(t => tagsToCheck.Add(new Tag { Name = t.Name }));
        var missingTags = tagsToCheck.Except(tags, new TagNameComparer()).ToList();
        if (missingTags.Any())
        {
            await _unitOfWork.Repository<Tag>().CreateRange(missingTags);
        }

        var recipeTags = await _unitOfWork.Repository<Tag>().GetAll().ToListAsync();
        recipe.Ingredients.ForEach(x => x.RecipeId = recipe.Id);
        recipe.Instructions.ForEach(x => x.RecipeId = recipe.Id);
        recipe.Tags = new List<Tag>();
        recipeTags.ForEach(x =>
        {
            if (tagsToCheck.Contains(x, new TagNameComparer()))
            {
                recipe.Tags.Add(x);
            }
        });

        await _unitOfWork.Repository<Recipe>().Create(recipe);
        _unitOfWork.Complete();

        return _mapper.Map<RecipeDTO>(recipe);
    }

    public async Task<RecipeDTO> Get(Guid id)
    {
        Expression<Func<Recipe, bool>> expression = x => x.Id == id;
        var recipe = await _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(expression, "User, Ingredients, Instructions, Tags, Cuisine").FirstOrDefaultAsync();

        return _mapper.Map<RecipeDTO>(recipe);
    }

    public async Task<List<RecipeDTO>> GetAll() {
        var recipes = await _unitOfWork.Repository<Recipe>().GetAll().ToListAsync();

        return _mapper.Map<List<RecipeDTO>>(recipes);
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

    public async Task Delete(Guid id)
    {
        var recipe = await _unitOfWork.Repository<Recipe>().GetById(r => r.Id == id).FirstOrDefaultAsync();

        if (recipe == null) return;

        _unitOfWork.Repository<Recipe>().Delete(recipe);
        _unitOfWork.Complete();
    }

    
}