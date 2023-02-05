using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.CollectionExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.CookBookExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.CookBookExceptions;
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
        cookBook.Recipes = await _unitOfWork.Repository<Recipe>().GetByCondition(x => cookBookToCreate.Recipes.Contains(x.Id) && x.UserId == userId && x.CookBook == null).ToListAsync();
        if (!cookBook.Recipes.Any()) throw new CookBookEmptyException("None of the recipes mentioned can be in this cookbook");
        cookBook = await _unitOfWork.Repository<CookBook>().Create(cookBook);
        if (cookBook is null) throw new CookBookCouldNotBeCreatedException("CookBook could not be created");

        _unitOfWork.Complete();

        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBook);
        cookBookDTO.NumberOfRecipes = cookBook.Recipes.Count;

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

    public async Task<CookBookDTO> Update(CookBookUpdateDTO cookBookToUpdate, Guid userId)
    {
        var cookBook = _unitOfWork.Repository<CookBook>().GetByCondition(x => x.Id == cookBookToUpdate.Id && x.UserId == userId);
        
        if (!cookBook.Any()) return null!;

        var cookBookWithIncludes = await cookBook
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

        if (cookBookWithIncludes is null) return null;

        cookBookWithIncludes.Name = cookBookToUpdate.Name;
        cookBookWithIncludes.Description = cookBookToUpdate.Description;
        

        _unitOfWork.Repository<CookBook>().Update(cookBookWithIncludes);
        _unitOfWork.Complete();
        if (cookBook is null) throw new CookBookCouldNotBeUpdatedException("CookBook could not be updated!");


        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBookWithIncludes);
        cookBookDTO.NumberOfRecipes = cookBookWithIncludes.Recipes.Count;

        return cookBookDTO;
    }

    public async Task<CookBookDTO> Delete(Guid id, Guid userId)
    {
        var cookBook = await _unitOfWork.Repository<CookBook>().GetById(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();

        if (cookBook == null) throw new CookBookNotFoundException("Cook Book was not found");

        _unitOfWork.Repository<CookBook>().Delete(cookBook);
        _unitOfWork.Complete();

        var cookBookDTO = _mapper.Map<CookBookDTO>(cookBook);
        cookBookDTO.NumberOfRecipes = cookBook.Recipes.Count;

        return cookBookDTO;
    }
    //TODO: Add controller for adding or deleting recipes from a cookbook
}