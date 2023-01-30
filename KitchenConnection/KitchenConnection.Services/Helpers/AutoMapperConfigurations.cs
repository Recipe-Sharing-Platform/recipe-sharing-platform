using AutoMapper;
using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs;
using KitchenConnection.DataLayer.Models.DTOs.Collection;
using KitchenConnection.DataLayer.Models.DTOs.CookBook;
using KitchenConnection.DataLayer.Models.DTOs.Ingredient;
using KitchenConnection.DataLayer.Models.DTOs.Instruction;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs.RecipeTag;
using KitchenConnection.DataLayer.Models.DTOs.Review;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Helpers;

public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            //CreateMap<Recipe, RecipeCreateDTO>().ReverseMap();
        
            CreateMap<RecipeCreateDTO, Recipe>().ForMember(src => src.Tags, dest => dest.Ignore());
            CreateMap<Recipe, RecipeDTO>().ReverseMap();
            CreateMap<Recipe, RecipeUpdateDTO>().ReverseMap();

            CreateMap<RecipeIngredient, RecipeIngredientCreateDTO>().ReverseMap();
            CreateMap<RecipeIngredient, RecipeIngredientDTO>().ReverseMap();
            CreateMap<RecipeIngredient, RecipeIngredientUpdateDTO>().ReverseMap();

            CreateMap<RecipeInstruction, RecipeInstructionCreateDTO>().ReverseMap();
            CreateMap<RecipeInstruction, RecipeInstructionDTO>().ReverseMap();
            CreateMap<RecipeInstruction, RecipeInstructionUpdateDTO>().ReverseMap();

            CreateMap<Tag, TagCreateDTO>().ReverseMap();
            CreateMap<Tag, TagDTO>().ReverseMap();
            CreateMap<Tag, RecipeTagUpdateDTO>().ReverseMap();
        
            CreateMap<Cuisine, CuisineCreateDTO>().ReverseMap();
            CreateMap<Cuisine, CuisineDTO>().ReverseMap();

            CreateMap<CookBookCreateDTO, CookBook>().ForMember(src => src.Recipes, dest => dest.Ignore()).ReverseMap();
            CreateMap<CookBook, CookBookDTO>().ReverseMap();

        CreateMap<CollectionCreateDTO, Collection>().ForMember(src => src.Recipes, dest => dest.Ignore()).ReverseMap();
            CreateMap<Collection, CollectionDTO>().ForMember(src => src.Recipes, dest => dest.MapFrom(c => c.Recipes.Select(rc => rc.Recipe)))
    .ReverseMap();

        CreateMap<User, UserCreateDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => String.Format("{0} {1}", src.FirstName, src.LastName))).ReverseMap();

            CreateMap<Review, ReviewCreateDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
        }
    }

/*public class UserCustomResolver : IValueResolver<User, UserDTO, string>
{
    public string Resolve(User source, UserDTO destionation, string member, ResolutionContext context)
    {
        return $"{source.FirstName} {source.LastName}";
    }
}*/