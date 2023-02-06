using KitchenConnection.Models.DTOs.Ingredient;
using KitchenConnection.Models.DTOs.Nutrients;

namespace KitchenConnection.BusinessLogic.Services.IServices;

public interface IRecipeNutrientsService
{
    Task<RecipeNutrientsDTO> GetNutrients(List<RecipeIngredientDTO> ingredients);
}
