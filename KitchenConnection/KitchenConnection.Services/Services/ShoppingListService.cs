using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.ShoppingListExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.DTOs.ShoppingCart;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services
{
    public class ShoppingListService : IShoppingListService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public ShoppingListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<List<ShoppingListItemCreateDTO>> AddToShoppingList(Guid userId, List<ShoppingListItemCreateDTO> shoppingListCreate)
        {
            var shoppingListItems = shoppingListCreate.Select(item => new ShoppingListItemDTO(userId, item)).ToList();
            var entities = _mapper.Map<List<ShoppingListItem>>(shoppingListItems);

            try
            {
                await _unitOfWork.Repository<ShoppingListItem>().CreateRange(entities);
                _unitOfWork.Complete();

                return shoppingListCreate;
            }
            catch (Exception ex)
            {
                throw new ShoppingListItemCouldNotBeAddedException("Failed to add shopping list item");
            }
        }
        public async Task<bool> DeleteFromShoppingList(Guid userId, Guid shoppingListItemId)
        {
            var result = false;
            var shoppingListItem = await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.Id == shoppingListItemId && x.UserId == userId).FirstOrDefaultAsync();

            if (shoppingListItem == null)
            {
                throw new ShoppingListItemNotFoundException(shoppingListItemId);
            }

            try
            {
                _unitOfWork.Repository<ShoppingListItem>().Delete(shoppingListItem);
                result = _unitOfWork.Complete();
                return result;
            }
            catch(Exception ex)
            {
                throw new ShoppingListItemCouldNotBeDeletedException("ShoppingList item could not be deleted");
            }
        }
        public async Task<ShoppingListItem> GetShoppingListItemById(Guid userId, Guid shoppingListItemId)
        {
            var shoppingListItem = await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.Id == shoppingListItemId && x.UserId == userId).FirstOrDefaultAsync();

            if (shoppingListItem == null)
            {
                throw new ShoppingListItemNotFoundException(shoppingListItemId);
            }

            return shoppingListItem;
        }

        public async Task<string> GetShoppingListItemUrl(Guid userId, Guid shoppingListItemId)
        {
            var shoppingListItem = await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.Id == shoppingListItemId && x.UserId == userId).FirstOrDefaultAsync();

            if (shoppingListItem == null)
            {
                throw new ShoppingListItemNotFoundException(shoppingListItemId);
            }

            string groceryShopUrl = "http://www.walmart.com/search?q=";
            string itemName = shoppingListItem.Name;
            string[] itemNameWords = itemName.Split(' ');
            string encodedItemName = string.Join("+", itemNameWords);
            string finalUrl = groceryShopUrl + encodedItemName;
     
            return finalUrl;
        }
        public async Task<List<ShoppingListItem>> GetShoppingListForUser(Guid userId)
        {
            try
            {
                return await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.UserId == userId).ToListAsync();
            }
            catch(Exception ex)
            {
                throw new ShoppingListItemsNotFoundException();
            }
        }


    }
}
