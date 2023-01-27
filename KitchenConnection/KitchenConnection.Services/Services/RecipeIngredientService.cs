using AutoMapper;
using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
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
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RecipeIngredientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<RecipeIngredient>> GetRecipeIngredients()
        {
            return await _unitOfWork.Repository<RecipeIngredient>().GetAll().ToListAsync();
        }

        public async Task<RecipeIngredient> GetRecipeIngredient(Guid id)
        {

            Expression<Func<RecipeIngredient, bool>> expression = x => x.Id == id;
            var recipeIngredient = await _unitOfWork.Repository<RecipeIngredient>().GetById(expression).FirstOrDefaultAsync();

            return recipeIngredient;
        }

        public async Task<RecipeIngredient> UpdateRecipeIngredient(RecipeIngredientDTO recipeIngredientToUpdate)
        {
            RecipeIngredient? recipeIngredient = await GetRecipeIngredient(recipeIngredientToUpdate.Id);

            recipeIngredient.Name = recipeIngredientToUpdate.Name;
            recipeIngredient.Amount = recipeIngredientToUpdate.Amount;
            recipeIngredient.Unit = recipeIngredientToUpdate.Unit;

            _unitOfWork.Repository<RecipeIngredient>().Update(recipeIngredient);

            var res= _unitOfWork.Complete();

            if (res)
            {
                return recipeIngredient;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteRecipeIngredient(Guid id)
        {
            var recipeIngredient = await GetRecipeIngredient(id);

            _unitOfWork.Repository<RecipeIngredient>().Delete(recipeIngredient);

            return _unitOfWork.Complete();
        }

        public async Task<RecipeIngredient> CreateRecipeIngredient(RecipeIngredientCreateDTO recipeIngredientToCreate)
        {
            var recipeIngredient = _mapper.Map<RecipeIngredient>(recipeIngredientToCreate);

            await _unitOfWork.Repository<RecipeIngredient>().Create(recipeIngredient);

            var res= _unitOfWork.Complete();

            if (res)
            {
                return recipeIngredient;
            }

            else
            {
                return null;
            }
        }
    }
}
