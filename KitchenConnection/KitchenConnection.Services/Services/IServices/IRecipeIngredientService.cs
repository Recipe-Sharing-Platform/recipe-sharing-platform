using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.Entities.Mappings;

namespace KitchenConnection.BusinessLogic.Services.IServices
{
    public interface IRecipeIngredientService
    {
        Task<List<RecipeIngredient>> GetRecipeIngredients();

        Task<RecipeIngredient> GetRecipeIngredient(Guid id);

        Task<RecipeIngredient> UpdateRecipeIngredient(RecipeIngredientDTO recipeIngredientToUpdate);

        Task<bool> DeleteRecipeIngredient(Guid id);

        Task<RecipeIngredient> CreateRecipeIngredient(RecipeIngredientCreateDTO recipeIngredientToCreate);
    }
}
