using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Services.IServices {
    public interface ICuisineService
    {
        Task<Cuisine> Create(Cuisine cuisineToCreate);
    }
}
