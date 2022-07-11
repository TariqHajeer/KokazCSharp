﻿using AutoMapper;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Infrastrcuter;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class CRUDService<TEntity, TDTO, CreateDto, UpdateDto> : ICRUDService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class, IIdEntity where TDTO : class where CreateDto : class where UpdateDto : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        protected readonly Logging _logging;
        protected readonly int _currentBranch;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly int[] _branchesIds;
        public CRUDService(IRepository<TEntity> repository, IMapper mapper, Logging logging, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _logging = logging;
            _httpContextAccessor = httpContextAccessor;
            string branchId = _httpContextAccessor.HttpContext.Request.Headers["branchId"];
            if (branchId == null)
                _currentBranch = 2;
            _currentBranch = Convert.ToInt32(branchId);
            _branchesIds = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == "BranchId").Select(c => Convert.ToInt32(c)).ToArray();
        }
        public virtual async Task<TDTO> GetById(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null)
                return null;
            return _mapper.Map<TDTO>(entity);
        }

        public virtual Task<IEnumerable<TDTO>> GetByIds(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<ErrorRepsonse<TDTO>> AddAsync(CreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            await _repository.AddAsync(entity);
            var response = new ErrorRepsonse<TDTO>(_mapper.Map<TDTO>(entity));
            return response;
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> expression)
        {
            return await _repository.Any(expression);
        }

        public virtual async Task<ErrorRepsonse<TDTO>> Delete(int id)
        {
            var response = new ErrorRepsonse<TDTO>();
            var entity = await _repository.GetById(id);
            if (entity == null)
            {
                response.Errors.Add("Not.Found");
                return response;
            }
            await _repository.Delete(entity);
            response.Data = _mapper.Map<TDTO>(entity);
            return response;

        }

        public virtual async Task<IEnumerable<TDTO>> GetAll(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var list = await _repository.GetAll(propertySelectors);
            return _mapper.Map<TDTO[]>(list).ToList();
        }
        public async Task<IEnumerable<TDTO>> GetAll(string[] propertySelectors)
        {
            var list = await _repository.GetAll(propertySelectors);
            return _mapper.Map<TDTO[]>(list).ToList();
        }

        public virtual async Task<IEnumerable<TDTO>> GetAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var list = await _repository.GetAsync(filter, propertySelectors);
            var response = _mapper.Map<TDTO[]>(list);
            return response.ToList();
        }




        public virtual async Task<ErrorRepsonse<TDTO>> Update(UpdateDto updateDto)
        {
            var entity = _mapper.Map<TEntity>(updateDto);
            await _repository.Update(entity);
            var response = new ErrorRepsonse<TDTO>(_mapper.Map<TDTO>(entity));
            return response;
        }

        public virtual Task<IEnumerable<TDTO>> AddRangeAsync(IEnumerable<CreateDto> entities)
        {
            throw new NotImplementedException();
        }
    }
}
