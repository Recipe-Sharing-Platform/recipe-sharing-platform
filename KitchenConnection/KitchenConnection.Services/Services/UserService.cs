using AutoMapper;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.DTOs;
using KitchenConnection.DataLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Create(UserCreateDTO userToCreate)
        {
            User? user=_mapper.Map<User>(userToCreate);

            _unitOfWork.Repository<User>().Create(user);

            var res=_unitOfWork.Complete();

            if (res)
            {
                return user;
            }
            else 
            {
                return null;
            }
        }

        public async Task<User> GetUser(Guid id)
        {
            Expression<Func<User, bool>> expression=x=>x.Id==id;

            var user = await _unitOfWork.Repository<User>().GetById(expression).FirstOrDefaultAsync();

            return user;            
        }

        public async Task<User> UpdateUser(UserDTO userToUpdate)
        {
            User? user = await GetUser(userToUpdate.Id);

            user.FirstName = userToUpdate.FirstName;
            user.LastName = userToUpdate.LastName;
            user.PhoneNumber = userToUpdate.PhoneNumber;
            user.DateOfBirth = userToUpdate.DateOfBirth;
            user.Email = userToUpdate.Email;
            user.Gender = userToUpdate.Gender;

            _unitOfWork.Repository<User>().Update(user);

            _unitOfWork.Complete();

            return user;//return updated user;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = await GetUser(id);

            _unitOfWork.Repository<User>().Delete(user);

            var res= _unitOfWork.Complete();

            return res;
        }



    }
}
