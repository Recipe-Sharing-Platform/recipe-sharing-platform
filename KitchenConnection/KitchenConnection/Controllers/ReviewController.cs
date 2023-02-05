﻿using KitchenConnection.BusinessLogic.Services.IServices;
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
        private readonly IHubContext<NotificationHub> _hubContext;
        public ReviewController(IReviewService reviewService, IHubContext<NotificationHub> hubContext)
        {
            _reviewService = reviewService;
            _hubContext = hubContext;

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
        public async Task<ActionResult<ReviewDTO>> Create(ReviewCreateRequestDTO reviewToCreate)
        {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var review = await _reviewService.Create(reviewToCreate, UserId);

            if (review== null)
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
