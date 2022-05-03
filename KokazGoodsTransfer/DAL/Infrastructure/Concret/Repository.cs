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
        protected IQueryable<T> Query;
        public Repository(KokazContext kokazContext)
        {
            this._kokazContext = kokazContext;
            Query = _kokazContext.Set<T>().AsQueryable();


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

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = this._kokazContext.Set<T>().AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = IncludeLmbda(query, propertySelectors);
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
        public virtual async Task<PagingResualt<IEnumerable<T>>> GetAsync(Paging paging, Expression<Func<T, bool>> filter = null, string[] propertySelectors =null)
        {
            var query = _kokazContext.Set<T>().AsQueryable();
            if (filter != null)
                query = query.Where(filter);
            if(propertySelectors.Any())
                foreach (var item in propertySelectors)
                {
                    query = query.Include(item);
                }
            var total = await query.CountAsync();
            return new PagingResualt<IEnumerable<T>>()
            {
                Data = await query.ToListAsync(),
                Total = total,
            };
        }
        public virtual async Task<PagingResualt<IEnumerable<T>>> GetAsync(Paging paging, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors)
        {

            var query = this._kokazContext.Set<T>().AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            var totalTask = query.CountAsync();
            query = IncludeLmbda(query, propertySelectors);
            var dataTask = query.Skip((paging.Page - 1) * paging.RowCount).Take(paging.RowCount).ToListAsync();
            var result = new PagingResualt<IEnumerable<T>>()
            {
                Total = await totalTask,
                Data = await dataTask
            };
            return result;
        }
        public virtual async Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] propertySelectors)
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

        public async Task<T> FirstOrDefualt(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
                return await _kokazContext.Set<T>().FirstOrDefaultAsync(filter);
            return await _kokazContext.Set<T>().FirstOrDefaultAsync();
        }
        public async Task<T> FirstOrDefualt(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = _kokazContext.Set<T>().AsQueryable();
            query = IncludeLmbda(query, propertySelectors);
            if (filter != null)
                query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
                return await _kokazContext.Set<T>().AnyAsync(filter);
            return await _kokazContext.Set<T>().AnyAsync();
        }

        public async Task LoadCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class
        {
            await _kokazContext.Entry(entity).Collection(propertyExpression).LoadAsync();
        }

        public async Task LoadRefernces<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class
        {
            await _kokazContext.Entry(entity).Reference(propertyExpression).LoadAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
                return await _kokazContext.Set<T>().CountAsync();
            return await _kokazContext.Set<T>().CountAsync(filter);
        }

        public async Task<IEnumerable<T>> GetAll(string[] propertySelectors)
        {
            var query = Query;
            if (propertySelectors?.Any() == true)
            {

                foreach (var item in propertySelectors)
                {
                    query = query.Include(item);
                }
            }
            return await query.ToListAsync();
        }
    }
}
