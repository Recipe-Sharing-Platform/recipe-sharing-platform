using AutoMapper;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.DTOs.Review;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services
{
    public class ReviewService : IReviewService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        private readonly IRecommendationsService _recommendationsService;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, IRecommendationsService recommendationsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _recommendationsService = recommendationsService;
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
