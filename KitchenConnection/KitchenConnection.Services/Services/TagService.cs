using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services;

    public class TagService : ITagService
    {
        public readonly IUnitOfWork _unitOfWork;
        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Tag>> GetTags()
        {
            return await _unitOfWork.Repository<Tag>().GetAll().ToListAsync();
        }

        public async Task<Tag> GetTag(string id)
        {

            Expression<Func<Tag, bool>> expression = x => x.Id == id;
            var tag = await _unitOfWork.Repository<Tag>().GetById(expression).FirstOrDefaultAsync();

            return tag;
        }

        public async Task UpdateTag(Tag tagToUpdate)
        {
            Tag? tag = await GetTag(tagToUpdate.Id);

            tag.Name = tagToUpdate.Name;

            _unitOfWork.Repository<Tag>().Update(tag);

            _unitOfWork.Complete();
        }

        public async Task DeleteTag(string id)
        {
            var tag = await GetTag(id);

            _unitOfWork.Repository<Tag>().Delete(tag);

            _unitOfWork.Complete();
        }
        
        public async Task CreateTag(Tag tagToCreate)
        {
            await _unitOfWork.Repository<Tag>().Create(tagToCreate);
            _unitOfWork.Complete();
        }
    }

