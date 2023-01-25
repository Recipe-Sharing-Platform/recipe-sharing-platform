using KitchenConnection.BusinessLogic.Services;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.DTOs.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenConnection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        public readonly IReviewService _reviewService;
        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("GetRecipeReviews")]
        public async Task<IActionResult> GetRecipeReview(Guid id)
        {
            var RecipeReviews = await _reviewService.GetRecipeReviews(id);
            if (RecipeReviews == null)
            {
                return NotFound();
            }
            return Ok(RecipeReviews);
        }

        [HttpPost("CreateReview")]
        public async Task<IActionResult> CreateReview(ReviewCreateDTO reviewCreate)
        {
            await _reviewService.CreateReview(reviewCreate);

            return Ok("Review Created Successfully!");
        }
    }
}
