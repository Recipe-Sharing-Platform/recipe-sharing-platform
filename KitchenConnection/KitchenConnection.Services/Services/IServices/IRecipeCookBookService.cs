using KitchenConnection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices
{
    public interface IRecipeCookBookService
    {
        Task<List<Recipe>> GetRecipeCookBooks();

        Task<Recipe> GetRecipeCookBook(string id);

        Task DeleteRecipeCookBook(string id);
    }
}
