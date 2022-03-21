using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class UnitOfWork : IUintOfWork
    {
        private readonly KokazContext _kokazContext;
        private IDbContextTransaction _dbContextTransaction;

        private Dictionary<string, object> _repositories;
        public UnitOfWork(KokazContext kokazContext)
        {
            _kokazContext = kokazContext;
        }
        
        public async Task Add<TEntity>(TEntity entity) where TEntity : class
        {
            await Repository<TEntity>().AddAsync(entity);
        }

        public async Task BegeinTransaction()
        {
            _dbContextTransaction = await _kokazContext.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _dbContextTransaction.CommitAsync();
            _dbContextTransaction.Dispose();
            _dbContextTransaction = null;

        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();

            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repository<TEntity>(_kokazContext);
                _repositories.Add(type, repositoryInstance);
            }
            return (Repository<TEntity>)_repositories[type];
        }

        public async Task RoleBack()
        {
            await _dbContextTransaction.RollbackAsync();
            _dbContextTransaction.Dispose();
            _dbContextTransaction = null;
        }

        public async Task Update<TEntity>(TEntity entity) where TEntity : class
        {
            await Repository<TEntity>().Update(entity);
        }
    }
}
