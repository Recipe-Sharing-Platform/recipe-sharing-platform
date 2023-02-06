using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Services.IServices {
    public interface IUserService
    {
        Task Create(User user);
    }
}
