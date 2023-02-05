using AutoMapper;
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

        public readonly IRecipeNutrientsService _nutrientsService;
        public ShoppingListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<List<ShoppingListItemCreateDTO>> AddToShoppingList(Guid userId, List<ShoppingListItemCreateDTO> shoppingListCreate)
        {
            var shoppingListItems = shoppingListCreate.Select(item => new ShoppingListItemDTO(userId, item)).ToList();
            var entities = _mapper.Map<List<ShoppingListItem>>(shoppingListItems);

            await _unitOfWork.Repository<ShoppingListItem>().CreateRange(entities);
            _unitOfWork.Complete();

            return shoppingListCreate;
        }
        public async Task<bool> DeleteFromShoppingList(Guid userId, Guid shoppingListItemId)
        {
            var result = false;
            var shoppingListItem = await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.Id == shoppingListItemId && x.UserId == userId).FirstOrDefaultAsync();

            if (shoppingListItem == null)
            {
                throw new Exception("No shopping list item found with the specified id for the given user.");
            }

            _unitOfWork.Repository<ShoppingListItem>().Delete(shoppingListItem);
            result = _unitOfWork.Complete();
            return result;
        }
        public async Task<ShoppingListItem> GetShoppingListItemById(Guid userId, Guid shoppingListItemId)
        {
            var shoppingListItem = await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.Id == shoppingListItemId && x.UserId == userId).FirstOrDefaultAsync();

            if (shoppingListItem == null)
            {
                throw new Exception("No shopping list item found with the specified id for the given user.");
            }

            return shoppingListItem;
        }

        public async Task<string> GetShoppingListItemUrl(Guid userId, Guid shoppingListItemId)
        {
            var shoppingListItem = await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.Id == shoppingListItemId && x.UserId == userId).FirstOrDefaultAsync();

            if (shoppingListItem == null)
            {
                throw new Exception("No shopping list item found with the specified id for the given user.");
            }

            string groceryShopUrl = "http://www.walmart.com/search?q=";
            string itemName = shoppingListItem.Name;
            string[] itemNameWords = itemName.Split(' ');
            string encodedItemName = WebUtility.UrlEncode(string.Join("+", itemNameWords));
            string finalUrl = groceryShopUrl + encodedItemName;
     
            return finalUrl;
        }
        public async Task<List<ShoppingListItem>> GetShoppingListForUser(Guid userId)
        {
            return await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.UserId == userId).ToListAsync();
        }


    }
}
