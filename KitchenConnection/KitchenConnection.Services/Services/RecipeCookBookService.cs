using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services;

    public class RecipeCookBookService : IRecipeCookBookService
    {
        public readonly IUnitOfWork _unitOfWork;
        public RecipeCookBookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<RecipeCookBook>> GetRecipeCookBooks()
        {
            return await _unitOfWork.Repository<RecipeCookBook>().GetAll().Include(x => x.CookBookId).ToListAsync();
        }

        public async Task<RecipeCookBook> GetRecipeCookBook(string id)
        {

            Expression<Func<RecipeCookBook, bool>> expression = x => x.Id == id;
            var recipeCookBook = await _unitOfWork.Repository<RecipeCookBook>().GetById(expression).FirstOrDefaultAsync();

            return recipeCookBook;
        }


        public async Task DeleteRecipeCookBook(string id)
        {
            var recipeCookBook = await GetRecipeCookBook(id);

            _unitOfWork.Repository<RecipeCookBook>().Delete(recipeCookBook);

            _unitOfWork.Complete();
        }
}

