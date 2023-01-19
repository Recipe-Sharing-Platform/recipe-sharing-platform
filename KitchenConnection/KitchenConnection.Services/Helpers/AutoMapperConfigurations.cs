using AutoMapper;
using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Helpers;

public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<Recipe, RecipeCreateDTO>().ReverseMap();
            CreateMap<Recipe, RecipeDTO>().ReverseMap();

            CreateMap<RecipeIngredient, RecipeIngredientCreateDTO>().ReverseMap();
            CreateMap<RecipeIngredient, RecipeIngredientDTO>().ReverseMap();

            CreateMap<RecipeInstruction, RecipeInstructionCreateDTO>().ReverseMap();
            CreateMap<RecipeInstruction, RecipeInstructionDTO>().ReverseMap();
        
            CreateMap<Tag, TagCreateDTO>().ReverseMap();
            CreateMap<Tag, TagDTO>().ReverseMap();

            CreateMap<RecipeTag, TagCreateDTO>().ReverseMap();
            CreateMap<RecipeTag, TagDTO>().ReverseMap();
        
            CreateMap<Cuisine, CuisineCreateDTO>().ReverseMap();
            CreateMap<Cuisine, CuisineDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
        }
    }

