﻿using KitchenConnection.Models.Entities;
using System.Linq.Expressions;

namespace KitchenConnection.DataLayer.Data.Repository.IRepository;

public interface IKitchenConnectionRepository<Tentity> where Tentity : BaseEntity {
    IQueryable<Tentity> GetByCondition(Expression<Func<Tentity, bool>> expression);
    IQueryable<Tentity> GetByConditionWithIncludes(Expression<Func<Tentity, bool>> expression, string? includeRelations = null);

    IQueryable<Tentity> GetByConditionPaginated(Expression<Func<Tentity, bool>> expression, Expression<Func<Tentity, object>> orderBy, int page, int pageSize, bool orderByDescending = true);

    IQueryable<Tentity> GetPaginated(int page, int pageSize);
    IQueryable<Tentity> GetAll();

    IQueryable<Tentity> GetById(Expression<Func<Tentity, bool>> expression);

    Task<Tentity> Create(Tentity entity);
    Task CreateRange(List<Tentity> entity);

    void Delete(Tentity entity);
    void DeleteRange(List<Tentity> entity);

    Tentity Update(Tentity entity);
    void UpdateRange(List<Tentity> entity);
}
