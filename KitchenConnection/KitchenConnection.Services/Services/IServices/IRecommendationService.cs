using KitchenConnection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices
{
    public interface IRecommendationsService
    {
        Task<bool> SetScore(Guid? userId, Guid? tagId);
        Task<bool> SetScore(Guid? userId, Guid? tagId, int? score);
        Task<Recipe> GetSingleRecommendation(Guid? userId);
        Task<List<Recipe>> GetCollectionRecommendations(Guid? userId, int length);
    }
}
