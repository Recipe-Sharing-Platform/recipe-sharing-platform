using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices;
    public interface IRecipeTagService
    {
        Task<List<RecipeTag>> GetRecipeTags();
        Task<RecipeTag> GetRecipeTag(Guid id);
        Task<RecipeTag> UpdateRecipeTag(RecipeTag recipetagToUpdate);
        Task<bool> DeleteRecipeTag(Guid id);
        Task<RecipeTag> CreateRecipeTag(RecipeTag recipetagToCreate);
    }
