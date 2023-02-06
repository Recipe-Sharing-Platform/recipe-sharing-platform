using FluentValidation;
using KitchenConnection.BusinessLogic.Services;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.EntityValidators;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.DTOs.Collection;
using KitchenConnection.Models.DTOs.CookBook;
using KitchenConnection.Models.DTOs.Cuisine;
using KitchenConnection.Models.DTOs.Ingredient;
using KitchenConnection.Models.DTOs.Instruction;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.DTOs.Tag;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace KitchenConnection.BusinessLogic.Helpers {
    public static class StartupHelper
    {        
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RecipeCreateDTO>, RecipeValidator>();
            services.AddScoped<IValidator<RecipeIngredientCreateDTO>, IngredientValidator>();
            services.AddScoped<IValidator<RecipeInstructionCreateDTO>, InstructionValidator>();
            services.AddScoped<IValidator<TagCreateDTO>, TagValidator>();
            services.AddScoped<IValidator<CookBookCreateDTO>, CookBookValidator>();
            services.AddScoped<IValidator<CollectionCreateDTO>, CollectionValidator>();
            services.AddScoped<IValidator<CuisineCreateDTO>, CuisineValidator>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IRecipeService, RecipeService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<ICookBookService, CookBookService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ICollectionService, CollectionService>();
            services.AddTransient<IRecommendationsService, RecommendationsService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddTransient<IRecipeNutrientsService, RecipeNutrientsService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IShoppingListService, ShoppingListService>();
            services.AddTransient<ICuisineService, CuisineService>();
        }
    }
}
