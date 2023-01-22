using AutoMapper;
using Azure.Identity;
using KitchenConnection.Application.Models.DTOs.Recipe;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Tag>> GetTags()
        {
            return await _unitOfWork.Repository<Tag>().GetAll().ToListAsync();
        }

        public async Task<Tag> GetTag(Guid id)
        {

            Expression<Func<Tag, bool>> expression = x => x.Id == id;
            var tag = await _unitOfWork.Repository<Tag>().GetById(expression).FirstOrDefaultAsync();

            return tag;
        }

        //NOTE: to be reviewed
       /* public async Task<Tag> UpdateTag(TagDTO tagToUpdate)
        {
            Tag? tag = await GetTag(tagToUpdate.Id);

            tag.Name = tagToUpdate.Name;//tag's 'set' is private - can't update

            _unitOfWork.Repository<Tag>().Update(tag);

            _unitOfWork.Complete();
        }*/

        public async Task<bool> DeleteTag(Guid id)
        {
            var tag = await GetTag(id);

            _unitOfWork.Repository<Tag>().Delete(tag);

            var res=_unitOfWork.Complete();
            
            return res;
        }
        
        public async Task<Tag> CreateTag(TagCreateDTO tagToCreate)
        {
            var tag= _mapper.Map<Tag>(tagToCreate);

            await _unitOfWork.Repository<Tag>().Create(tag);

            var res=_unitOfWork.Complete();

            if (res) 
            {
                return tag;
            }
            else
            {
                return null;
            }
        }
    }

