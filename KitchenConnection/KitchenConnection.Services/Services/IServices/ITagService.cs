using KitchenConnection.Models.DTOs.Tag;

namespace KitchenConnection.BusinessLogic.Services.IServices;
public interface ITagService
{
    Task<TagDTO> Create(TagCreateDTO tagToCreate);
    Task<List<TagDTO>> GetAll();
    Task<TagDTO> Get(Guid id);
    Task<TagDTO> Update(TagDTO tagToUpdate);
    Task<TagDTO> Delete(Guid id);
}
