using KitchenConnection.BusinessLogic.Services;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.DTOs.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ReviewController : ControllerBase
    {
        public readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReviewDTO>>> GetAll(Guid id)
        {
            var recipeReviews = await _reviewService.GetRecipeReviews(id);
            if (recipeReviews == null)
            {
                return NotFound();
            }
            return Ok(recipeReviews);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDTO>> Create(ReviewCreateDTO reviewToCreate)
        {
            reviewToCreate.UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var review = await _reviewService.Create(reviewToCreate);

            if(review== null)
            {
                return BadRequest("Could not add the review!");
            }

            return Ok(review);
        }

        [HttpDelete]
        public async Task<ActionResult<ReviewDTO>> Delete(Guid id)
        {
            var review = await _reviewService.Delete(id);

            if(review == null)
            {
                return BadRequest("Could not delete the review!");
            }

            return Ok(review);
        }
    }
}
