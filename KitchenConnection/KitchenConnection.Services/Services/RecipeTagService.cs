using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services;
    public class RecipeTagService : IRecipeTagService
    {

    public readonly IUnitOfWork _unitOfWork;
    public RecipeTagService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<RecipeTag>> GetRecipeTags()
    {
        return await _unitOfWork.Repository<RecipeTag>().GetAll().ToListAsync();
    }

    public async Task<RecipeTag> GetRecipeTag(string id)
    {

        Expression<Func<RecipeTag, bool>> expression = x => x.Id == id;
        var tag = await _unitOfWork.Repository<RecipeTag>().GetById(expression).FirstOrDefaultAsync();

        return tag;
    }

    public async Task UpdateRecipeTag(RecipeTag recipeTagToUpdate)
    {
        RecipeTag? recipetag = await GetRecipeTag(recipeTagToUpdate.Id);

        recipetag.TagId = recipeTagToUpdate.Id;
        recipetag.RecipeId= recipeTagToUpdate.Id;

        _unitOfWork.Repository<RecipeTag>().Update(recipetag);

        _unitOfWork.Complete();
    }

    public async Task DeleteRecipeTag(string id)
    {
        var recipetag = await GetRecipeTag(id);

        _unitOfWork.Repository<RecipeTag>().Delete(recipetag);

        _unitOfWork.Complete();
    }

    public async Task CreateRecipeTag(RecipeTag recipetagToCreate)
    {
        await _unitOfWork.Repository<RecipeTag>().Create(recipetagToCreate);
        _unitOfWork.Complete();
    }
}

