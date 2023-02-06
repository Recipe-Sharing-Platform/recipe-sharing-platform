using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Controllers;
using KitchenConnection.Models.DTOs.CookBook;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace KitchenConnection.UnitTesting
{
    public class Recommendations_UnitTests_Controller
    {
        private readonly Mock<IRecommendationsService> _recommendationsMockService;
        private readonly IMapper _mapper;
        public Guid recommendationId1 = Guid.NewGuid();
        private Guid recommendationId2 = Guid.NewGuid();
        private Guid recommendationId3 = Guid.NewGuid();
        public Guid recipeId1 = Guid.NewGuid();
        private Guid recipeId2 = Guid.NewGuid();
        private Guid recipeId3 = Guid.NewGuid();
        public Guid userId2 = Guid.NewGuid();
        public Guid userId3 = Guid.NewGuid();
        private static Guid tagId1 = Guid.NewGuid();
        private Guid tagId2 = Guid.NewGuid();
        private Guid tagId3 = Guid.NewGuid();
        Tag italianTag = new Tag
        {
            Id = tagId1,
            Name = "Italian",
            Recipes = new List<Recipe>()//???
        };

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

        public Recommendations_UnitTests_Controller()
        {
            var mapperConfiguration = new MapperConfiguration(
             mc => mc.AddProfile(new AutoMapperConfigurations()));

            _mapper = mapperConfiguration.CreateMapper();
            _recommendationsMockService = new Mock<IRecommendationsService>();
        }
       
        [Fact]
        public async void GetSingleRecommendation_ShouldReturnRecommendation()
        {
            //arrange
            var recipes = GetMockRecipes();

            _recommendationsMockService.Setup(x => x.GetSingleRecommendation(user1.Id).Result)
               .Returns(recipes[0]);                               

            ILogger<RecommendationsController> logger = null;//wont be actually used
            var recosController = new RecommendationsController(_recommendationsMockService.Object, logger);
            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            recosController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<Recipe> actionResult = await recosController.GetSingleRecommendation();

            Recipe result = (Recipe)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.IsType<Recipe>(result);            
            Assert.Contains(result,recipes);

        }

        [Fact]
        public async void GetRecomendations_ShouldReturnRecommendationss()
        {
            //arrange
            var recipes = GetMockRecipes();

            _recommendationsMockService.Setup(x => x.GetCollectionRecommendations(user1.Id,2).Result)
               .Returns(recipes);


            ILogger<RecommendationsController> logger = null;//wont be actually used
            var recosController = new RecommendationsController(_recommendationsMockService.Object,logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            recosController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<List<Recipe>> actionResult = await recosController.GetCollectionRecommendations(2);

            List<Recipe> result = (List<Recipe>)(actionResult.Result as OkObjectResult).Value;

            //assert                    
            Assert.NotNull(result);
            Assert.IsType<List<Recipe>>(result);
            foreach(Recipe recipe in result)
            {
                Assert.Contains(recipe,recipes);
            }
        }  
       
        //mock data for recipes
        private List<Recipe> GetMockRecipes()
        {
            List<Recipe> recipes = new List<Recipe>
            {
                new Recipe
                {
                    Id=recipeId1,
                    UserId=userId2,
                    Name="Random1",
                    User=new User
                            {
                                Id=userId2,
                                FirstName="UserName2",
                                LastName="User Surname2",
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
                        italianTag
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
                    UserId=userId2,
                    User=new User
                            {
                                Id=userId2,
                                FirstName="UserName2",
                                LastName="User Surname2",
                                Gender="Male",
                                Email="randomEmail@gjirafa.com",
                                PhoneNumber="44123123",
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
                       italianTag
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
                    UserId=userId3,
                    User=new User
                            {
                                Id=userId3,
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
                            Id=tagId3,
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

            foreach (Recipe rec in recipes)
            {
                RecipeDTO r = _mapper.Map<RecipeDTO>(rec);
                recipesDto.Add(r);
            }

            return recipesDto;
        }

        public List<RecommendationScore> GetMockRecommendationScores()
        {
            List<RecommendationScore> recos = new List<RecommendationScore>
            {
                new RecommendationScore
                {
                    Id= Guid.NewGuid(),
                    UserId=user1.Id,
                    Score=5,
                    TagId=tagId1
                },
                new RecommendationScore
                {
                    Id= Guid.NewGuid(),
                    UserId=user1.Id,
                    Score=2,
                    TagId=tagId2
                }
            };

            return recos;

        }

    }
}
