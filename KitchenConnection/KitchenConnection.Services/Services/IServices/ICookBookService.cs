using KitchenConnection.Models.DTOs.CookBook;

namespace KitchenConnection.BusinessLogic.Services.IServices;

public interface ICookBookService
{
    Task<CookBookDTO> Create(CookBookCreateRequestDTO cookBookToCreate, Guid userId);
    Task<List<CookBookDTO>> GetAll();
    Task<CookBookDTO> Get(Guid id);
    Task<CookBookDTO> Update(Guid userId,CookBookUpdateDTO cookbookToUpdate);
    Task<CookBookDTO> Delete(Guid userId,Guid id);
}