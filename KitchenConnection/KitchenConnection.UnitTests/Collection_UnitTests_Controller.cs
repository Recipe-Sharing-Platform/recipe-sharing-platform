using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Controllers;
using KitchenConnection.Models.DTOs.Collection;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.DTOs.RecipeTag;
using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Logging;

namespace KitchenConnection.UnitTesting
{
    public class Collection_UnitTests_Controller
    {
        private readonly Mock<ICollectionService> _collectionMockService;
        private readonly IMapper _mapper;
        public Guid collectionId1 = Guid.NewGuid();
        private Guid collectionId2 = Guid.NewGuid();
        private Guid collectionId3 = Guid.NewGuid();
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

        public Collection_UnitTests_Controller()
        {
            var mapperConfiguration = new MapperConfiguration(
             mc => mc.AddProfile(new AutoMapperConfigurations()));

            _mapper = mapperConfiguration.CreateMapper();
            _collectionMockService = new Mock<ICollectionService>();
        }

        [Fact]
        public async void CreateCollection_ShouldReturnCollection()
        {
            var colls = GetMockCollectionDtos();
            var collToCreate = _mapper.Map<CollectionCreateRequestDTO>(colls[0]);

            _collectionMockService.Setup(x => x.Create(user1.Id,collToCreate).Result)
               .Returns(colls[0]);
            ILogger<RecipeController> logger = null;//wont be actually used
            var collectionController = new CollectionController(_collectionMockService.Object,logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            collectionController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            ActionResult<CollectionDTO> actionResult = await collectionController.Create(collToCreate);

            CollectionDTO result = (CollectionDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(colls[0].Id, value);
            Assert.True(colls[0].Id == value);
        }
        [Fact]
        public async void GetCollectionById_ShouldReturnCollection()
        {
            //arrange
            var colls = GetMockCollectionDtos();

            _collectionMockService.Setup(x => x.Get(user1.Id,colls[0].Id).Result)
               .Returns(colls[0]);
            ILogger<RecipeController> logger = null;//wont be actually used
            var collectionController = new CollectionController(_collectionMockService.Object,logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            collectionController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<CollectionDTO> actionResult = await collectionController.Get(collectionId1);

            CollectionDTO result = (CollectionDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(colls[0].Id, value);
            Assert.True(colls[0].Id == value);

        }

        [Fact]
        public async void GetAllCollections_ShouldReturnCollections()
        {
            //arrange
            var colls = GetMockCollectionDtos();

            _collectionMockService.Setup(x => x.GetAll(user1.Id).Result)
               .Returns(colls);
            ILogger<RecipeController> logger = null;//wont be actually used
            var collectionController = new CollectionController(_collectionMockService.Object,logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            collectionController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<List<CollectionDTO>> actionResult = await collectionController.GetAll();

            List<CollectionDTO> result = (List<CollectionDTO>)(actionResult.Result as OkObjectResult).Value;

            //assert           
            Assert.NotNull(result);
            Assert.Equal(result.Count, result.Count);
            Assert.True(colls.Equals(result));
        }

        [Fact]
        public async void DeleteCollectionById_ShouldReturnDeletedCollection()
        {
            //arrange
            var colls = GetMockCollectionDtos();

            _collectionMockService.Setup(x => x.Delete(user1.Id, colls[0].Id).Result)
                .Returns(colls[0]);
            ILogger<RecipeController> logger = null;//wont be actually used
            var collectionController = new CollectionController(_collectionMockService.Object,logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            collectionController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<CollectionDTO> actionResult = await collectionController.Delete(collectionId1);

            CollectionDTO result = (CollectionDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(colls[0].Id, value);
            Assert.True(colls[0].Id == value);
        }

        [Fact]
        public async void UpdateCollection_ShouldReturnUpdatedCollection()
        {
            //arrange
            var colls = GetMockCollectionDtos();
            var collectionToUpdate = _mapper.Map<CollectionUpdateDTO>(colls[0]);
            collectionToUpdate.Name = "Updated Tag Name";

            _collectionMockService.Setup(x => x.Update(user1.Id,collectionToUpdate).Result)
                .Returns(_mapper.Map<CollectionDTO>(collectionToUpdate));

            ILogger<RecipeController> logger = null;//wont be actually used
            var collectionController = new CollectionController(_collectionMockService.Object,logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            collectionController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<CollectionDTO> actionResult = await collectionController.UpdateCollection(collectionToUpdate);

            CollectionDTO result = (CollectionDTO)(actionResult.Result as OkObjectResult).Value;

            //assert                      
            Assert.NotNull(result);
            Assert.Equal(colls[0].Id, result.Id);
            Assert.True(colls[0].Id == result.Id);
            Assert.NotEqual(colls[0].Name, result.Name);
            Assert.False(colls[0].Name == (result.Name));
        }

        //mock data for collections
        private List<Collection> GetMockCollections()
        {
            List<Collection> collections = new List<Collection>
            {
                new Collection
                {
                    Id=collectionId1,
                    Name="My collection 1",
                    Recipes=new List<CollectionRecipe>(),
                    Description="Desctription 1",
                    NumberOfRecipes=1,
                    User=user1,
                    UserId=user1.Id                    
                },
                new Collection
                {
                    Id=collectionId2,
                    Name="My collection 2",
                    Recipes=new List<CollectionRecipe>(),
                    Description="Desctription 2",
                    NumberOfRecipes=1,
                    User=user1,
                    UserId=user1.Id
                },
                new Collection
                {
                    Id=collectionId3,
                    Name="My collection 3",
                    Recipes=new List<CollectionRecipe>(),
                    Description="Desctription 3",
                    NumberOfRecipes=1,
                    User=user1,
                    UserId=user1.Id
                },
            };

            return collections;
        }

        //map tags to tagsDTOs
        public List<CollectionDTO> GetMockCollectionDtos()
        {
            List<CollectionDTO> collDtos = new List<CollectionDTO>();
            List<Collection> colls = GetMockCollections();

            foreach (Collection col in colls)
            {
                CollectionDTO c = _mapper.Map<CollectionDTO>(col);
                collDtos.Add(c);
            }

            return collDtos;
        }



    }
}
