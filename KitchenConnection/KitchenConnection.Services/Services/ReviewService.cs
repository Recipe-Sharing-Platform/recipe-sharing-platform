using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.ReviewExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Hubs;
using KitchenConnection.Models.DTOs.Review;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KitchenConnection.BusinessLogic.Services {
    public class ReviewService : IReviewService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        private readonly IRecommendationsService _recommendationsService;
        private readonly IRecipeService _recipeService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly MessageSender _messageSender;


        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, IRecommendationsService recommendationsService, IRecipeService recipeService, IHubContext<NotificationHub> hubContext, MessageSender messageSender) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _recommendationsService = recommendationsService;
            _recipeService = recipeService;
            _hubContext = hubContext;
            _messageSender = messageSender;
        }

        public async Task<ReviewDTO> Create(Guid userId, ReviewCreateDTO reviewToCreate)
        {
            var review = _mapper.Map<Review>(reviewToCreate);
            review.UserId = userId;

            var recipe = await _unitOfWork.Repository<Recipe>().GetById(recipe => recipe.Id == reviewToCreate.RecipeId).Include(recipe => recipe.Tags).FirstOrDefaultAsync();

            if(recipe is null) throw new RecipeNotFoundException(reviewToCreate.RecipeId);
            if (recipe.Tags is null || recipe.Tags.Count == 0) throw new RecipeNotFoundException("Recipe Tags could not be found!");

            review = await _unitOfWork.Repository<Review>().Create(review);

            if (review is null) throw new ReviewCouldNotBeCreatedException("Review could not be created!");

            //create recommendation score
            if (recipe.Tags is null || recipe.Tags.Count == 0) throw new RecipeNotFoundException("Recipe Tags could not be found!");
            //set recommendationScore for each tag
            foreach (var tag in recipe.Tags)
            {
                await _recommendationsService.SetScore(review.UserId, tag.Id, (int)review.Rating);
            }

            _unitOfWork.Complete();

            var recipeCreatorId = await _recipeService.GetRecipeCreatorId(review.RecipeId);

            review.User.Recipes = null;
            review.Recipe.Reviews = null;
            review.Recipe.Tags = null;
            review.Recipe.User = null;
            review.User.Reviews = null;
            _messageSender.SendMessage(new { User = review.User, Recipe = review.Recipe, Review = review }, "created-review-email");

            await _hubContext.Clients.Group(recipeCreatorId.ToString()).SendAsync("Receivereview", review);

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<List<ReviewDTO>> GetRecipeReviews(Guid recipeId)
        {
            var reviews = await _unitOfWork.Repository<Review>().GetByConditionWithIncludes(x => x.RecipeId == recipeId, "User, Recipe").ToListAsync();

            if (reviews is null || reviews.Count == 0) throw new RecipeReviewsNotFoundException(recipeId);

            return _mapper.Map<List<ReviewDTO>>(reviews);
        }

        public async Task<ReviewDTO> Update(ReviewUpdateDTO reviewToUpdate)
        {
            var review = await _unitOfWork.Repository<Review>().GetById(x => x.Id == reviewToUpdate.Id).FirstOrDefaultAsync();

            if (review is null) throw new ReviewNotFoundException(reviewToUpdate.Id);

            review.Rating = reviewToUpdate.Rating;
            review.Message = reviewToUpdate.Message;

            _unitOfWork.Repository<Review>().Update(review);
            _unitOfWork.Complete();

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<ReviewDTO> Delete(Guid reviewId)
        {
            var review = await _unitOfWork.Repository<Review>().GetById(x => x.Id == reviewId).FirstOrDefaultAsync();

            if (review == null) throw new ReviewNotFoundException(reviewId);

            _unitOfWork.Repository<Review>().Delete(review);
            _unitOfWork.Complete();

            return _mapper.Map<ReviewDTO>(review);
        }
    }
}
