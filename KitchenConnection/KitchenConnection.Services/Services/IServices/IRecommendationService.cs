using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Services.IServices {
    public interface IRecommendationsService
    {
        Task<bool> SetScore(Guid? userId, Guid? tagId);
        Task<bool> SetScore(Guid? userId, Guid? tagId, int? score);
        Task<Recipe> GetSingleRecommendation(Guid? userId);
        Task<List<Recipe>> GetCollectionRecommendations(Guid? userId, int length);
    }
}
