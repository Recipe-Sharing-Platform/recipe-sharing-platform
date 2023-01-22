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
        Task<CookBook> CreateCookBook(CookBook cookBookToCreate);
        Task<List<CookBook>> GetCookBooks();

        Task<CookBook> GetCookBook(Guid id);

        Task<CookBook> UpdateCookBook(CookBook cookbookToUpdate);

        Task<bool> DeleteCookBook(Guid id);
    }
}