using AutoMapper;
using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KitchenConnection.BusinessLogic.Services;
public class RecipeService : IRecipeService {
    public readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RecipeService(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<Recipe>> GetRecipes() {
        return await _unitOfWork.Repository<Recipe>().GetAll().ToListAsync();
    }

    public async Task<Recipe> GetRecipe(Guid id)
    {

        Expression<Func<Recipe, bool>> expression = x => x.Id == id;
        var recipe = await _unitOfWork.Repository<Recipe>().GetById(expression).FirstOrDefaultAsync();
        foreach (var item in _unitOfWork.Repository<RecipeInstruction>().GetAll())
        {
            if (item.RecipeId == id)
            {
                recipe.Instructions.Add(item);
            }
        }

        return recipe;
    }

    public async Task<Recipe> UpdateRecipe(RecipeDTO recipeToUpdate)
    {
        Recipe? recipe = await GetRecipe(recipeToUpdate.Id);

        recipe.Name = recipeToUpdate.Name;
        recipe.Description = recipeToUpdate.Description;
        recipe.Instructions = _mapper.Map<List<RecipeInstruction>>(recipeToUpdate.Instructions);

        _unitOfWork.Repository<Recipe>().Update(recipe);

        var res=_unitOfWork.Complete();

        if (res)
        {
            return recipe;
        }
        else
        {
            return null;
        }

    }

    public async Task<bool> DeleteRecipe(Guid id)
    {
        var recipe = await GetRecipe(id);

        _unitOfWork.Repository<Recipe>().Delete(recipe);

        return _unitOfWork.Complete();
    }


    public async Task<Recipe> CreateRecipe(RecipeCreateDTO recipeToCreate)
    {       
        var recipe = _mapper.Map<Recipe>(recipeToCreate);
        foreach (var item in recipeToCreate.Tags)
        {
            var tags = _unitOfWork.Repository<Tag>().GetByCondition(x => x.Name == item.Name).ToList();
            foreach (var tag in tags)
            {
                if (item.Name == tag.Name)
                {
                    foreach (var i in recipe.Tags)
                    {
                        i.Tag = _mapper.Map<Tag>(item);
                        i.Recipe = _mapper.Map<Recipe>(recipeToCreate);
                    }

                }
            }
        }
        recipe.Ingredients.ForEach(x => x.RecipeId = recipe.Id);
        recipe.Instructions.ForEach(x => x.RecipeId = recipe.Id);
        //recipe.Tags.ForEach(x => x.RecipeId = recipe.Id);
        recipe.CuisineId = recipe.Id;
        recipe.Cuisine = await _unitOfWork.Repository<Cuisine>().GetById(x => x.Id == recipeToCreate.Cuisine.CuisineId).FirstOrDefaultAsync();

        recipe.Ingredients.ForEach(x => x.RecipeId = recipe.Id);
        recipe.Instructions.ForEach(x => x.RecipeId = recipe.Id);

        await _unitOfWork.Repository<Recipe>().Create(recipe);
        
        var res=_unitOfWork.Complete();

        if (res)
        {
            return recipe;
        }
        else
        {
            return null;
        }
    }
}