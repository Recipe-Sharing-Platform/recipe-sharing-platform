using KitchenConnection.DataLayer.Models.DTOs.Review;
using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices
{
    public interface IReviewService
    {
        Task<ReviewDTO> Create(ReviewCreateRequestDTO reviewToCreate, Guid userId);
        Task<List<ReviewDTO>> GetRecipeReviews(Guid recipeId);
        Task<ReviewDTO> Update(ReviewUpdateDTO reviewToUpdate);
        Task<ReviewDTO> Delete(Guid id);
    }
}
