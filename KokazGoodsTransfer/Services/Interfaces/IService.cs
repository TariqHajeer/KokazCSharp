using KokazGoodsTransfer.DAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IService<TEntity,TDTO> where TEntity : class where TDTO : class
    {
        Task<TDTO> AddAsync(TEntity entity);
        Task<List<TDTO>> GetAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors);
        Task<PagingResualt<List<TDTO>>> GetAsync(Paging paging, Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors);


    }
}
