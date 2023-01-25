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
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services
{
    public class ReviewService : IReviewService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Review>> GetRecipeReviews(Guid recipeId)
        {
           var review = await _unitOfWork.Repository<Review>().GetByCondition(x => x.RecipeId == recipeId).ToListAsync();
           foreach(var item in review)
            {
                item.Recipe = _unitOfWork.Repository<Recipe>().GetById(x=>x.Id == recipeId).FirstOrDefault();
                item.User = _unitOfWork.Repository<User>().GetById(x => x.Id == item.UserId).FirstOrDefault();
            }
           return review;
        }
        
        public async Task CreateReview(ReviewCreateDTO reviewCreate)
        {
            Review review = new Review();

            review.Message = reviewCreate.Message;
            review.Rating = reviewCreate.Rating;
            review.RecipeId= reviewCreate.RecipeId;
            review.UserId = reviewCreate.UserId;
            review.Recipe = await _unitOfWork.Repository<Recipe>().GetById(x => x.Id == reviewCreate.RecipeId).FirstOrDefaultAsync();
            review.User = await _unitOfWork.Repository<User>().GetById(x => x.Id == reviewCreate.UserId).FirstOrDefaultAsync();

            await _unitOfWork.Repository<Review>().Create(review);
            _unitOfWork.Complete();
        }
    }
}
