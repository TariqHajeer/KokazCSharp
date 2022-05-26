using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using KokazGoodsTransfer.DAL.Helper;
using System.Linq;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entityes);
        Task<T> FirstOrDefualt(Expression<Func<T, bool>> filter = null);
        Task<T> FirstOrDefualt(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors);
        Task<PagingResualt<IEnumerable<T>>> GetAsync(Paging paging, string[] propertySelectors, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<PagingResualt<IEnumerable<T>>> GetAsync(Paging paging, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors);
        Task<IEnumerable<T>> GetByFilterInclue(Expression<Func<T, bool>> filter, string[] propertySelectors);
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] propertySelectors);
        Task<IEnumerable<T>> GetAll(string[] propertySelectors);
        Task Update(T entity);
        Task Delete(T entity);
        Task Update(IEnumerable<T> entites);
        Task<T> GetById(int Id);
        Task<bool> Any(Expression<Func<T, bool>> filter = null);
        Task LoadCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class;
        Task LoadRefernces<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class;
        Task<int> Count(Expression<Func<T, bool>> filter = null);
        Task<PagingResualt<IEnumerable<T>>> GetAsync(Paging paging, Expression<Func<T, bool>> filter = null, string[] propertySelectors = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<IEnumerable<Projection>> Select<Projection>(Expression<Func<T, bool>> filter, Expression<Func<T, Projection>> projection, params Expression<Func<T, object>>[] propertySelectors);
    }
}
