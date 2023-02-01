using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices;
public interface ITagService
{
    Task<TagDTO> Create(TagCreateDTO tagToCreate);
    Task<List<TagDTO>> GetAll();
    Task<TagDTO> Get(Guid id);
    Task<TagDTO> Update(TagDTO tagToUpdate);
    Task<TagDTO> Delete(Guid id);
}
