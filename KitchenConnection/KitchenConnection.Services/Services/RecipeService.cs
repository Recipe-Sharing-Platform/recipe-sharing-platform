
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KitchenConnection.BusinessLogic.Services;
public class RecipeService : IRecipeService {
    public readonly IUnitOfWork _unitOfWork;
    public RecipeService(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<Recipe>> GetRecipes() {
        return await _unitOfWork.Repository<Recipe>().GetAll().ToListAsync();
    }

    public async Task<Recipe> GetRecipe(string id){
        
        Expression<Func<Recipe, bool>> expression = x => x.Id == id;
        var recipe = _unitOfWork.Repository<Recipe>().GetById(expression);

        return (Recipe)recipe;
    }

    public async Task UpdateRecipe(Recipe recipeToUpdate)
    {
        Recipe? recipe = await GetRecipe(recipeToUpdate.Id);

        recipe.Name = recipeToUpdate.Name;
        recipe.Description = recipeToUpdate.Description;
        recipe.Steps = recipeToUpdate.Steps;

        _unitOfWork.Repository<Recipe>().Update(recipe);

        _unitOfWork.Complete();
    }

    public async Task DeleteRecipe(string id)
    {
        var recipe = await GetRecipe(id);

        _unitOfWork.Repository<Recipe>().Delete(recipe);

        _unitOfWork.Complete();
    }

   /* public async Task CreateProduct(ProductCreateDto coverToCreate)
    {
        var product = _mapper.Map<Product>(coverToCreate);

        _unitOfWork.Repository<Product>().Create(product);

        _unitOfWork.Complete();
    }*/
}
