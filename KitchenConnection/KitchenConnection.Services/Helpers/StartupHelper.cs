using FluentValidation;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.BusinessLogic.Services;
using KitchenConnection.DataLayer.Data.EntityValidators;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.DTOs.Collection;
using KitchenConnection.DataLayer.Models.DTOs.CookBook;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace KitchenConnection.BusinessLogic.Helpers
{
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
