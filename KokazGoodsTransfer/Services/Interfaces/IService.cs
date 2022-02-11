using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class where TDTO : class where CreateDto : class where UpdateDto : class
    {
        Task<ErrorRepsonse<TDTO>> AddAsync(CreateDto entity);
        Task<List<TDTO>> GetAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors);
        //Task<PagingResualt<List<TDTO>>> GetAsync(Paging paging, Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors);
        Task<ErrorRepsonse<TDTO>> Update(UpdateDto updateDto);
        Task<ErrorRepsonse<TDTO>> Delete(int id);
        Task<bool> Any(Expression<Func<TEntity,bool>> expression);
        Task<List<TDTO>> GetALl(params Expression<Func<TEntity, object>>[] propertySelectors);


    }
}
