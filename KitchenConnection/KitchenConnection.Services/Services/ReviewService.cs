using AutoMapper;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Hubs;
using KitchenConnection.Models.DTOs.Review;
using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace KitchenConnection.BusinessLogic.Services {
    public class ReviewService : IReviewService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        private readonly IRecommendationsService _recommendationsService;
        private readonly IRecipeService _recipeService;
        private readonly IHubContext<NotificationHub> _hubContext;



        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, IRecommendationsService recommendationsService, IRecipeService recipeService,IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _recommendationsService = recommendationsService;
            _recipeService = recipeService;
            _hubContext = hubContext;
        }

        public async Task<ReviewDTO> Create(ReviewCreateRequestDTO reviewToRequestCreate, Guid userId)
        {
            ReviewCreateDTO reviewToCreate = new ReviewCreateDTO(reviewToRequestCreate, userId);
            var review = await _unitOfWork.Repository<Review>().Create(_mapper.Map<Review>(reviewToCreate));

            //create recommendation score
            //first get all recipe tags ids            
            Expression<Func<Recipe, bool>> expression = x => x.Id == review.RecipeId;
            var recipe = await _unitOfWork.Repository<Recipe>().GetByConditionWithIncludes(expression, "Tags").FirstOrDefaultAsync();

            //set recommendationScore for each tag
            foreach (var item in recipe.Tags)
            {
                await _recommendationsService.SetScore(review.UserId, item.Id, (int)review.Rating);
            }

            _unitOfWork.Complete();

            var recipeId = review.RecipeId;
            var recipeCreatorId = await _recipeService.GetRecipeCreatorId(recipeId);

            await _hubContext.Clients.Group(recipeCreatorId.ToString()).SendAsync("Receivereview", review);

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<List<ReviewDTO>> GetRecipeReviews(Guid recipeId)
        {
           var reviews = await _unitOfWork.Repository<Review>().GetByConditionWithIncludes(x => x.RecipeId == recipeId, "User, Recipe").ToListAsync();
           
           return _mapper.Map<List<ReviewDTO>>(reviews);
        }

        public async Task<ReviewDTO> Update(ReviewUpdateDTO reviewToUpdate)
        {
            var review = await _unitOfWork.Repository<Review>().GetById(x => x.Id == reviewToUpdate.Id).FirstOrDefaultAsync();

            review.Rating = reviewToUpdate.Rating;
            review.Message = reviewToUpdate.Message;

            _unitOfWork.Repository<Review>().Update(review);
            _unitOfWork.Complete();

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<ReviewDTO> Delete(Guid id)
        {
            var review = await _unitOfWork.Repository<Review>().GetById(x => x.Id == id).FirstOrDefaultAsync();

            if (review == null) return null;

            _unitOfWork.Repository<Review>().Delete(review);
            _unitOfWork.Complete();

            return _mapper.Map<ReviewDTO>(review);
        }
    }
}
