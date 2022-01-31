using AutoMapper;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class Service<TEntity,TDTO> : IService<TEntity,TDTO> where TEntity : class where TDTO:class
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        public Service(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<TDTO> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual  async Task<List<TDTO>> GetAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var list = await _repository.GetAsync(filter, propertySelectors);
            var response = _mapper.Map<TDTO[]>(list);
            return response.ToList();
        }

        public Task<PagingResualt<List<TDTO>>> GetAsync(Paging paging, Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            throw new NotImplementedException();
        }
    }
}
