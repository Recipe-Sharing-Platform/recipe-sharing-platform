using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Controllers;
using KitchenConnection.DataLayer.Models.DTOs.CookBook;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;


namespace KitchenConnection.UnitTesting
{
    public class CookBook_UnitTests_Controller
    {
        private readonly Mock<ICookBookService> _cookbookMockService;
        private readonly IMapper _mapper;
        public Guid cookbookId1 = Guid.NewGuid();
        private Guid cookbookId2 = Guid.NewGuid();
        private Guid cookbookId3 = Guid.NewGuid();
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
            Reviews = new List<Review>()
        };

        public CookBook_UnitTests_Controller()
        {
            var mapperConfiguration = new MapperConfiguration(
             mc => mc.AddProfile(new AutoMapperConfigurations()));

            _mapper = mapperConfiguration.CreateMapper();
            _cookbookMockService = new Mock<ICookBookService>();
        }

        [Fact]
        public async void CreateCookBook_ShouldReturnCookBook()
        {
            var cookbooks = GetMockCookbookDtos();
            var cookbookToCreate = _mapper.Map<CookBookCreateRequestDTO>(cookbooks[0]);

            _cookbookMockService.Setup(x => x.Create(cookbookToCreate, user1.Id).Result)
               .Returns(cookbooks[0]);
            var cookbookController = new CookBookController(_cookbookMockService.Object);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            cookbookController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            ActionResult<CookBookDTO> actionResult = await cookbookController.Create(cookbookToCreate);

            CookBookDTO result = (CookBookDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(cookbooks[0].Id, value);
            Assert.True(cookbooks[0].Id == value);
        }
        [Fact]
        public async void GetCookBookById_ShouldReturnCookBook()
        {
            //arrange
            var cookbooks = GetMockCookbookDtos();

            _cookbookMockService.Setup(x => x.Get(cookbooks[0].Id).Result)
               .Returns(cookbooks[0]);
            var cookbookController = new CookBookController(_cookbookMockService.Object);

            //act
            ActionResult<CookBookDTO> actionResult = await cookbookController.Get(cookbookId1);

            CookBookDTO result = (CookBookDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(cookbooks[0].Id, value);
            Assert.True(cookbooks[0].Id == value);

        }

        [Fact]
        public async void GetAllCookbooks_ShouldReturnCookbooks()
        {
            //arrange
            var cookbooks = GetMockCookbookDtos();

            _cookbookMockService.Setup(x => x.GetAll().Result)
               .Returns(cookbooks);
            var cookbookController = new CookBookController(_cookbookMockService.Object);

            //act
            ActionResult<List<CookBookDTO>> actionResult = await cookbookController.GetAll();

            List<CookBookDTO> result = (List<CookBookDTO>)(actionResult.Result as OkObjectResult).Value;

            //assert           
            Assert.NotNull(result);
            Assert.Equal(result.Count, result.Count);
            Assert.True(cookbooks.Equals(result));
        }

        [Fact]
        public async void DeleteCookbookById_ShouldReturnDeletedCookbook()
        {
            //arrange
            var cookbooks = GetMockCookbookDtos();

            _cookbookMockService.Setup(x => x.Delete(cookbooks[0].Id).Result)
                .Returns(cookbooks[0]);
            var cookbookController = new CookBookController(_cookbookMockService.Object);

            //act
            ActionResult<CookBookDTO> actionResult = await cookbookController.Delete(cookbookId1);

            CookBookDTO result = (CookBookDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(cookbooks[0].Id, value);
            Assert.True(cookbooks[0].Id == value);
        }

        [Fact]
        public async void UpdateCookbook_ShouldReturnUpdatedCookbook()
        {
            //arrange
            var cookbooks = GetMockCookbookDtos();
            var cookbookToUpdate = _mapper.Map<CookBookUpdateDTO>(cookbooks[0]);
            cookbookToUpdate.Name = "Updated Cookbook Name";
            _cookbookMockService.Setup(x => x.Update(cookbookToUpdate).Result)
                .Returns(_mapper.Map<CookBookDTO>(cookbookToUpdate));
            var cookbookController = new CookBookController(_cookbookMockService.Object);

            //act
            ActionResult<CookBookDTO> actionResult = await cookbookController.UpdateCookBook(cookbookToUpdate);

            CookBookDTO result = (CookBookDTO)(actionResult.Result as OkObjectResult).Value;

            //assert                      
            Assert.NotNull(result);
            Assert.Equal(cookbooks[0].Id, result.Id);
            Assert.True(cookbooks[0].Id == result.Id);
            Assert.NotEqual(cookbooks[0].Name, result.Name);
            Assert.False(cookbooks[0].Name == (result.Name));
        }

        //mock data for cookbooks
        private List<CookBook> GetMockCookBooks()
        {
            List<CookBook> cookbooks = new List<CookBook>
            {
                new CookBook
                {
                    Id=cookbookId1,
                    Name="My cookbook 1",
                    Recipes=new List<Recipe>(),
                    Description="Description 1",                    
                    User=user1,
                    UserId=user1.Id,      
                },
                new CookBook
                {
                    Id=cookbookId2,
                    Name="My cookbook 2",
                    Recipes=new List<Recipe>(),
                    Description="Description 2",
                    User=user1,
                    UserId=user1.Id
                },
                new CookBook
                {
                    Id=cookbookId3,
                    Name="My cookbook 3",
                    Recipes=new List<Recipe>(),
                    Description="Description 3",
                    User=user1,
                    UserId=user1.Id
                },
            };

            return cookbooks;
        }

        //map cookbooks to cookbooksDTOs
        public List<CookBookDTO> GetMockCookbookDtos()
        {
            List<CookBookDTO> cookbooksDtos = new List<CookBookDTO>();
            List<CookBook> cookbooks = GetMockCookBooks();

            foreach (CookBook cookbook in cookbooks)
            {
                CookBookDTO c = _mapper.Map<CookBookDTO>(cookbook);
                cookbooksDtos.Add(c);
            }

            return cookbooksDtos;
        }

    }
}
