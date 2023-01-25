using AutoMapper;
using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
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
        public readonly IMapper _mapper;
        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task UpdateTag(TagDTO tagToUpdate)
        {
            Tag? tag = await GetTag(tagToUpdate.Id);

            tag.Name = tagToUpdate.Name; 

            _unitOfWork.Repository<Tag>().Update(tag);

            _unitOfWork.Complete();
        }

        public async Task DeleteTag(Guid id)
        {
            var tag = await GetTag(id);

            _unitOfWork.Repository<Tag>().Delete(tag);

            _unitOfWork.Complete();
        }
        
        public async Task CreateTag(TagCreateDTO tagToCreate)
        {
        var tag = _mapper.Map<Tag>(tagToCreate);

        _unitOfWork.Repository<Tag>().Create(tag);

        _unitOfWork.Complete();
    }
    }

