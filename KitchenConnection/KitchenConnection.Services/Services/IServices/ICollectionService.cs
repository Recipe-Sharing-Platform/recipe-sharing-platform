using KitchenConnection.Models.DTOs.Collection;

namespace KitchenConnection.BusinessLogic.Services.IServices {
    public interface ICollectionService
    {
        Task<CollectionDTO> Create(Guid userId,CollectionCreateRequestDTO collectionToCreate);
        Task<CollectionDTO> Update(Guid userId, CollectionUpdateDTO collectionToUpdate);
        Task<List<CollectionDTO>> GetAll(Guid userId);
        Task<CollectionDTO> Get(Guid userId,Guid id);
        Task<CollectionDTO> Delete(Guid userId, Guid id);
    }
}
