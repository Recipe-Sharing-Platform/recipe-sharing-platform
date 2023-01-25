using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices;
    public interface IRecipeCookBookService
    {
        Task<List<CookBookRecipe>> GetRecipeCookBooks();
        Task<CookBookRecipe> GetRecipeCookBook(Guid id);
        Task<bool> DeleteRecipeCookBook(Guid id);
        Task<CookBookRecipe> UpdateCookBookRecipe(CookBookRecipe cookBookRecipeToUpdate);
    }
