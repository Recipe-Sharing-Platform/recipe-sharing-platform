using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Services.IServices; 
public interface IRecipeService {

    Task<Recipe> CreateRecipe(RecipeCreateDTO recipeToCreate);
    Task<List<Recipe>> GetRecipes();

    Task<Recipe> GetRecipe(Guid id);

    Task<Recipe> UpdateRecipe(RecipeDTO recipeToUpdate);

    Task<bool> DeleteRecipe(Guid id);
}