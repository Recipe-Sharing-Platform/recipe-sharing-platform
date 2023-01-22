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

namespace KitchenConnection.BusinessLogic.Services
{
    public class CookBookService : ICookBookService
    {
        public readonly IUnitOfWork _unitOfWork;
        public CookBookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<CookBook>> GetCookBooks()
        {
            return await _unitOfWork.Repository<CookBook>().GetAll().ToListAsync();
        }

        public async Task<CookBook> GetCookBook(Guid id)
        {

            Expression<Func<CookBook, bool>> expression = x => x.Id == id;
            var cookbook = _unitOfWork.Repository<CookBook>().GetById(expression);

            return (CookBook)cookbook;
        }

        public async Task<CookBook> UpdateCookBook(CookBook cookbookToUpdate)
        {
            CookBook? cookBook = await GetCookBook(cookbookToUpdate.Id);

            cookBook.Name = cookbookToUpdate.Name;
            cookBook.Description = cookbookToUpdate.Description;
            

            _unitOfWork.Repository<CookBook>().Update(cookBook);

            var res=_unitOfWork.Complete();

            if (res)
            {
                return cookBook;//return updated cookbook
            }

            else
            {
                return null;//couldn't update
            }
        }

        public async Task<bool> DeleteCookBook(Guid id)
        {
            var cookBook = await GetCookBook(id);

            _unitOfWork.Repository<CookBook>().Delete(cookBook);

            return _unitOfWork.Complete();
        }

        public async Task<CookBook> CreateCookBook(CookBook cookBookToCreate) 
        {
            await _unitOfWork.Repository<CookBook>().Create(cookBookToCreate);
            
            var res=_unitOfWork.Complete();

            if (res)
            {
                return cookBookToCreate;
            }
            else
            {
                return null;
            }
        }
    }
}