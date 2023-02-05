using KitchenConnection.Models.DTOs.Collection;

namespace KitchenConnection.BusinessLogic.Services.IServices {
    public interface ICollectionService
    {
        Task<CollectionDTO> Create(CollectionCreateRequestDTO collectionToCreate, Guid userId);
        Task<CollectionDTO> Update(CollectionUpdateDTO collectionToUpdate);
        Task<List<CollectionDTO>> GetAll();
        Task<CollectionDTO> Get(Guid id);
        Task<CollectionDTO> Delete(Guid id);
    }
}
