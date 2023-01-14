using KitchenConnection.DataLayer.Models.Entities;
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
        Task<List<RecipeCookBook>> GetRecipeCookBooks();

        Task<RecipeCookBook> GetRecipeCookBook(string id);

        Task DeleteRecipeCookBook(string id);
    }
}
