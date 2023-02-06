using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.CuisineExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.DTOs.Collection;
using KitchenConnection.Models.DTOs.Cuisine;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KitchenConnection.BusinessLogic.Services {
    public class CuisineService : ICuisineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CuisineService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CuisineDTO> Create(Cuisine cuisineToCreate)
        {
            var cuisine = await _unitOfWork.Repository<Cuisine>().Create(cuisineToCreate);

            if (cuisine is null) throw new CuisineCouldNotBeCreatedException("Cuisine could not be created!");

            _unitOfWork.Complete();

            return _mapper.Map<CuisineDTO>(cuisine);
        }

        public async Task<CuisineDTO> Get(Guid cuisineId)
        {
            var cuisine = await _unitOfWork.Repository<Cuisine>().GetByConditionWithIncludes(c => c.Id == cuisineId, "Recipes").FirstOrDefaultAsync();

            if (cuisine is null) throw new CuisineNotFoundException(cuisineId);

            return _mapper.Map<CuisineDTO>(cuisine);
        }

        public async Task<List<CuisineDTO>> GetAll()
        {
            var cuisines = await _unitOfWork.Repository<Cuisine>().GetAll().ToListAsync();

            if (cuisines is null || cuisines.Count == 0) throw new CuisinesNotFoundException("Cuisines not found!");

            return _mapper.Map<List<CuisineDTO>>(cuisines);
        }

        public async Task<List<CuisineDTO>> GetPaginated(int page, int pageSize)
        {
            var cuisines = await _unitOfWork.Repository<Cuisine>().GetPaginated(page, pageSize)
               .Include(c => c.Recipes).ToListAsync();

            if (cuisines is null) throw new CuisinesNotFoundException("Cuisines not found!");

            return _mapper.Map<List<CuisineDTO>>(cuisines);
        }

        public async Task<CuisineDTO> Delete(Guid cuisineId)
        {
            var cuisine = await _unitOfWork.Repository<Cuisine>().GetById(c => c.Id == cuisineId).FirstOrDefaultAsync();

            if (cuisine is null) throw new CuisineNotFoundException($"Cuisine not found: {cuisineId}");

            _unitOfWork.Repository<Cuisine>().Delete(cuisine);
            _unitOfWork.Complete();

            return _mapper.Map<CuisineDTO>(cuisine);
        }

        public async Task<CuisineDTO> Update(CuisineUpdateDTO cuisineToUpdate)
        {
            var cuisine = await _unitOfWork.Repository<Cuisine>().GetById(c => c.Id == cuisineToUpdate.Id).FirstOrDefaultAsync();

            if (cuisine is null) throw new CuisineNotFoundException(cuisineToUpdate.Id);

            cuisine.Name = cuisineToUpdate.Name;

            cuisine = _unitOfWork.Repository<Cuisine>().Update(cuisine);
            if (cuisine is null) throw new CuisineCouldNotBeUpdatedException("Cusine could not be updated!");
            _unitOfWork.Complete();

            return _mapper.Map<CuisineDTO>(cuisine);
        }
    }
}
