using KitchenConnection.DataLayer.Data.Repository.IRepository;
using KitchenConnection.DataLayer.Data.Repository;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.DataLayer.Data.UnitOfWork;

public interface IUnitOfWork
{
    public IKitchenConnectionRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    bool Complete();
}
