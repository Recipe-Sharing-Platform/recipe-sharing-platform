using AutoMapper;
using KitchenConnection.Models.DTOs;
using KitchenConnection.Models.DTOs.Collection;
using KitchenConnection.Models.DTOs.CookBook;
using KitchenConnection.Models.DTOs.Cuisine;
using KitchenConnection.Models.DTOs.Ingredient;
using KitchenConnection.Models.DTOs.Instruction;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.DTOs.RecipeTag;
using KitchenConnection.Models.DTOs.Review;
using KitchenConnection.Models.DTOs.ShoppingCart;
using KitchenConnection.Models.DTOs.Tag;
using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities.Mappings;

namespace KitchenConnection.BusinessLogic.Helpers;

public class AutoMapperConfigurations : Profile
{
    public AutoMapperConfigurations()
    {        
        CreateMap<RecipeCreateDTO, Recipe>().ForMember(src => src.Tags, dest => dest.Ignore());
        CreateMap<Recipe, RecipeDTO>().ForMember(src => src.TotalTime, dest => dest.MapFrom(x => x.PrepTime + x.CookTime));
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
        CreateMap<Collection, CollectionDTO>().ForMember(src => src.Recipes, dest => dest.MapFrom(c => c.Recipes.Select(rc => rc.Recipe))).ReverseMap();

        CreateMap<User, UserCreateDTO>().ReverseMap();

        CreateMap<User, UserDTO>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => String.Format("{0} {1}", src.FirstName, src.LastName))).ReverseMap();

        CreateMap<Review, ReviewCreateDTO>().ReverseMap();
        CreateMap<Review, ReviewDTO>().ReverseMap();

        CreateMap<RecommendationScore, RecommendationScoreCreateDto>().ReverseMap();
        CreateMap<ShoppingListItemDTO, ShoppingListItem>().ReverseMap();
         CreateMap<ShoppingListItemCreateDTO, ShoppingListItem>().ReverseMap();

        //needed for unit testing
        CreateMap<RecipeDTO, RecipeUpdateDTO>().ReverseMap();
        CreateMap<RecipeIngredientDTO, RecipeIngredientUpdateDTO>().ReverseMap();
        CreateMap<RecipeInstructionDTO, RecipeInstructionUpdateDTO>().ReverseMap();
        CreateMap<TagDTO, RecipeTagUpdateDTO>().ReverseMap();
        //CreateMap<RecipeDTO, RecipeCreateRequestDTO>().ReverseMap();
        CreateMap<TagDTO, TagCreateDTO>().ReverseMap();
        CreateMap<RecipeIngredientDTO, RecipeIngredientCreateDTO>().ReverseMap();
        CreateMap<RecipeInstructionDTO, RecipeInstructionCreateDTO>().ReverseMap();
        CreateMap<CollectionDTO, CollectionCreateRequestDTO>().ReverseMap();
        CreateMap<CookBookDTO, CollectionUpdateDTO>().ReverseMap();
        CreateMap<CookBookDTO, CookBookCreateRequestDTO>().ReverseMap();
        CreateMap<CookBookDTO, CookBookUpdateDTO>().ReverseMap();
        CreateMap<CollectionDTO, CollectionUpdateDTO>().ReverseMap();

        CreateMap<User, UserDTO>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => String.Format("{0} {1}", src.FirstName, src.LastName))).ReverseMap();


        CreateMap<Review, ReviewCreateDTO>().ReverseMap();
        CreateMap<Review, ReviewDTO>().ReverseMap();

        CreateMap<RecommendationScore, RecommendationScoreCreateDto>().ReverseMap();
        CreateMap<ShoppingListItemDTO, ShoppingListItem>().ReverseMap();
        CreateMap<ShoppingListItemCreateDTO, ShoppingListItem>().ReverseMap();

        //needed for unit testing
        CreateMap<RecipeDTO, RecipeUpdateDTO>().ReverseMap();
        CreateMap<RecipeIngredientDTO, RecipeIngredientUpdateDTO>().ReverseMap();
        CreateMap<RecipeInstructionDTO, RecipeInstructionUpdateDTO>().ReverseMap();
        CreateMap<TagDTO, RecipeTagUpdateDTO>().ReverseMap();
        CreateMap<RecipeDTO, RecipeCreateDTO>().ReverseMap();
        CreateMap<TagDTO, TagCreateDTO>().ReverseMap();
        CreateMap<RecipeIngredientDTO, RecipeIngredientCreateDTO>().ReverseMap();
        CreateMap<RecipeInstructionDTO, RecipeInstructionCreateDTO>().ReverseMap();
        CreateMap<CollectionDTO, CollectionCreateRequestDTO>().ReverseMap();
        CreateMap<CookBookDTO, CollectionUpdateDTO>().ReverseMap();
        CreateMap<CookBookDTO, CookBookCreateRequestDTO>().ReverseMap();
        CreateMap<CookBookDTO, CookBookUpdateDTO>().ReverseMap();
        CreateMap<CollectionDTO, CollectionUpdateDTO>().ReverseMap();
        CreateMap<ReviewDTO,ReviewUpdateDTO>().ReverseMap();
    }
}

/*public class UserCustomResolver : IValueResolver<User, UserDTO, string>
{
    public string Resolve(User source, UserDTO destionation, string member, ResolutionContext context)
    {
        return $"{source.FirstName} {source.LastName}";
    }
}*/