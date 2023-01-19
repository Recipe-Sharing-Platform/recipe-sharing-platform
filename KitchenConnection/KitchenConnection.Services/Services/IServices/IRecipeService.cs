
using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Services.IServices; 
public interface IRecipeService {
    Task<List<Recipe>> GetRecipes();

    Task<Recipe> GetRecipe(Guid id);

    Task UpdateRecipe(Recipe recipeToUpdate);

    Task DeleteRecipe(Guid id);
}
