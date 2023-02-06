using AutoMapper;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.DTOs.Tag;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public async Task<TagDTO> Create(TagCreateDTO tagToCreate)
    {
        var tag = await _unitOfWork.Repository<Tag>().Create(_mapper.Map<Tag>(tagToCreate));
        _unitOfWork.Complete();

        return _mapper.Map<TagDTO>(tag);
    }

    public async Task<List<TagDTO>> GetAll()
    {
        var tags = await _unitOfWork.Repository<Tag>().GetAll().ToListAsync();
        _unitOfWork.Complete();

        if (tags == null) return null;

        return _mapper.Map<List<TagDTO>>(tags);
    }

    public async Task<TagDTO> Get(Guid id)
    {
        Expression<Func<Tag, bool>> expression = x => x.Id == id;
        var tag = await _unitOfWork.Repository<Tag>().GetById(expression).FirstOrDefaultAsync();
        _unitOfWork.Complete();

        if (tag == null) return null;

        return _mapper.Map<TagDTO>(tag);
    }

    public async Task<TagDTO> Update(TagDTO tagToUpdate)
    {
        Expression<Func<Tag, bool>> expression = x => x.Id == tagToUpdate.Id;
        var tag = await _unitOfWork.Repository<Tag>().GetById(expression).FirstOrDefaultAsync();

        tag.Name = tagToUpdate.Name;

        _unitOfWork.Repository<Tag>().Update(tag);
        _unitOfWork.Complete();

        return _mapper.Map<TagDTO>(tag);
    }

    public async Task<TagDTO> Delete(Guid id)
    {
        Expression<Func<Tag, bool>> expression = x => x.Id == id;
        var tag = await _unitOfWork.Repository<Tag>().GetById(expression).FirstOrDefaultAsync();

        _unitOfWork.Repository<Tag>().Delete(tag);
        _unitOfWork.Complete();

        return _mapper.Map<TagDTO>(tag);
    }
}