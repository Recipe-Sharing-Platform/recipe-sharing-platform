using KitchenConnection.Models.DTOs.Cuisine;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Services.IServices {
    public interface ICuisineService
    {
        Task<CuisineDTO> Create(Cuisine cuisineToCreate);
        Task<CuisineDTO> Get(Guid cuisineId);
        Task<List<CuisineDTO>> GetAll();
        Task<CuisineDTO> Update(CuisineUpdateDTO cuisineToUpdate);
        Task<CuisineDTO> Delete(Guid cuisineId);

        Task<List<CuisineDTO>> GetPaginated(int page, int pageSize);
    }
}
