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
        Task<List<Tag>> GetTags();
        Task<Tag> GetTag(Guid id);
        Task UpdateTag(TagDTO tagToUpdate);
        Task DeleteTag(Guid id);
        Task CreateTag(TagCreateDTO tagToCreate);
    }
