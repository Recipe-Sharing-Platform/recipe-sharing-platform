using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.CollectionExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.CookBookExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.DTOs.CookBook;
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
            // TODO: Exception Handling when recipe is not added to cookbook
            var recipe = _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(r => r.Id == recipeId, "User, Cuisine, Tags, Ingredients, Instructions").FirstOrDefault();
            if (recipe != null && recipe.UserId == cookBook.UserId)
            {
                cookBook.Recipes.Add(recipe);
            }
        });

        cookBook = await _unitOfWork.Repository<CookBook>().Create(cookBook);
        if (cookBook is null) throw new CookBookCouldNotBeCreatedException("CookBook could not be created");

        _unitOfWork.Complete();

        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBook);
        cookBookDTO.NumberOfRecipes = cookBook.Recipes.Count();

        return cookBookDTO;
    }

    public async Task<List<CookBookDTO>> GetAll()
    {
        // TODO: Optimize method
        var cookBooks = await _unitOfWork.Repository<CookBook>().GetAll()
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Cuisine)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Tags)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Ingredients)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Instructions).ToListAsync();
        if (cookBooks is null || cookBooks.Count == 0) throw new CookBooksNotFoundException();

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
        if (cookBook is null) throw new CookBookNotFoundException(id);

        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBook);
        cookBookDTO.NumberOfRecipes = cookBook.Recipes.Count();

        return cookBookDTO;
    }

    public async Task<CookBookDTO> Update(Guid userId, CookBookUpdateDTO cookBookToUpdate)
    {
        var cookBook = await _unitOfWork.Repository<CookBook>().GetById(x => x.Id == cookBookToUpdate.Id)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Cuisine)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Tags)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Ingredients)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Instructions)
            .Where(x => x.UserId == userId).FirstOrDefaultAsync();
        if (cookBook == null) throw new CookBookNotFoundException("User doesn't have access to update this cookBook");

        cookBook.Name = cookBookToUpdate.Name;
        cookBook.Description = cookBookToUpdate.Description;


        cookBook = _unitOfWork.Repository<CookBook>().Update(cookBook);
        _unitOfWork.Complete();
        if (cookBook is null) throw new CookBookCouldNotBeUpdatedException("CookBook could not be updated!");


        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBook);
        cookBookDTO.NumberOfRecipes = cookBook.Recipes.Count();

        return cookBookDTO;
    }

    public async Task<CookBookDTO> Delete(Guid userId, Guid id)
    {
        var cookBook = await _unitOfWork.Repository<CookBook>().GetById(x => x.Id == id)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Cuisine)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Tags)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Ingredients)
            .Include(r => r.Recipes)
            .ThenInclude(r => r.Instructions)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        if (cookBook == null) throw new CollectionsNotFoundException("User doesn't have access to delete this cookBook");


        _unitOfWork.Repository<CookBook>().Delete(cookBook);
        _unitOfWork.Complete();

        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBook);
        cookBookDTO.NumberOfRecipes = cookBook.Recipes.Count();

        return cookBookDTO;
    }
    //TODO: Add controller for adding or deleting recipes from a cookbook
}

