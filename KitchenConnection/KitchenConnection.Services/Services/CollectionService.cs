using AutoMapper;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.DTOs.Collection;
using KitchenConnection.DataLayer.Models.DTOs.CookBook;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<CollectionDTO>> GetAll()
        {
            var collection = await _unitOfWork.Repository<Collection>().GetAll()
                .Include(x => x.Recipes)
                .ThenInclude(x => x.Recipe)
                .ThenInclude(r => r.Cuisine)
                .Include(r => r.Recipes)
                .ThenInclude(x=>x.Recipe)
                .ThenInclude(r => r.Tags)
                .Include(r => r.Recipes)
                .ThenInclude(x=>x.Recipe)
                .ThenInclude(r => r.Ingredients)
                .Include(r => r.Recipes)
                .ThenInclude(x=>x.Recipe)
                .ThenInclude(r => r.Instructions).ToListAsync();

            collection.ForEach(x => x.Recipes.ForEach(x => { x.Recipe =  _unitOfWork.Repository<Recipe>().GetById(y => y.Id == x.RecipeId).FirstOrDefault(); }));

            var collectionDTO = _mapper.Map<List<CollectionDTO>>(collection);


            collectionDTO.ForEach(async collection =>
            {
                var recipes = _unitOfWork.Repository<CollectionRecipe>().GetById(x => x.CollectionId == collection.Id).Count();
                collection.NumberOfRecipes= recipes;
            });
            return collectionDTO;
        }
        public async Task<CollectionDTO> Get(Guid id)
        {
            var collection = await _unitOfWork.Repository<Collection>().GetById(x => x.Id == id)
                .Include(r => r.Recipes)
                .ThenInclude(x=>x.Recipe)
            .ThenInclude(r => r.Cuisine)
            .Include(r => r.Recipes)
            .ThenInclude(x=>x.Recipe)
            .ThenInclude(r => r.Tags)
            .Include(r => r.Recipes)
            .ThenInclude(x=>x.Recipe)
            .ThenInclude(r => r.Ingredients)
            .Include(r => r.Recipes)
            .ThenInclude(x=>x.Recipe)
            .ThenInclude(r => r.Instructions).FirstOrDefaultAsync();

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            collectionDTO.NumberOfRecipes = collection.Recipes.Count();


            return collectionDTO;
        }
        public async Task<CollectionDTO> Create(CollectionCreateRequestDTO collectionToCreateRequest, Guid userId)
        {
            CollectionCreateDTO collectionToCreate = new CollectionCreateDTO(collectionToCreateRequest, userId);
            var collection = _mapper.Map<Collection>(collectionToCreate);
            collection.Recipes = new List<CollectionRecipe>();
 
            collectionToCreate.Recipes.ForEach(recipeId =>
            {
                var recipe = _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(r => r.Id == recipeId, "User, Cuisine, Tags, Ingredients, Instructions").FirstOrDefault();
                if (recipe != null)
                {
                    collection.Recipes.Add(new CollectionRecipe { Collection = collection, RecipeId = recipeId });
                }
            });

            collection = await _unitOfWork.Repository<Collection>().Create(collection);
            _unitOfWork.Complete();

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            collectionDTO.NumberOfRecipes = collection.Recipes.Count();

            return collectionDTO;
        }
        public async Task<CollectionDTO> Update(CollectionUpdateDTO collectionToUpdate)
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
                .ThenInclude(r => r.Instructions).FirstOrDefaultAsync();

            if (collection == null) return null;

            collection.Name = collectionToUpdate.Name;
            collection.Description = collectionToUpdate.Description;


            _unitOfWork.Repository<Collection>().Update(collection);
            _unitOfWork.Complete();

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            collectionDTO.NumberOfRecipes = collection.Recipes.Count();

            return collectionDTO;
        }
        public async Task<CollectionDTO> Delete(Guid id)
        {
            var collection = await _unitOfWork.Repository<Collection>().GetById(x=>x.Id == id).FirstOrDefaultAsync(); 

            _unitOfWork.Repository<Collection>().Delete(collection);
            _unitOfWork.Complete();

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            return collectionDTO;
        }
    }
}
