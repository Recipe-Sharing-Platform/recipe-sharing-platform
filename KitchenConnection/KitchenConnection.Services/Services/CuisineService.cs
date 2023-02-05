using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services
{
    public class CuisineService : ICuisineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CuisineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Cuisine> Create(Cuisine cuisineToCreate)
        {
            var cuisine = await _unitOfWork.Repository<Cuisine>().Create(cuisineToCreate);
            _unitOfWork.Complete();

            return cuisine;
        }
    }
}
