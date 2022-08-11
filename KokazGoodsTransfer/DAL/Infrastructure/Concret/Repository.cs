using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Helpers.Extensions;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Infrastrcuter;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly KokazContext _kokazContext;
        protected IQueryable<T> Query;
        protected readonly int branchId;
        public Repository(KokazContext kokazContext, IHttpContextAccessorService httpContextAccessorService)
        {
            this._kokazContext = kokazContext;
            Query = _kokazContext.Set<T>().AsQueryable();
            if (IsIHaveBranch() || IsIMaybeHaveBranch())
            {
                if (httpContextAccessorService.UserBranches().Any())
                {
                    branchId = httpContextAccessorService.CurrentBranchId();
                    if (IsIHaveBranch())
                    {
                        Query = Query.Where(c => ((IHaveBranch)c).BranchId == branchId);
                    }
                    if (IsIMaybeHaveBranch())
                    {
                        Query = Query.Where(c => ((IMaybeHaveBranch)c).BranchId == null || ((IMaybeHaveBranch)c).BranchId == branchId);
                    }
                }
            }

        }
        private bool IsIMaybeHaveBranch()
        {
            return typeof(IMaybeHaveBranch).IsAssignableFrom(typeof(T));
        }
        private bool IsIHaveBranch()
        {
            return typeof(IHaveBranch).IsAssignableFrom(typeof(T));
        }
        public virtual async Task AddRangeAsync(IEnumerable<T> entityes)
        {
            var data = entityes.ToList();

            if (IsIHaveBranch() || IsIMaybeHaveBranch())
            {
                data.ForEach(entity => ((IHaveBranch)entity).BranchId = branchId);
            }

            await _kokazContext.AddRangeAsync(data);
            await _kokazContext.SaveChangesAsync();
        }
        public virtual async Task AddAsync(T entity)
        {
            if (IsIHaveBranch())
            {
                ((IHaveBranch)entity).BranchId = branchId;
            }
            await _kokazContext.AddAsync(entity);
            await _kokazContext.SaveChangesAsync();
        }
        public virtual async Task<T> GetById(int Id)
        {
            return await _kokazContext.Set<T>().FindAsync(Id);
        }


        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = Query;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = IncludeLmbda(query, propertySelectors);
            return await query.ToListAsync();
        }
        protected IQueryable<T> IncludeLmbda(IQueryable<T> query, params Expression<Func<T, object>>[] propertySelectors)
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
        public virtual async Task<PagingResualt<IEnumerable<T>>> GetAsync(Paging paging, Expression<Func<T, bool>> filter = null, string[] propertySelectors = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = Query;
            if (filter != null)
                query = query.Where(filter);
            if (propertySelectors?.Any() == true)
                foreach (var item in propertySelectors)
                {
                    query = query.Include(item);
                }
            var total = await query.CountAsync();
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return new PagingResualt<IEnumerable<T>>()
            {
                Data = await query.Skip((paging.Page - 1) * paging.RowCount).Take(paging.RowCount).ToListAsync(),
                Total = total,
            };
        }
        public virtual async Task<PagingResualt<IEnumerable<T>>> GetAsync(Paging paging, string[] propertySelectors, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var query = Query;
            if (propertySelectors?.Any() == true)
                foreach (var item in propertySelectors)
                {
                    query = query.Include(item);
                }
            if (orderBy != null)
                query = orderBy(query);
            var total = await query.CountAsync();
            return new PagingResualt<IEnumerable<T>>()
            {
                Data = await query.Skip((paging.Page - 1) * paging.RowCount).Take(paging.RowCount).ToListAsync(),
                Total = total,
            };
        }
        public virtual async Task<IEnumerable<T>> GetByFilterInclue(Expression<Func<T, bool>> filter, string[] propertySelectors)
        {
            var query = Query;
            if (filter != null)
                query = query.Where(filter);
            if (propertySelectors?.Any() == true)
                foreach (var item in propertySelectors)
                {
                    query = query.Include(item);
                }
            return await query.ToListAsync();
        }
        public virtual async Task<PagingResualt<IEnumerable<T>>> GetAsync(Paging paging, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors)
        {

            var query = this.Query;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            var total = await query.CountAsync();
            query = IncludeLmbda(query, propertySelectors);
            var data = await query.Skip((paging.Page - 1) * paging.RowCount).Take(paging.RowCount).ToListAsync();
            var result = new PagingResualt<IEnumerable<T>>()
            {
                Total = total,
                Data = data
            };
            return result;
        }
        public virtual async Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = Query;
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
        public async Task Delete(IEnumerable<T> entities)
        {
            _kokazContext.RemoveRange(entities);
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
            var query = Query;
            query = IncludeLmbda(query, propertySelectors);
            if (filter != null)
                query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
                return await Query.AnyAsync(filter);
            return await Query.AnyAsync();
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
                return await Query.CountAsync();
            return await Query.CountAsync(filter);
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

        public async Task<IEnumerable<Projection>> Select<Projection>(Expression<Func<T, bool>> filter, Expression<Func<T, Projection>> projection, params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = Query.Where(filter);
            query = IncludeLmbda(query, propertySelectors);
            return await query.Select(projection).ToListAsync();
        }

        public async Task<T> FirstOrDefualt(Expression<Func<T, bool>> filter, string[] propertySelectors)
        {
            var query = Query.Where(filter);
            if (propertySelectors?.Any() == true)
            {
                foreach (var item in propertySelectors)
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<decimal> Sum(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> filter = null)
        {
            return await Query.Where(filter).SumAsync(selector);
        }


    }
}
