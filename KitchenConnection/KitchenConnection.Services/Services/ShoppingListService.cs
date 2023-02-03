//using AutoMapper;
//using KitchenConnection.BusinessLogic.Services.IServices;
//using KitchenConnection.DataLayer.Data.UnitOfWork;
//using KitchenConnection.DataLayer.Models.DTOs.ShoppingCart;
//using KitchenConnection.DataLayer.Models.Entities;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace KitchenConnection.BusinessLogic.Services
//{
//    public class ShoppingListService : IShoppingListService
//    {
//        public readonly IUnitOfWork _unitOfWork;
//        public readonly IMapper _mapper;

//        public readonly IRecipeNutrientsService _nutrientsService;
//        public ShoppingListService(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;

//        }
//        public async Task<ShoppingListCreateDTO> AddShoppingList(Guid userId, ShoppingListCreateDTO shoppingListCreate)
//        {
//            if (userId == null) return null;
//            shoppingListCreate.UserId = userId;

//            var existingList = await _unitOfWork.Repository<ShoppingList>().GetByCondition(x => x.UserId == userId).FirstOrDefaultAsync();
//            if (existingList != null)
//            {
//                return null;
//            }

//            //You have to first create the shoppinglist to not have problems with foreign key at ShoppingListItem
//            var shoppingList = _mapper.Map<ShoppingList>(shoppingListCreate);
//            shoppingList = await _unitOfWork.Repository<ShoppingList>().Create(shoppingList);
//            _unitOfWork.Complete();

//            var itemsToCheck = new List<ShoppingListItemDTO>();
//            shoppingList.ShoppingListItems.ForEach(t =>
//            {
//                t.ShoppingListId = shoppingList.Id;
//                itemsToCheck.Add(_mapper.Map<ShoppingListItemDTO>(t));
//            });

//            foreach (var item in itemsToCheck)
//            {
//                var existingItem = await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.Name == item.Name && x.ShoppingListId == shoppingList.Id).FirstOrDefaultAsync();
//                if (existingItem == null)
//                {
//                    var shoppingItem = _mapper.Map<ShoppingListItem>(item);
//                    await _unitOfWork.Repository<ShoppingListItem>().Create(shoppingItem);
//                }
//            }
//            _unitOfWork.Complete();

//            return _mapper.Map<ShoppingListCreateDTO>(shoppingList);
//        }
//        public async Task AddShoppingListItems(List<ShoppingListItemDTO> items)
//        {
//            foreach (var item in items)
//            {
//                var shoppingItem = _mapper.Map<ShoppingListItem>(item);
//                await _unitOfWork.Repository<ShoppingListItem>().Create(shoppingItem);
//            }
//        }

//        public async Task<ShoppingListCreateDTO> UpdateShoppingList(Guid userId, ShoppingListCreateDTO shoppingListCreate)
//        {
//            if (userId == null) return null;
//            shoppingListCreate.UserId = userId;

//            var existingList = await _unitOfWork.Repository<ShoppingList>().GetByCondition(x => x.UserId == userId).FirstOrDefaultAsync();
//            if (existingList == null)
//            {
//                return null;
//            }

//            existingList = _mapper.Map(shoppingListCreate, existingList);

//            _unitOfWork.Repository<ShoppingList>().Update(existingList);
//            _unitOfWork.Complete();

//            var itemsToCheck = new List<ShoppingListItemDTO>();
//            shoppingListCreate.ShoppingListItems.ForEach(t =>
//            {
//                t.ShoppingListId = existingList.Id;
//                itemsToCheck.Add(t);
//            });

//            foreach (var item in itemsToCheck)
//            {
//                var existingItem = await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.Name == item.Name && x.ShoppingListId == existingList.Id).FirstOrDefaultAsync();
//                if (existingItem == null)
//                {
//                    var shoppingItem = _mapper.Map<ShoppingListItem>(item);
//                    await _unitOfWork.Repository<ShoppingListItem>().Create(shoppingItem);
//                }
//                else
//                {
//                    if (existingItem.ShoppingList.UserId != userId)
//                    {
//                        continue;
//                    }
//                    existingItem = _mapper.Map(item, existingItem);
//                    _unitOfWork.Repository<ShoppingListItem>().Update(existingItem);
//                }
//            }
//            _unitOfWork.Complete();

//            return _mapper.Map<ShoppingListCreateDTO>(existingList);
//        }


//    }
//}

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
        public async Task<List<ShoppingListItem>> GetShoppingListForUser(Guid userId)
        {
            return await _unitOfWork.Repository<ShoppingListItem>().GetByCondition(x => x.UserId == userId).ToListAsync();
        }


    }
}

