using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.ReviewExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Hubs;
using KitchenConnection.Models.DTOs.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace KitchenConnection.Controllers {
    [ApiController]
    [Route("api/reviews")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ReviewController : ControllerBase
    {
        public readonly IReviewService _reviewService;
        private readonly ILogger<ReviewController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        public ReviewController(IReviewService reviewService, IHubContext<NotificationHub> hubContext, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDTO>> Create(ReviewCreateDTO reviewToCreate)
        {
            try
            {
                var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var review = await _reviewService.Create(userId, reviewToCreate);

                return Ok(review);
            }
            catch (RecipeNotFoundException ex)
            {
                _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(Create)}, Exception: {ex}");
                return NotFound(ex.Message);
            }
            catch(ReviewCouldNotBeCreatedException ex)
            {
                _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(Create)}, Exception: {ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReviewDTO>>> GetAll(Guid id)
        {
            try
            {
                var recipeReviews = await _reviewService.GetRecipeReviews(id);

                return Ok(recipeReviews);
            }
            catch (RecipeReviewsNotFoundException ex)
            {
                _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(GetAll)}, Exception: {ex}");
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReviewDTO>> Update(Guid id, ReviewUpdateDTO reviewToUpdate)
        {
            try
            {
                var updatedReview = await _reviewService.Update(reviewToUpdate);

                return Ok(updatedReview);
            }
            catch(ReviewNotFoundException ex)
            {
                _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(Update)}, Exception: {ex}");
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ReviewDTO>> Delete(Guid id)
        {
            try
            {
                var review = await _reviewService.Delete(id);

                return Ok(review);
            }
            catch (ReviewNotFoundException ex)
            {
                _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(Delete)}, Exception: {ex}");
                return NotFound(ex.Message);
            }
        }
    }
}
