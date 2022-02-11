using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface IUintOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task Commit();
        Task Update<TEntity>(TEntity entity) where TEntity : class;
        Task Add<TEntity>(TEntity entity) where TEntity : class;
        Task BegeinTransaction();
        Task RoleBack();

    }
}
