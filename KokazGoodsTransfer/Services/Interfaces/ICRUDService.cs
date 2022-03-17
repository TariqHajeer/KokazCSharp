﻿using KokazGoodsTransfer.DAL.Helper;
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
        Task<List<TDTO>> GetByIds(IEnumerable<int> ids);
        Task<ErrorRepsonse<TDTO>> AddAsync(CreateDto entity);
        Task<List<TDTO>> GetAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors);
        Task<ErrorRepsonse<TDTO>> Update(UpdateDto updateDto);
        Task<ErrorRepsonse<TDTO>> Delete(int id);
        Task<bool> Any(Expression<Func<TEntity,bool>> expression);
        Task<List<TDTO>> GetAll(params Expression<Func<TEntity, object>>[] propertySelectors);
        


    }
}
