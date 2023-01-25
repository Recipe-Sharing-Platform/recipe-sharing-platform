using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Services.IServices;
public interface IRecipeService
{
    Task<RecipeDTO> Create(RecipeCreateDTO recipeToCreate);
    Task<RecipeDTO> Get(Guid id);
    Task<List<RecipeDTO>> GetAll();
    Task<RecipeDTO> Update(RecipeUpdateDTO recipeToUpdate);
    Task Delete(Guid id);
}