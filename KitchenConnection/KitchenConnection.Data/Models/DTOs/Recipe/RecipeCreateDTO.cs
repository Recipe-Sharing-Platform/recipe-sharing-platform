﻿namespace KitchenConnection.DataLayer.Models.DTOs.Recipe
{
    public class RecipeCreateDTO
    {
        public Guid UserId { get; set; }    
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CuisineId { get; set; }
        public List<TagCreateDTO> Tags { get; set; }
        public DateTime PrepTime { get; set; }
        public DateTime CookTime { get; set; }
        public DateTime TotalTime { get; set; }
        public List<RecipeIngredientCreateDTO> Ingredients { get; set; }
        public List<RecipeInstructionCreateDTO> Instructions { get; set; }
        public int Servings { get; set; }
        public int Yield { get; set; }
        public double Calories { get; set; }
        public string AudioInstructions { get; set; } // Audio Url
        public string VideoInstructions { get; set; } // Video Url

        public RecipeCreateDTO(RecipeCreateRequestDTO requestDto, Guid userId)
        {
            UserId = userId;
            Name = requestDto.Name;
            Description = requestDto.Description;
            CuisineId = requestDto.CuisineId;
            Tags = requestDto.Tags;
            PrepTime = requestDto.PrepTime;
            CookTime = requestDto.CookTime;
            TotalTime = requestDto.TotalTime;
            Ingredients = requestDto.Ingredients;
            Instructions = requestDto.Instructions;
            Servings = requestDto.Servings;
            Yield = requestDto.Yield;
            Calories = requestDto.Calories;
            AudioInstructions = requestDto.AudioInstructions;
            VideoInstructions = requestDto.VideoInstructions;
        }

    }
    public class RecipeCreateRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CuisineId { get; set; }
        public List<TagCreateDTO> Tags { get; set; }
        public DateTime PrepTime { get; set; }
        public DateTime CookTime { get; set; }
        public DateTime TotalTime { get; set; }
        public List<RecipeIngredientCreateDTO> Ingredients { get; set; }
        public List<RecipeInstructionCreateDTO> Instructions { get; set; }
        public int Servings { get; set; }
        public int Yield { get; set; }
        public double Calories { get; set; }
        public string AudioInstructions { get; set; } // Audio Url
        public string VideoInstructions { get; set; } // Video Url
    }
}