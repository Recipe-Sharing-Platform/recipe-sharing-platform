using KitchenConnection.BusinessLogic.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers {
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class RecommendationsController:ControllerBase
    {
        private readonly IRecommendationsService _recommendationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RecommendationsController(IRecommendationsService recommendationsService, IHttpContextAccessor httpContextAccessor)
        {
            _recommendationService = recommendationsService;   
            _httpContextAccessor= httpContextAccessor;
        }

        [HttpGet("GetSingleRecommendation")]
        public async Task<IActionResult> GetSingleRecommendation()
        {
            string? userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString != null)
            {
                Guid? userId = new Guid(userIdString);

                if (userId != null && userId != Guid.Empty)
                {
                    var res = await _recommendationService.GetSingleRecommendation(userId);
                    if (res != null)
                    {
                        return Ok(res);
                    }
                    else
                    {
                        return BadRequest("Could not get recommendation from database!");
                    }
                }
                else
                {
                    return BadRequest("Could not get recommendation! User id was null or empty!");
                }
            }
            else
            {
                return BadRequest("Could not get recommendation! User id was not found! Check authentication!");
            }
        }

        [HttpGet("GetCollectionRecommendation")]
        public async Task<IActionResult> GetCollectionRecommendations()
        {
            string? userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                        
            if (userIdString != null)
            {
                Guid? userId = new Guid(userIdString);
                if (userId != null && userId != Guid.Empty)
                {
                    var res = await _recommendationService.GetCollectionRecommendations(userId, 2);
                    if (res != null)
                    {
                        return Ok(res);
                    }
                    else
                    {
                        return BadRequest("Could not get collection recommendations from database!");
                    }
                }
                else
                {
                    return BadRequest("Could not get collection recommendations! User id was null or empty!");
                }
            }
            else
            {
                return BadRequest("Could not get collection recommendations! User id was not found! Check authentication!");
            }
           

           
        }

    }
}
