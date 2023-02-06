using KitchenConnection.Models.DTOs.Collection;
using KitchenConnection.Models.DTOs.CookBook;

namespace KitchenConnection.BusinessLogic.Services.IServices {
    public interface ICollectionService
    {
        Task<CollectionDTO> Create(Guid userId,CollectionCreateRequestDTO collectionToCreate);
        Task<CollectionDTO> Update(Guid userId, CollectionUpdateDTO collectionToUpdate);
        Task<List<CollectionDTO>> GetAll(Guid userId);
        Task<CollectionDTO> Get(Guid userId,Guid id);
        Task<CollectionDTO> Delete(Guid userId, Guid id);
        Task<CollectionDTO> AddRecipeToCollection(Guid userId, Guid cookBookId, Guid recipeId);
        Task<CollectionDTO> RemoveRecipeFromCollection(Guid userId, Guid cookBookId, Guid recipeId);
        Task<List<CollectionDTO>> GetPaginated(int page, int pageSize, Guid userId);
    }
}
