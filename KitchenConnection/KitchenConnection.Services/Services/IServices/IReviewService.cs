using KitchenConnection.Models.DTOs.Review;

namespace KitchenConnection.BusinessLogic.Services.IServices {
    public interface IReviewService
    {
        Task<ReviewDTO> Create(ReviewCreateRequestDTO reviewToCreate, Guid userId);
        Task<List<ReviewDTO>> GetRecipeReviews(Guid recipeId);
        Task<ReviewDTO> Update(ReviewUpdateDTO reviewToUpdate);
        Task<ReviewDTO> Delete(Guid id);
    }
}
