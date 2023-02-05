using KitchenConnection.Models.DTOs.Nutrients;
using KitchenConnection.Models.DTOs.Recipe;

namespace KitchenConnection.BusinessLogic.Services.IServices;
public interface IRecipeService
{
    Task<RecipeDTO> Create(Guid userId, RecipeCreateDTO recipeToCreate);
    Task<RecipeDTO> Get(Guid recipeId);
    Task<List<RecipeDTO>> GetAll();
    Task<RecipeDTO> Update(RecipeUpdateDTO recipeToUpdate);
    Task<RecipeDTO> Delete(Guid recipeId);

    Task<RecipeNutrientsDTO> GetRecipeNutrients(Guid recipeId);
    Task<Guid> GetRecipeCreatorId(Guid recipeId);
    Task<List<RecipeDTO>> GetPaginated(int page, int pageSize);
}