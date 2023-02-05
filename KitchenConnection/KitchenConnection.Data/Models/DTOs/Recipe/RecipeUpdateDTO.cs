﻿using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitchenConnection.DataLayer.Models.DTOs.Instruction;
using KitchenConnection.DataLayer.Models.DTOs.Ingredient;
using KitchenConnection.DataLayer.Models.DTOs.RecipeTag;

namespace KitchenConnection.DataLayer.Models.DTOs.Recipe
{
    public class RecipeUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RecipeIngredientUpdateDTO> Ingredients { get; set; }
        public List<RecipeInstructionUpdateDTO> Instructions { get; set; }
        public List<RecipeTagUpdateDTO> Tags { get; set; }
        public Guid CuisineId { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int Servings { get; set; }
        public int Yield { get; set; }
        public double Calories { get; set; }
        public string AudioInstructions { get; set; } // Audio Url
        public string VideoInstructions { get; set; } // Video Url
    }
}
