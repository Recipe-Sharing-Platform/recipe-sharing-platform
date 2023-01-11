using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices
{
    public interface ICookBookService
    {
        Task<List<CookBook>> GetCookBooks();

        Task<CookBook> GetCookBook(string id);

        Task UpdateCookBook(CookBook cookbookToUpdate);

        Task DeleteCookBook(string id);
    }
}
