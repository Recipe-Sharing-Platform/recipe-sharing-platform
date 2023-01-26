using KitchenConnection.DataLayer.Models.DTOs.CookBook;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices;

public interface ICookBookService
{
    Task<CookBookDTO> Create(CookBookCreateDTO cookBookToCreate);
    Task<List<CookBookDTO>> GetAll();
    Task<CookBookDTO> Get(Guid id);
    Task<CookBookDTO> Update(CookBookUpdateDTO cookbookToUpdate);
    Task<CookBookDTO> Delete(Guid id);
}