using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Models.Infrastrcuter;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors);
        Task<PagingResualt<List<T>>> GetAsync(Paging paging, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors);
        Task<List<T>> GetAll(params string[] propertySelectors);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
