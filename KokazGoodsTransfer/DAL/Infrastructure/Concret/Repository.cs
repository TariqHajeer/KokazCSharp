using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Helpers.Extensions;
using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly KokazContext _kokazContext;

        public Repository(KokazContext kokazContext)
        {
            this._kokazContext = kokazContext;
        }
        public virtual async Task AddAsync(T entity)
        {
            await _kokazContext.AddAsync(entity);
            await _kokazContext.SaveChangesAsync();
        }
        public virtual async Task<T> GetById(int Id)
        {
            return await _kokazContext.Set<T>().FindAsync(Id);
        }

        public virtual async Task<List<T>> GetAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = this._kokazContext.Set<T>().AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (propertySelectors != null)
            {
                foreach (var item in propertySelectors)
                {
                    query = query.Include(item);
                }
            }
            return await query.ToListAsync();
        }
        private IQueryable<T> IncludeLmbda(IQueryable<T> query, params Expression<Func<T, object>>[] propertySelectors)
        {
            if (propertySelectors != null)
            {
                foreach (var item in propertySelectors)
                {
                    query = query.Include(item.AsPath());
                }

            }
            return query;
        }

        public virtual async Task<PagingResualt<List<T>>> GetAsync(Paging paging, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors)
        {

            var query = this._kokazContext.Set<T>().AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            var totalTask = query.CountAsync();
            if (propertySelectors != null)
            {
                foreach (var item in propertySelectors)
                {
                    query = query.Include(item);
                }
            }
            var dataTask = query.Skip((paging.Page - 1) * paging.RowCount).Take(paging.RowCount).ToListAsync();
            var result = new PagingResualt<List<T>>()
            {
                Total = await totalTask,
                Data = await dataTask
            };
            return result;
        }
        public virtual async Task<List<T>> GetAll(params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = _kokazContext.Set<T>().AsQueryable();
            query = IncludeLmbda(query, propertySelectors);
            return await query.ToListAsync();
        }


        public virtual async Task Update(T entity)
        {
            _kokazContext.Entry(entity).State = EntityState.Modified;
            _kokazContext.Set<T>().Update(entity);
            await _kokazContext.SaveChangesAsync();
        }

        public virtual async Task Delete(T entity)
        {
            _kokazContext.Set<T>().Remove(entity);
            await _kokazContext.SaveChangesAsync();
        }

        public virtual async Task Update(IEnumerable<T> entites)
        {
            _kokazContext.UpdateRange(entites);
            await _kokazContext.SaveChangesAsync();
        }


    }
}
