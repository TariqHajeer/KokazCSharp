using System.Collections.Generic;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface IUintOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task Commit();
        Task Update<TEntity>(TEntity entity) where TEntity : class;
        Task UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        Task Add<TEntity>(TEntity entity) where TEntity : class;
        Task AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        Task Remove<TEntity>(TEntity entity) where TEntity : class;
        Task RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        Task BegeinTransaction();
        Task Rollback();
        bool IsTransactionOpen { get; }

    }
}
