using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices
{
    public interface ITagService
    {
        Task<List<Tag>> GetTags();
        Task<Tag> GetTag(string id);
        Task UpdateTag(Tag tagToUpdate);
        Task DeleteTag(string id);
        Task CreateTag(Tag tagToCreate);
    }
}
