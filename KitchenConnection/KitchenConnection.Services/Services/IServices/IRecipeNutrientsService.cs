using KitchenConnection.DataLayer.Models.DTOs.Nutrients;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices;

public interface IRecipeNutrientsService
{
    Task<RecipeNutrientsDTO> GetNutrients(List<RecipeIngredientDTO> ingredients);
}
