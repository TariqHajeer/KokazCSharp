using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Models.Infrastrcuter;
using KokazGoodsTransfer.Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface ICRUDService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class, IIdEntity where TDTO : class where CreateDto : class where UpdateDto : class
    {
        Task<TDTO> GetById(int id);
        Task<IEnumerable<TDTO>> GetByIds(IEnumerable<int> ids);
        Task<ErrorRepsonse<TDTO>> AddAsync(CreateDto entity);
        Task<IEnumerable<TDTO>> GetAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors);
        Task<ErrorRepsonse<TDTO>> Update(UpdateDto updateDto);
        Task<ErrorRepsonse<TDTO>> Delete(int id);
        Task<bool> Any(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TDTO>> GetAll(params Expression<Func<TEntity, object>>[] propertySelectors);
        Task<IEnumerable<TDTO>> GetAll(string[] propertySelectors);
        Task<IEnumerable<TDTO>> AddRangeAsync(IEnumerable<CreateDto> entities);



    }
}
