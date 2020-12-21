using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Classes
{
    //public class AbstractUnitOfWork
    //{
        
    //    protected DbContext _context;
    //    public DbContext Context { get { return _context; } }
    //    protected Dictionary<string, object> _repositories;
    //    protected bool _disposed;
    //    public AbstractUnitOfWork(DbContext context)
    //         => _context = context;

    //    public abstract void Commit();
    //    public bool IHaveTransaction()
    //    {
    //        return this.transaction != null;
    //    }
    //    public abstract void Dispose();
    //    public abstract void Update<TEntity>(TEntity entity, string updatedBy) where TEntity : class, IEntity;
    //    protected abstract void Dispose(bool disposing);

    //    public abstract Repository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;
    //    public abstract void Add<TEntity>(TEntity entity, string createdBy) where TEntity : class, IEntity;


    //    public abstract void BegeinTransaction();

    //    public abstract void RoleBack();

    //    public abstract void CommitTransaction();
    //    public abstract TEntity FirstOrDefult<TEntity>() where TEntity : class, IEntity;
    //    public abstract void Remove<TEntity>(TEntity entity, string RemovedBy) where TEntity : class, IEntity;
    //}
}
