using KitchenConnection.DataLayer.Models.DTOs.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices
{
    public interface ICollectionService
    {
        Task<CollectionDTO> Create(CollectionCreateDTO collectionToCreate);
        Task<CollectionDTO> Update(CollectionUpdateDTO collectionToUpdate);
        Task<List<CollectionDTO>> GetAll();
        Task<CollectionDTO> Get(Guid id);
        Task<CollectionDTO> Delete(Guid id);
    }
}
