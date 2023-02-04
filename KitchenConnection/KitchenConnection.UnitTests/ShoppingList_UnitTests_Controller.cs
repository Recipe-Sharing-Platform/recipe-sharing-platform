using AutoMapper;
using KitchenConnecition.DataLayer.Hubs;
using KitchenConnection.BusinessLogic.Helpers;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Controllers;
using KitchenConnection.DataLayer.Models.DTOs.CookBook;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs.Review;
using KitchenConnection.DataLayer.Models.DTOs.ShoppingCart;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;


namespace KitchenConnection.UnitTesting
{
    public class ShoppingList_UnitTests_Controller
    {
        private readonly Mock<IShoppingListService> _shoppingListMockService;
        private readonly IMapper _mapper;
        public Guid shopListId1 = Guid.NewGuid();
        private Guid shopListId2 = Guid.NewGuid();
        private Guid shopListId3 = Guid.NewGuid();
        public Guid userId2 = Guid.NewGuid();
        public Guid userId3 = Guid.NewGuid();
        private static Guid recipeId1 = Guid.NewGuid();
        private Guid recipeId2 = Guid.NewGuid();
        private Guid recipeId3 = Guid.NewGuid();

        private User user1 = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "UserName1",
            LastName = "User Surname1",
            Gender = "Male",
            Email = "randomEmail@gjirafa.com",
            PhoneNumber = "44123123",
            DateOfBirth = new DateTime(),
            Recipes = new List<Recipe>(),
            Reviews = new List<Review>(),           
        };

        public ShoppingList_UnitTests_Controller()
        {
            var mapperConfiguration = new MapperConfiguration(
             mc => mc.AddProfile(new AutoMapperConfigurations()));

            _mapper = mapperConfiguration.CreateMapper();
            _shoppingListMockService = new Mock<IShoppingListService>();
        }

        public async void AddToShoppingList_ShouldReturnRecipe()
        {
            var shoppingList = GetMockShoppingListItems();
            List<ShoppingListItemCreateDTO> listToCreate = new List<ShoppingListItemCreateDTO>();
            listToCreate.Add(_mapper.Map<ShoppingListItemCreateDTO>(shoppingList[0]));

            _shoppingListMockService.Setup(x => x.AddToShoppingList(user1.Id,listToCreate).Result)
               .Returns(listToCreate);

            var shoppingListController = new ShoppingListController(_shoppingListMockService.Object);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            shoppingListController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            ActionResult<List<ShoppingListItemCreateDTO>> actionResult = await shoppingListController.AddToShoppingList(listToCreate);

            List<ShoppingListItemCreateDTO> result = (List<ShoppingListItemCreateDTO>)(actionResult.Result as OkObjectResult).Value;

            //assert                      
            Assert.NotNull(result);
            Assert.Equal(result, listToCreate);
            Assert.True(result == listToCreate);
        }

        [Fact]
        public async void GetShoppingListItem_ShouldReturnShoppingListItem()
        {
            //arrange
            var shoppingList = GetMockShoppingListItems();         

            _shoppingListMockService.Setup(x => x.GetShoppingListItemById(user1.Id,shoppingList[0].Id).Result)
               .Returns(shoppingList[0]);

            var shoppingListController = new ShoppingListController(_shoppingListMockService.Object);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            shoppingListController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            ActionResult<ShoppingListItem> actionResult = await shoppingListController.GetShoppingListItem(shoppingList[0].Id);

            ShoppingListItem result = (ShoppingListItem)(actionResult.Result as OkObjectResult).Value;

            //assert                      
            Assert.NotNull(result);
            Assert.Equal(result, shoppingList[0]);
            Assert.True(result == shoppingList[0]);
        }

        [Fact]
        public async void DeleteShoppingListItemById_ShouldReturnDeletedReview()
        {

            //arrange
            var shoppingList = GetMockShoppingListItems();

            _shoppingListMockService.Setup(x => x.DeleteFromShoppingList(user1.Id, shoppingList[0].Id).Result)
               .Returns(true);

            var shoppingListController = new ShoppingListController(_shoppingListMockService.Object);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            shoppingListController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<bool> actionResult = await shoppingListController.DeleteShoppingListItems(shoppingList[0].Id);

            bool result = (bool)(actionResult.Result as OkObjectResult).Value;

            //assert                      
            Assert.NotNull(result);
            Assert.Equal(result, true);
            Assert.True(result==true);
        }

        [Fact]
        public async void GetShoppingList_ShouldReturnShoppingList()
        {
            //arrange
            var shoppingList = GetMockShoppingListItems();

            _shoppingListMockService.Setup(x => x.GetShoppingListForUser(user1.Id).Result)
               .Returns(shoppingList);

            var shoppingListController = new ShoppingListController(_shoppingListMockService.Object);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            shoppingListController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            ActionResult<List<ShoppingListItem>> actionResult = await shoppingListController.GetShoppingList();

            List<ShoppingListItem> result = (List<ShoppingListItem>)(actionResult.Result as OkObjectResult).Value;

            //assert                      
            Assert.NotNull(result);
            Assert.Equal(shoppingList.Count, result.Count);
            Assert.True(shoppingList.Equals(result));
        }       

        //mock data for shoopingItemsList
        private List<ShoppingListItem> GetMockShoppingListItems()
        {
            List<ShoppingListItem> list = new List<ShoppingListItem>
           {
               new ShoppingListItem
               {
                   Id=shopListId1,
                   Name="Item 1",
                   Quantity=1,
                   UserId=user1.Id,
                   User=user1
               },
               new ShoppingListItem
               {
                   Id=shopListId2,
                   Name="Item 2",
                   Quantity=1,
                   UserId=user1.Id,
                   User=user1
               },
                new ShoppingListItem
               {
                   Id=shopListId3,
                   Name="Item 3",
                   Quantity=3,
                   UserId=user1.Id,
                   User=user1
               }

           };

            return list;
        }       

    }
}
