using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
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

        public async Task<CookBookRecipe> CreateRecipeCookBook(CookBookRecipe cookBookRecipe)
        {
            await _unitOfWork.Repository<CookBookRecipe>().Create(cookBookRecipe);

            var res = _unitOfWork.Complete();

            if (res)
            {
                return cookBookRecipe;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<CookBookRecipe>> GetRecipeCookBooks()
        {
            return await _unitOfWork.Repository<CookBookRecipe>().GetAll().Include(x => x.CookBookId).ToListAsync();
        }

        public async Task<CookBookRecipe> GetRecipeCookBook(Guid id)
        {

            Expression<Func<CookBookRecipe, bool>> expression = x => x.Id == id;
            var recipeCookBook = await _unitOfWork.Repository<CookBookRecipe>().GetById(expression).FirstOrDefaultAsync();

            return recipeCookBook;
        }

        public async Task<bool> DeleteRecipeCookBook(Guid id)
        {
            var recipeCookBook = await GetRecipeCookBook(id);

            _unitOfWork.Repository<CookBookRecipe>().Delete(recipeCookBook);

            return _unitOfWork.Complete();
            
        }

        public async Task<CookBookRecipe> UpdateCookBookRecipe(CookBookRecipe cookBookRecipeToUpdate)
        {
            CookBookRecipe? cookBookRecipe = await GetRecipeCookBook(cookBookRecipeToUpdate.Id);

            cookBookRecipe.Recipe=cookBookRecipeToUpdate.Recipe;
            cookBookRecipe.CookBook = cookBookRecipeToUpdate.CookBook;
            
            _unitOfWork.Repository<CookBookRecipe>().Update(cookBookRecipe);

            var res= _unitOfWork.Complete();

            if (res)
            {
                return cookBookRecipe;
            }
            else
            {
                return null;
            }
        } 
    }


