
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KitchenConnection.BusinessLogic.Services;
public class RecipeService : IRecipeService {
    public readonly IUnitOfWork _unitOfWork;
    public RecipeService(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<Recipe>> GetRecipes() {
        return await _unitOfWork.Repository<Recipe>().GetAll().ToListAsync();
    }
}
