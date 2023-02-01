using AutoMapper;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.DTOs.CookBook;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KitchenConnection.BusinessLogic.Services;

public class CookBookService : ICookBookService
{
    public readonly IUnitOfWork _unitOfWork;
    public readonly IMapper _mapper;

    public CookBookService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CookBookDTO> Create(CookBookCreateRequestDTO cookBookToCreateRequest, Guid userId)
    {
        CookBookCreateDTO cookBookToCreate = new CookBookCreateDTO(cookBookToCreateRequest, userId);
        var cookBook = _mapper.Map<CookBook>(cookBookToCreate);
        cookBook.Recipes = new List<Recipe>();
        cookBookToCreate.Recipes.ForEach(recipeId =>
        {
            var recipe = _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(r => r.Id == recipeId, "User, Cuisine, Tags, Ingredients, Instructions").FirstOrDefault();
            if (recipe != null && recipe.UserId == cookBook.UserId)
            {
                cookBook.Recipes.Add(recipe);
            }
        });

        cookBook = await _unitOfWork.Repository<CookBook>().Create(cookBook);
        _unitOfWork.Complete();

        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBook);
        cookBookDTO.NumberOfRecipes = cookBook.Recipes.Count();

        return cookBookDTO;
    }

    public async Task<List<CookBookDTO>> GetAll()
    {
        var cookBooks =  await _unitOfWork.Repository<CookBook>().GetAll()
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Cuisine)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Tags)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Ingredients)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Instructions).ToListAsync();

        if (cookBooks == null) return null;
        
        var cookBooksDTO = _mapper.Map<List<CookBookDTO>>(cookBooks);
        cookBooksDTO.ForEach(cookBook =>
        {
            cookBook.NumberOfRecipes = cookBook.Recipes.Count();
        });

        return cookBooksDTO;
    }

    public async Task<CookBookDTO> Get(Guid id)
    {
        var cookBook = await _unitOfWork.Repository<CookBook>().GetById(x => x.Id == id)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Cuisine)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Tags)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Ingredients)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Instructions).FirstOrDefaultAsync();

        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBook);
        cookBookDTO.NumberOfRecipes = cookBook.Recipes.Count();

        return cookBookDTO;
    }

    public async Task<CookBookDTO> Update(CookBookUpdateDTO cookBookToUpdate)
    {
        var cookBook = await _unitOfWork.Repository<CookBook>().GetById(x => x.Id == cookBookToUpdate.Id)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Cuisine)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Tags)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Ingredients)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Instructions).FirstOrDefaultAsync();

        if (cookBook == null) return null;
        
        cookBook.Name = cookBookToUpdate.Name;
        cookBook.Description = cookBookToUpdate.Description;
        

        _unitOfWork.Repository<CookBook>().Update(cookBook);
        _unitOfWork.Complete();

        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBook);
        cookBookDTO.NumberOfRecipes = cookBook.Recipes.Count();

        return cookBookDTO;
    }

    public async Task<CookBookDTO> Delete(Guid id)
    {
        var cookBook = await _unitOfWork.Repository<CookBook>().GetById(x => x.Id == id)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Cuisine)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Tags)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Ingredients)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Instructions).FirstOrDefaultAsync();

        if (cookBook == null) return null;
        
        _unitOfWork.Repository<CookBook>().Delete(cookBook);
        _unitOfWork.Complete();

        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBook);
        cookBookDTO.NumberOfRecipes = cookBook.Recipes.Count();

        return cookBookDTO;
    }
    //TODO: Add controller for adding or deleting recipes from a cookbook
}

