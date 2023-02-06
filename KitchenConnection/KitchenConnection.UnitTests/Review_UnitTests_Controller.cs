using AutoMapper;
using KitchenConnection.DataLayer.Hubs;
using KitchenConnection.BusinessLogic.Helpers;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Controllers;
using KitchenConnection.Models.DTOs.CookBook;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.DTOs.Review;
using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System.Security.Claims;
using KitchenConnection.DataLayer.Hubs;
using Microsoft.Extensions.Logging;

namespace KitchenConnection.UnitTesting
{
    public class Review_UnitTests_Controller
    {
        private readonly Mock<IReviewService> _reviewMockService;
        private IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;
        public Guid reviewId1 = Guid.NewGuid();
        private Guid reviewId2 = Guid.NewGuid();
        private Guid reviewId3 = Guid.NewGuid();       
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
            Reviews = new List<Review>()
        };

        public Review_UnitTests_Controller()
        {
            var mapperConfiguration = new MapperConfiguration(
             mc => mc.AddProfile(new AutoMapperConfigurations()));

            _mapper = mapperConfiguration.CreateMapper();
            _reviewMockService = new Mock<IReviewService>();
        }

        public async void CreateReview_ShouldReturnReview()
        {
            var reviews = GetMockReviewsDtos();
            var reviewToCreate = _mapper.Map<ReviewCreateDTO>(reviews[0]);

            _reviewMockService.Setup(x => x.Create(user1.Id,reviewToCreate).Result)
               .Returns(reviews[0]);


            ILogger<ReviewController> logger = null;//wont be actually used
            var reviewController = new ReviewController(_reviewMockService.Object,_hubContext,logger);//_hubContext won;t be used in testing so it doesn't matter if it's not properly injected

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            reviewController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            ActionResult<ReviewDTO> actionResult = await reviewController.Create(reviewToCreate);

            ReviewDTO result = (ReviewDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(reviews[0].Id, value);
            Assert.True(reviews[0].Id == value);
        }      

        [Fact]
        public async void GetAllReview_ShouldReturnAllReviews()
        {
            //arrange
            var reviews = GetMockReviewsDtos();
            var returningReviews = new List<ReviewDTO>
            {
                reviews[0]
            };

            _reviewMockService.Setup(x => x.GetRecipeReviews(recipeId1).Result)
               .Returns(returningReviews);

            ILogger<ReviewController> logger = null;//wont be actually used
            var reviewController = new ReviewController(_reviewMockService.Object,_hubContext,logger);

            //act
            ActionResult<List<ReviewDTO>> actionResult = await reviewController.GetAll(recipeId1);

            List<ReviewDTO> result = (List<ReviewDTO>)(actionResult.Result as OkObjectResult).Value;

            //assert           

            Assert.NotNull(result);
            Assert.Equal(returningReviews.Count, result.Count);
            Assert.True(returningReviews.Equals(result));
        }

        [Fact]
        public async void DeleteReviewById_ShouldReturnDeletedReview()
        {
            //arrange
            var reviews = GetMockReviewsDtos();

            _reviewMockService.Setup(x => x.Delete(reviews[0].Id,user1.Id).Result)
                .Returns(reviews[0]);

            ILogger<ReviewController> logger = null;//wont be actually used
            var reviewController = new ReviewController(_reviewMockService.Object,_hubContext,logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            reviewController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<ReviewDTO> actionResult = await reviewController.Delete(reviews[0].Id);

            ReviewDTO result = (ReviewDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(reviews[0].Id, value);
            Assert.True(reviews[0].Id == value);
        }

        [Fact]
        public async void UpdateReview_ShouldReturnUpdatedReview()
        {
            //arrange
            var reviews = GetMockReviewsDtos();
            ReviewUpdateDTO reviewToUpdate = _mapper.Map<ReviewUpdateDTO>(reviews[0]);
            reviewToUpdate.Message= "Updated message";

            _reviewMockService.Setup(x => x.Update(reviewToUpdate, user1.Id).Result)
                .Returns(_mapper.Map<ReviewDTO>(reviewToUpdate));

            ILogger<ReviewController> logger = null;//wont be actually used
            var reviewController = new ReviewController(_reviewMockService.Object,_hubContext,logger);

            //mock httpcontext with fake user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user1.Id.ToString())
            }, "mock"));

            reviewController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //act
            ActionResult<ReviewDTO> actionResult = await reviewController.Update(reviewToUpdate.Id,reviewToUpdate);

            ReviewDTO result = (ReviewDTO)(actionResult.Result as OkObjectResult).Value;

            //assert                      
            Assert.NotNull(result);
            Assert.Equal(reviews[0].Id, result.Id);
            Assert.True(reviews[0].Id == result.Id);
            Assert.NotEqual(reviews[0].Message, result.Message);
            Assert.False(reviews[0].Message == (result.Message));
        }


        //mock data for recipes
        private List<Review> GetMockReviews()
        {
            List<Review> reviews = new List<Review>
           {
               new Review
               {
                   Id=reviewId1,
                   RecipeId=recipeId1,
                   UserId=userId2,
                   User=new User
                   {
                       Id=userId2,
                       FirstName="John"
                   },
                   Message="not good!!!",
                   Rating=1,
                   Recipe=new Recipe
                   {
                       Id=recipeId2,
                       Name="My recipe"
                   }
               },
               new Review
               {
                   Id=reviewId2,
                   RecipeId=recipeId2,
                   UserId=userId3,
                   User=new User
                   {
                       Id=userId3,
                       FirstName="Jonathan"
                   },
                   Message="good!!!",
                   Rating=4,
                   Recipe=new Recipe
                   {
                       Id=recipeId3,
                       Name="My delicious recipe"
                   }
               },
                new Review
               {
                   Id=reviewId3,
                   RecipeId=recipeId3,
                   UserId=userId3,
                   User=new User
                   {
                       Id=userId3,
                       FirstName="Jonathan"
                   },
                   Message="great!!!",
                   Rating=5,
                   Recipe=new Recipe
                   {
                       Id=recipeId2,
                       Name="soup recipe"
                   }
               }
           };

            return reviews;
        }

        //map reviews to reviewDTOs
        public List<ReviewDTO> GetMockReviewsDtos()
        {
            List<ReviewDTO> reviewsDto = new List<ReviewDTO>();
            List<Review> reviews = GetMockReviews();

            foreach (Review rev in reviews)
            {
                ReviewDTO r = _mapper.Map<ReviewDTO>(rev);
                reviewsDto.Add(r);
            }

            return reviewsDto;
        }

    }
}
