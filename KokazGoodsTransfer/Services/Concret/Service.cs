using AutoMapper;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class Service<TEntity, TDTO, CreateDto, UpdateDto> : IService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class where TDTO : class where CreateDto : class where UpdateDto : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        public Service(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<ErrorRepsonse<TDTO>> AddAsync(CreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            await _repository.AddAsync(entity);
            var response = new ErrorRepsonse<TDTO>(_mapper.Map<TDTO>(entity));
            return response;
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

        public virtual async Task<List<TDTO>> GetAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var list = await _repository.GetAsync(filter, propertySelectors);
            var response = _mapper.Map<TDTO[]>(list);
            return response.ToList();
        }

        public virtual async Task<PagingResualt<List<TDTO>>> GetAsync(Paging paging, Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return null;
        }

        public async Task<ErrorRepsonse<TDTO>> Update(UpdateDto updateDto)
        {
            var entity = _mapper.Map<TEntity>(updateDto);
            await _repository.Update(entity);
            var response = new ErrorRepsonse<TDTO>(_mapper.Map<TDTO>(entity));
            return response;
        }
    }
}
