using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.CollectionExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.DTOs.Collection;
using KitchenConnection.Models.DTOs.CookBook;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KitchenConnection.BusinessLogic.Services
{
    public class CollectionService : ICollectionService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public CollectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CollectionDTO>> GetAll(Guid userId)
        {
            var collection = await _unitOfWork.Repository<Collection>().GetAll()
                            .Include(x => x.Recipes)
                            .ThenInclude(x => x.Recipe)
                            .ThenInclude(r => r.Cuisine)
                            .Include(r => r.Recipes)
                            .ThenInclude(x => x.Recipe)
                            .ThenInclude(r => r.Tags)
                            .Include(r => r.Recipes)
                            .ThenInclude(x => x.Recipe)
                            .ThenInclude(r => r.Ingredients)
                            .Include(r => r.Recipes)
                            .ThenInclude(x => x.Recipe)
                            .ThenInclude(r => r.Instructions)
                            .Where(x => x.UserId == userId)
                            .ToListAsync();
            if (collection is null || collection.Count == 0) throw new CollectionsNotFoundException();


            collection.ForEach(x => x.Recipes.ForEach(x => { x.Recipe = _unitOfWork.Repository<Recipe>().GetById(y => y.Id == x.RecipeId).FirstOrDefault(); }));

            var collectionDTO = _mapper.Map<List<CollectionDTO>>(collection);


            collectionDTO.ForEach(async collection =>
            {
                var recipes = _unitOfWork.Repository<CollectionRecipe>().GetById(x => x.CollectionId == collection.Id).Count();
                if (recipes == 0) throw new RecipesNotFoundException();
                collection.NumberOfRecipes = recipes;
            });
            return collectionDTO;
        }
        public async Task<CollectionDTO> Get(Guid userId, Guid id)
        {
            var collection = await _unitOfWork.Repository<Collection>().GetById(x => x.Id == id)
                .Include(r => r.Recipes)
                .ThenInclude(x => x.Recipe)
                .ThenInclude(r => r.Cuisine)
                .Include(r => r.Recipes)
                .ThenInclude(x => x.Recipe)
                .ThenInclude(r => r.Tags)
                .Include(r => r.Recipes)
                .ThenInclude(x => x.Recipe)
                .ThenInclude(r => r.Ingredients)
                .Include(r => r.Recipes)
                .ThenInclude(x => x.Recipe)
                .ThenInclude(r => r.Instructions)
                .Where(x => x.UserId == userId).FirstOrDefaultAsync();

            if (collection is null) throw new CollectionNotFoundException(id);

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            collectionDTO.NumberOfRecipes = collection.Recipes.Count();


            return collectionDTO;
        }
        public async Task<CollectionDTO> Create(Guid userId, CollectionCreateRequestDTO collectionToCreateRequest)
        {
            CollectionCreateDTO collectionToCreate = new CollectionCreateDTO(collectionToCreateRequest, userId);
            var collection = _mapper.Map<Collection>(collectionToCreate);
            collection.Recipes = new List<CollectionRecipe>();

            collectionToCreate.Recipes.ForEach(recipeId =>
            {
                var recipe = _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(r => r.Id == recipeId, "User, Cuisine, Tags, Ingredients, Instructions").FirstOrDefault();
                if (recipe is null) throw new RecipeNotFoundException(recipeId);

                if (recipe != null)
                {
                    collection.Recipes.Add(new CollectionRecipe { Collection = collection, RecipeId = recipeId });
                }
            });

            collection = await _unitOfWork.Repository<Collection>().Create(collection);
            if (collection is null) throw new CollectionCouldNotBeCreatedException("Collection could not be created");
            _unitOfWork.Complete();

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            collectionDTO.NumberOfRecipes = collection.Recipes.Count();

            return collectionDTO;
        }
        public async Task<CollectionDTO> Update(Guid userId, CollectionUpdateDTO collectionToUpdate)
        {
            var collection = await _unitOfWork.Repository<Collection>().GetById(x => x.Id == collectionToUpdate.Id)
                .Include(r => r.Recipes)
                .ThenInclude(e => e.Recipe)
                .ThenInclude(r => r.Cuisine)
                .Include(r => r.Recipes)
                .ThenInclude(e => e.Recipe)
                .ThenInclude(r => r.Tags)
                .Include(r => r.Recipes)
                .ThenInclude(e => e.Recipe)
                .ThenInclude(r => r.Ingredients)
                .Include(r => r.Recipes)
                .ThenInclude(e => e.Recipe)
                .ThenInclude(r => r.Instructions)
                .Where(x => x.UserId == userId).FirstOrDefaultAsync();

            if (collection == null) throw new CollectionNotFoundException("User doesn't have access to update this collection");

            collection.Name = collectionToUpdate.Name;
            collection.Description = collectionToUpdate.Description;


            collection = _unitOfWork.Repository<Collection>().Update(collection);
            _unitOfWork.Complete();
            if (collection is null) throw new CollectionCouldNotBeUpdatedException("Collection could not be updated!");

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            collectionDTO.NumberOfRecipes = collection.Recipes.Count();

            return collectionDTO;
        }
        public async Task<CollectionDTO> Delete(Guid userId, Guid id)
        {
            var collection = await _unitOfWork.Repository<Collection>().GetById(x => x.Id == id).Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (collection == null) throw new CollectionNotFoundException("User doesn't have access to delete this collection");

            _unitOfWork.Repository<Collection>().Delete(collection);
            _unitOfWork.Complete();

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            return collectionDTO;
        }

        public async Task<List<CollectionDTO>> GetPaginated(int page, int pageSize)
        {
            var recipes = await _unitOfWork.Repository<Collection>().GetPaginated(page, pageSize)
                .Include(u => u.User).Include(r => r.Recipes).ToListAsync();

            return _mapper.Map<List<CollectionDTO>>(recipes);
        }
    }
}
