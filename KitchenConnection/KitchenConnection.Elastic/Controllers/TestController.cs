using AutoMapper;
using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Elastic.Models;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace KitchenConnection.Elastic.Controllers;

[ApiController]
public class TestController : ControllerBase {

    public readonly MessageSender _messageSender;

    public TestController(MessageSender messageSender) {
        _messageSender = messageSender;
    }

    [HttpPost("SendRecipeToQueue")]
    public IActionResult SendRecipeToQueue([FromBody] RecipeCreateDTO recipeDto) {
        var recipeId = Guid.NewGuid();
        
        List<RecipeIngredient> ingredients = new();
        foreach (var ingredient in recipeDto.Ingredients) {
            ingredients.Add(new RecipeIngredient() {
                Id = Guid.NewGuid(),
                Name = ingredient.Name,
                Amount = ingredient.Amount,
                Unit = ingredient.Unit
            });
        }

        List<RecipeInstruction> instructions = new();
        foreach (var instruction in recipeDto.Instructions) {
            instructions.Add(new RecipeInstruction() {
                Id = Guid.NewGuid(),
                RecipeId = recipeId,
                StepDescription = instruction.StepDescription,
                StepNumber = instruction.StepNumber
            });
        }
        List<RecipeTag> tags = new();
        foreach (var tag in recipeDto.Tags) {
            var tagId = Guid.NewGuid();
            tags.Add(new RecipeTag() {
                Id = Guid.NewGuid(),
                RecipeId = recipeId,
                TagId = tagId,
                Tag = new Tag() {
                    Id = tagId,
                    Name = tag.Name
                }
            });
        }
        
        var recipe = new Recipe() {
            UserId = recipeDto.UserId,
            Id = recipeId,
            Name = recipeDto.Name,
            Description = recipeDto.Description,
            Ingredients = ingredients,
            Instructions = instructions,
            PrepTime = recipeDto.PrepTime,
            CookTime = recipeDto.CookTime,
            Servings = recipeDto.Servings,
            Calories = recipeDto.Calories,
            CuisineId = recipeDto.CuisineId,
            Tags = tags,
            AudioInstructions = recipeDto.AudioInstructions,
            TotalTime = recipeDto.TotalTime,
            VideoInstructions = recipeDto.VideoInstructions,
            Yield = recipeDto.Yield,
        };

        _messageSender.SendMessage(recipe, "index-recipes");

        return Ok();
    }

    [HttpPost("DeleteRecipeFromIndex")]
    public IActionResult DeleteRecipeFromIndex(DeleteRecipe deleteRecipe) {
        _messageSender.SendMessage(deleteRecipe, "delete-recipes");

        return Ok();
    }

}
