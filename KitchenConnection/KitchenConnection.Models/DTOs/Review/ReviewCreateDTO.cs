using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.Models.DTOs.Review
{
    public class ReviewCreateDTO : ReviewCreateRequestDTO
    {
        public Guid UserId { get; set; }
        public ReviewCreateDTO(ReviewCreateRequestDTO requestDto, Guid userId)
        {
            UserId = userId;
            RecipeId = requestDto.RecipeId;
            Rating = requestDto.Rating;
            Message = requestDto.Message;
        }
    }
    public class ReviewCreateRequestDTO
    {
        public Guid RecipeId { get; set; }
        public double Rating { get; set; }
        public string? Message { get; set; }
    }
}
