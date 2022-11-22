using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;
using Microsoft.AspNetCore.Http;
using KokazGoodsTransfer.Services.Interfaces;

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class UnitOfWork : IUintOfWork
    {
        private readonly KokazContext _kokazContext;
        private IDbContextTransaction _dbContextTransaction;
        private readonly IHttpContextAccessorService _httpContextAccessorService;
        private Dictionary<string, object> _repositories;

        public bool IsTransactionOpen => _dbContextTransaction != null;

        public UnitOfWork(KokazContext kokazContext, IHttpContextAccessorService httpContextAccessorService)
        {
            _kokazContext = kokazContext;
            _httpContextAccessorService = httpContextAccessorService;
        }

        public async Task Add<TEntity>(TEntity entity) where TEntity : class
        {
            await Repository<TEntity>().AddAsync(entity);
        }
        public async Task AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            await Repository<TEntity>().AddRangeAsync(entities);
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

                if (typeof(TEntity) == typeof(Order))
                {
                    var repositoryInstance = new OrderRepository(_kokazContext, _httpContextAccessorService);
                    _repositories.Add(type, repositoryInstance);
                }
                else
                {
                    var repositoryInstance = new Repository<TEntity>(_kokazContext, _httpContextAccessorService);
                    _repositories.Add(type, repositoryInstance);
                }
            }
            return (Repository<TEntity>)_repositories[type];
        }

        public async Task Rollback()
        {
            await _dbContextTransaction.RollbackAsync();
            _dbContextTransaction.Dispose();
            _dbContextTransaction = null;
        }
        public async Task Update<TEntity>(IEnumerable<TEntity> entity) where TEntity : class
        {
            await Repository<TEntity>().Update(entity);
        }
        public async Task Update<TEntity>(TEntity entity) where TEntity : class
        {
            await Repository<TEntity>().Update(entity);
        }

        public async Task UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            await Repository<TEntity>().Update(entities);
        }

        public async Task Remove<TEntity>(TEntity entity) where TEntity : class
        {
            await Repository<TEntity>().Delete(entity);
        }

        public async Task RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            await Repository<TEntity>().Delete(entities);
        }
    }
}
