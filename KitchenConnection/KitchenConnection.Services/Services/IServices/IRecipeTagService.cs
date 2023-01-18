using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices;
    public interface IRecipeTagService
    {
        Task<List<RecipeTag>> GetRecipeTags();
        Task<RecipeTag> GetRecipeTag(string id);
        Task UpdateRecipeTag(RecipeTag recipetagToUpdate);
        Task DeleteRecipeTag(string id);
        Task CreateRecipeTag(RecipeTag recipetagToCreate);
    }
