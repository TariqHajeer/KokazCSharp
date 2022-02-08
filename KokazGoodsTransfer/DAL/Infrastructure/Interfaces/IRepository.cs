using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using KokazGoodsTransfer.DAL.Helper;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> FirstOrDefualt(Expression<Func<T, bool>> filter = null);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors);
        Task<PagingResualt<List<T>>> GetAsync(Paging paging, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors);
        Task<List<T>> GetAll(params Expression<Func<T, object>>[] propertySelectors);
        Task Update(T entity);
        Task Delete(T entity);
        Task Update(IEnumerable<T> entites);
        Task<T> GetById(int Id);
        Task<bool> Any(Expression<Func<T, bool>> filter = null);
        Task LoadCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class;
        Task LoadRefernces<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class;
    }
}
