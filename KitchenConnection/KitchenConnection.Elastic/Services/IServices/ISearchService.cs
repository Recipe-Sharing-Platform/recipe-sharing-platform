using KitchenConnection.Models.Entities;

namespace KitchenConnection.Elastic.Services.IServices; 
public interface ISearchService {
    Task<List<Recipe>> SearchRecipes(string query);
}
