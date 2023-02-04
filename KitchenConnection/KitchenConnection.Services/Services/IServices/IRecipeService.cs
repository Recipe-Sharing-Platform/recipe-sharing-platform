using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs.Nutrients;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs.ShoppingCart;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Services.IServices;
public interface IRecipeService
{
    Task<RecipeDTO> Create(RecipeCreateRequestDTO recipeToRequestCreate, Guid userId);
    Task<RecipeDTO> Get(Guid id);
    Task<List<RecipeDTO>> GetAll();
    Task<RecipeDTO> Update(RecipeUpdateDTO recipeToUpdate);
    Task<RecipeDTO> Delete(Guid id);

    Task<RecipeNutrientsDTO> GetRecipeNutrients(Guid recipeId);
    Task<Guid> GetRecipeCreatorId(Guid recipeId);
}