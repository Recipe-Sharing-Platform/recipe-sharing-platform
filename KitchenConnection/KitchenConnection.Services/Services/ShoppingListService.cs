using AutoMapper;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.DTOs.ShoppingCart;
using KitchenConnection.DataLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services
{
    public class ShoppingListService : IShoppingListService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public readonly IRecipeNutrientsService _nutrientsService;
        public ShoppingListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<ShoppingListCreateDTO> AddShoppingList(Guid userId, ShoppingListCreateDTO shoppingListCreate)
        {
            if (userId == null) return null;
            shoppingListCreate.UserId = userId;

            var existingList = await _unitOfWork.Repository<ShoppingList>().GetByCondition(x => x.UserId == userId).FirstOrDefaultAsync();
            if (existingList != null)
            {
                return null;
            }

            //You have to first create the shoppinglist to not have problems with foreign key at ShoppingListItem
            var shoppingList = _mapper.Map<ShoppingList>(shoppingListCreate);
            shoppingList = await _unitOfWork.Repository<ShoppingList>().Create(shoppingList);
            _unitOfWork.Complete();

            var itemsToCheck = new List<ShoppingListItemDTO>();
            shoppingList.ShoppingListItems.ForEach(t =>
            {
                t.ShoppingListId = shoppingList.Id;
                itemsToCheck.Add(_mapper.Map<ShoppingListItemDTO>(t));
            });

            foreach (var item in itemsToCheck)
            {
                var existingItem = await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.Name == item.Name && x.ShoppingListId == shoppingList.Id).FirstOrDefaultAsync();
                if (existingItem == null)
                {
                    var shoppingItem = _mapper.Map<ShoppingListItem>(item);
                    await _unitOfWork.Repository<ShoppingListItem>().Create(shoppingItem);
                }
            }
            _unitOfWork.Complete();

            return _mapper.Map<ShoppingListCreateDTO>(shoppingList);
        }
        public async Task AddShoppingListItems(List<ShoppingListItemDTO> items)
        {
            foreach (var item in items)
            {
                var shoppingItem = _mapper.Map<ShoppingListItem>(item);
                await _unitOfWork.Repository<ShoppingListItem>().Create(shoppingItem);
            }
        }
    }
}

