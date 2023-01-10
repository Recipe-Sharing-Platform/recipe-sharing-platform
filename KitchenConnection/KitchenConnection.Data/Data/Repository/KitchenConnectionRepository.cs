using KitchenConnection.DataLayer.Data.Repository.IRepository;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KitchenConnection.DataLayer.Data.Repository;
public class KitchenConnectionRepository<Tentity> : IKitchenConnectionRepository<Tentity> where Tentity : BaseEntity {
    private readonly KitchenConnectionDbContext _dbContext;

    public KitchenConnectionRepository(KitchenConnectionDbContext dbContext) {
        _dbContext = dbContext;
    }

    public IQueryable<Tentity> GetByCondition(Expression<Func<Tentity, bool>> expression) => 
        _dbContext.Set<Tentity>().Where(expression);

    public IQueryable<Tentity> GetByConditionPaginated(Expression<Func<Tentity, bool>> expression, Expression<Func<Tentity, object>> orderBy, int page, int pageSize, bool orderByDescending = true) {
        const int defaultPageNumber = 1;

        var query = _dbContext.Set<Tentity>().Where(expression);

        // Check if the page number is greater then zero - otherwise use default page number
        if (page <= 0) {
            page = defaultPageNumber;
        }

        // It is necessary sort items before it
        query = orderByDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public IQueryable<Tentity> GetAll() =>
        _dbContext.Set<Tentity>();

    public IQueryable<Tentity> GetById(Expression<Func<Tentity, bool>> expression) =>
       _dbContext.Set<Tentity>().Where(expression);


    public async Task<Tentity> Create(Tentity entity) {
        var createdEntity = await _dbContext.Set<Tentity>().AddAsync(entity);
        return createdEntity.Entity;
    }

    public async Task CreateRange(List<Tentity> entities) {
        await _dbContext.Set<Tentity>().AddRangeAsync(entities);
    }

    public void Delete(Tentity entity) {
        _dbContext.Set<Tentity>().Remove(entity);
    }

    public void DeleteRange(List<Tentity> entities) {
        _dbContext.Set<Tentity>().RemoveRange(entities);
    }

    public Tentity Update(Tentity entity) =>
        _dbContext.Set<Tentity>().Update(entity).Entity;


    public void UpdateRange(List<Tentity> entities) {
        _dbContext.Set<Tentity>().UpdateRange(entities);
    }

    public IQueryable<Tentity> GetByConditionWithIncludes(Expression<Func<Tentity, bool>> expression, string? includeRelations = null) {
        var query = _dbContext.Set<Tentity>().Where(expression);

        if (!string.IsNullOrEmpty(includeRelations)) {
            var relations = includeRelations.Split(",");

            foreach (var relation in relations) {
                query = query.Include(relation);
            }
        }

        return query;
    }
}
