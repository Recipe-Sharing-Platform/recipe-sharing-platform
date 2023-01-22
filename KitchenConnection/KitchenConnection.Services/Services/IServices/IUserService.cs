using KitchenConnection.DataLayer.Models.DTOs;
using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices
{
    public interface IUserService
    {
        Task<User> Create(UserCreateDTO userToCreate);
        Task<User> GetUser(Guid id);
        Task<User> UpdateUser(UserDTO userToUpdate);
        Task<bool> DeleteUser(Guid id);
    }
}
