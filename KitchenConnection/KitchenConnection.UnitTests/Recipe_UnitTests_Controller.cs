using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Controllers;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace KitchenConnection.UnitTesting
{
    public class Recipe_UnitTests_Controller
    {
        private readonly Mock<IRecipeService> _recipeMockService;
        private readonly IMapper _mapper;
        public Guid recipeId1=Guid.NewGuid();
        private Guid recipeId2 = Guid.NewGuid();
        private Guid recipeId3 = Guid.NewGuid();
        private User user1=new User
                            {
                                Id=Guid.NewGuid(),
                                FirstName="UserName1",
                                LastName="User Surname1",
                                Gender="Male",
                                Email="randomEmail@gjirafa.com",
                                PhoneNumber="44123123",
                                DateOfBirth=new DateTime(),
                                Recipes=new List<Recipe>(),
                                Reviews=new List<Review>()
                            };

        public Recipe_UnitTests_Controller()
        {
            var mapperConfiguration = new MapperConfiguration(
             mc => mc.AddProfile(new AutoMapperConfigurations()));

            _mapper = mapperConfiguration.CreateMapper();         
            _recipeMockService = new Mock<IRecipeService>();
        }

        [Fact]

        public async void CreateRecipe_ShouldReturnRecipe()
        {
            var recipes = GetMockRecipesDtos();
            var recipeToCreate = _mapper.Map<RecipeCreateDTO>(recipes[0]);

            _recipeMockService.Setup(x => x.Create(user1.Id,recipeToCreate).Result)
               .Returns(recipes[0]);

            ILogger<RecipeController> logger=null;//wont be actually used
            var recipeController = new RecipeController(_recipeMockService.Object,logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {                
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())               
            }, "mock"));
           
            recipeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };         

            ActionResult<RecipeDTO> actionResult = await recipeController.Create(recipeToCreate);

            RecipeDTO result = (RecipeDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(recipes[0].Id, value);
            Assert.True(recipes[0].Id == value);
        }
        [Fact]
        public async void GetRecipeById_ShouldReturnRecipe()
        {
            //arrange
            var recipes = GetMockRecipesDtos();

            _recipeMockService.Setup(x => x.Get(recipes[0].Id).Result)
               .Returns(recipes[0]);
            ILogger<RecipeController> logger = null;//wont be actually used
            var recipeController = new RecipeController(_recipeMockService.Object, logger);


            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            recipeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<RecipeDTO> actionResult = await recipeController.Get(recipeId1);

            RecipeDTO result = (RecipeDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(recipes[0].Id, value);
            Assert.True(recipes[0].Id == value);

        }

        [Fact]
        public async void GetAllRecipes_ShouldReturnRecipes()
        {
            //arrange
            var recipes = GetMockRecipesDtos();

            _recipeMockService.Setup(x => x.GetAll().Result)
               .Returns(recipes);
            ILogger<RecipeController> logger = null;//wont be actually used
            var recipeController = new RecipeController(_recipeMockService.Object, logger);


            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            recipeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<List<RecipeDTO>> actionResult = await recipeController.GetAll();

            List<RecipeDTO> result = (List<RecipeDTO>)(actionResult.Result as OkObjectResult).Value;

            //assert           
            
            Assert.NotNull(result);
            Assert.Equal(recipes.Count,result.Count);
            Assert.True(recipes.Equals(result));

        }

        [Fact]
        public async void DeleteRecipeById_ShouldReturnDeletedRecipe()
        {            
            //arrange
            var recipes = GetMockRecipesDtos();

            _recipeMockService.Setup(x => x.Delete(recipes[0].Id,user1.Id).Result)
                .Returns(recipes[0]);
            ILogger<RecipeController> logger = null;//wont be actually used
            var recipeController = new RecipeController(_recipeMockService.Object, logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            recipeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<RecipeDTO> actionResult = await recipeController.Delete(recipeId1);

            RecipeDTO result= (RecipeDTO)(actionResult.Result as OkObjectResult).Value;            

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(recipes[0].Id, value);
            Assert.True(recipes[0].Id == value);
        }

        [Fact]
        public async void UpdateRecipe_ShouldReturnUpdatedRecipe()
        {
            //arrange
            var recipes = GetMockRecipesDtos();
            RecipeUpdateDTO recipeToUpdate = _mapper.Map<RecipeUpdateDTO>(recipes[0]);
            recipeToUpdate.Name = "Updated Name";

            _recipeMockService.Setup(x => x.Update(recipeToUpdate,user1.Id).Result)
                .Returns(_mapper.Map<RecipeDTO>(recipeToUpdate));
            ILogger<RecipeController> logger = null;//wont be actually used
            var recipeController = new RecipeController(_recipeMockService.Object, logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            recipeController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<RecipeDTO> actionResult = await recipeController.Update(recipeToUpdate);

            RecipeDTO result = (RecipeDTO)(actionResult.Result as OkObjectResult).Value;

            //assert                      
            Assert.NotNull(result);
            Assert.Equal(recipes[0].Id, result.Id);
            Assert.True(recipes[0].Id == result.Id);
            Assert.NotEqual(recipes[0].Name, result.Name);
            Assert.False(recipes[0].Name==(result.Name));
        }


        //mock data for recipes
        private List<Recipe> GetMockRecipes()
        {
            List<Recipe> recipes = new List<Recipe>
            {   
                new Recipe
                {
                    Id=recipeId1,
                    UserId=Guid.NewGuid(),
                    Name="Random1",
                    User=new User
                            {
                                Id=user1.Id,
                                FirstName="UserName1",
                                LastName="User Surname1",
                                Gender="Male",
                                Email="randomEmail@gjirafa.com",
                                PhoneNumber="44123123",
                                DateOfBirth=new DateTime(),
                                Recipes=new List<Recipe>(),
                                Reviews=new List<Review>()
                            },
                    Description="Desc1",
                    Ingredients=new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        { 
                            RecipeId=Guid.NewGuid(),
                            Recipe=new Recipe(),//???
                            Amount = 100,
                            Name="Milk",
                            Unit=Models.Enums.Unit.Mililiter,
                        }
                    },               
                    Instructions=new List<RecipeInstruction>
                    {
                        new RecipeInstruction
                        {
                            RecipeId=Guid.NewGuid(),
                            Recipe=new Recipe(),
                            StepNumber=1,
                            StepDescription="random step description 1"
                        }
                    },
                    Tags=new List<Tag>
                    {
                        new Tag
                        {
                            Id=Guid.NewGuid(),
                            Name="Italian",
                            Recipes=new List<Recipe>()//???
                        }
                    },
                    CuisineId=Guid.NewGuid(),
                    Cuisine=new Cuisine(),
                    PrepTime=2,
                    CookTime=2,
                    Servings=2,
                    Yield=10,
                    Calories=500,
                    AudioInstructions="randomAudioUrl",
                    VideoInstructions="randomVideoUrl",
                    Reviews=new List<Review>
                    {
                        new Review
                        {
                            Id=Guid.NewGuid(),
                            UserId=Guid.NewGuid(),
                            User=new User
                            {
                                Id=new Guid("4fd7e75b-7100-47de-9b5e-9ab9939d0801"),
                                FirstName="UserName1",
                                LastName="User Surname1",
                                Gender="Male",
                                Email="randomEmail@gjirafa.com",
                                PhoneNumber="44123123",
                                DateOfBirth=new DateTime(),
                                Recipes=new List<Recipe>(),
                                Reviews=new List<Review>()
                            }
                        }
                    },
                    CookBook=new CookBook(),
                },

                new Recipe
                {
                    Id=recipeId2,
                    UserId=Guid.NewGuid(),
                    User=new User
                            {
                                Id=Guid.NewGuid(),
                                FirstName="User1",
                                LastName="User Surname2",
                                Gender="Male",
                                Email="randomEmail2@gjirafa.com",
                                PhoneNumber="45123123",
                                DateOfBirth=new DateTime(),
                                Recipes=new List<Recipe>(),
                                Reviews=new List<Review>()
                            },
                    Name="Random2",
                    Description="Desc2",
                    Ingredients=new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            RecipeId=Guid.NewGuid(),
                            Recipe=new Recipe(),//???
                            Amount = 200,
                            Name="Coffee",
                            Unit=Models.Enums.Unit.Mililiter,
                        }
                    },
                    Instructions=new List<RecipeInstruction>
                    {
                        new RecipeInstruction
                        {
                            RecipeId=Guid.NewGuid(),
                            Recipe=new Recipe(),
                            StepNumber=1,
                            StepDescription="random step description 1"
                        }
                    },
                    Tags=new List<Tag>
                    {
                        new Tag
                        {
                            Id=Guid.NewGuid(),
                            Name="Albanian",
                            Recipes=new List<Recipe>()//???
                        }
                    },
                    CuisineId=Guid.NewGuid(),
                    Cuisine=new Cuisine(),
                    PrepTime=2,
                    CookTime=2,                    
                    Servings=2,
                    Yield=10,
                    Calories=500,
                    AudioInstructions="randomAudioUrl2",
                    VideoInstructions="randomVideoUrl2",
                    Reviews=new List<Review>
                    {
                        new Review
                        {
                            Id=Guid.NewGuid(),
                            UserId=Guid.NewGuid(),
                            User=new User
                            {
                                Id=Guid.NewGuid(),
                                FirstName="User Name2",
                                LastName="User Surname2",
                                Gender="Male",
                                Email="randomEmail2@gjirafa.com",
                                PhoneNumber="45123123",
                                DateOfBirth=new DateTime(),
                                Recipes=new List<Recipe>(),
                                Reviews=new List<Review>()
                            }
                        }
                    },
                    CookBook=new CookBook(),
                },

                new Recipe
                {
                    Id=recipeId3,
                    UserId=Guid.NewGuid(),
                    User=new User
                            {
                                Id=Guid.NewGuid(),
                                FirstName="User Name3",
                                LastName="User Surname3",
                                Gender="Male",
                                Email="randomEmail3@gjirafa.com",
                                PhoneNumber="46123123",
                                DateOfBirth=new DateTime(),
                                Recipes=new List<Recipe>(),
                                Reviews=new List<Review>()
                            },
                    Name="Random3",
                    Description="Desc3",
                    Ingredients=new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            RecipeId=Guid.NewGuid(),
                            Recipe=new Recipe(),//???
                            Amount = 300,
                            Name="Potatoe",
                            Unit=Models.Enums.Unit.Mililiter,
                        }
                    },
                    Instructions=new List<RecipeInstruction>
                    {
                        new RecipeInstruction
                        {
                            RecipeId=Guid.NewGuid(),
                            Recipe=new Recipe(),
                            StepNumber=1,
                            StepDescription="random step description 3"
                        }
                    },
                    Tags=new List<Tag>
                    {
                        new Tag
                        {
                            Id=Guid.NewGuid(),
                            Name="Sweets",
                            Recipes=new List<Recipe>()//???
                        }
                    },
                    CuisineId=Guid.NewGuid(),
                    Cuisine=new Cuisine(),
                    PrepTime=2,
                    CookTime=2,                    
                    Servings=2,
                    Yield=10,
                    Calories=500,
                    AudioInstructions="randomAudioUrl3",
                    VideoInstructions="randomVideoUrl3",
                    Reviews=new List<Review>
                    {
                        new Review
                        {
                            Id=Guid.NewGuid(),
                            UserId=Guid.NewGuid(),
                            User=new User
                            {
                                Id=Guid.NewGuid(),
                                FirstName="User Name3",
                                LastName="User Surname3",
                                Gender="Male",
                                Email="randomEmail3@gjirafa.com",
                                PhoneNumber="46123123",
                                DateOfBirth=new DateTime(),
                                Recipes=new List<Recipe>(),
                                Reviews=new List<Review>()
                            }
                        }
                    },
                    CookBook=new CookBook(),
                },

            };

            return recipes;
        }

        //map recipes to recipeDTOs
        public List<RecipeDTO> GetMockRecipesDtos()
        {
            List<RecipeDTO> recipesDto = new List<RecipeDTO>();
            List<Recipe> recipes = GetMockRecipes();

            foreach(Recipe rec in recipes)
            {
                RecipeDTO r = _mapper.Map<RecipeDTO>(rec);
                recipesDto.Add(r);
            }

            return recipesDto;
        }

    }
}
