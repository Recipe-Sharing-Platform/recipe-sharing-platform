﻿using KitchenConnection.Models.DTOs.Review;

namespace KitchenConnection.BusinessLogic.Services.IServices {
    public interface IReviewService
    {
        Task<ReviewDTO> Create(Guid userId, ReviewCreateDTO reviewToCreate);
        Task<List<ReviewDTO>> GetRecipeReviews(Guid recipeId);
        Task<ReviewDTO> Update(ReviewUpdateDTO reviewToUpdate, Guid userId);
        Task<ReviewDTO> Delete(Guid reviewId, Guid userId);
    }
}
