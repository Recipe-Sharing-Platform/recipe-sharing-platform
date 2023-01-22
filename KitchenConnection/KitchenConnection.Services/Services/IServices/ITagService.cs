using KitchenConnection.Application.Models.DTOs.Recipe;
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
       // Task UpdateTag(Tag tagToUpdate);//needs to be reviewed
        Task<bool> DeleteTag(Guid id);
        Task<Tag> CreateTag(TagCreateDTO tagToCreate);
    }
