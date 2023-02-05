using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices
{
    public interface ICuisineService
    {
        Task<Cuisine> Create(Cuisine cuisineToCreate);
    }
}
