using KitchenConnection.DataLayer.Data;
using KitchenConnection.DataLayer.Data.Repository;
using KitchenConnection.DataLayer.Data.Repository.IRepository;
using KitchenConnection.Models.Entities;
using System.Collections;

namespace KitchenConnection.DataLayer.Data.UnitOfWork;

//The unit of work class serves one purpose: to make sure that when you use multiple repositories,
//they share a single database context.
//That way, when a unit of work is complete you can call the SaveChanges method on that instance
//of the context and be assured that all related changes will be coordinated.
public class UnitOfWork : IUnitOfWork {
    private readonly KitchenConnectionDbContext _context;

    private Hashtable _repositories;

    public UnitOfWork(KitchenConnectionDbContext context) {
        _context = context;
    }
    public bool Complete() {
        var numberOfAffectedRows = _context.SaveChanges();
        return numberOfAffectedRows > 0;
    }

    public IKitchenConnectionRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity {
        if (_repositories == null) _repositories = new Hashtable();

        var type = typeof(TEntity).Name;
        if (!_repositories.ContainsKey(type)) {
            var repositoryType = typeof(KitchenConnectionRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

            _repositories.Add(type, repositoryInstance);
        }

        return _repositories[type] as IKitchenConnectionRepository<TEntity>;
    }
}
