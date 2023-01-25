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
        Task<List<Review>> GetRecipeReviews(Guid recipeId);
        Task CreateReview(ReviewCreateDTO reviewCreate);
    }
}
