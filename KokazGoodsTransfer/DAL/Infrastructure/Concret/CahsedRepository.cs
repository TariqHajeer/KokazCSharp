using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Infrastrcuter;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class CahsedRepository<T> : Repository<T>, ICashedRepository<T> where T:class,IIdEntity
    {
        private readonly IMemoryCache _cache;
        public CahsedRepository(KokazContext kokazContext, IMemoryCache cache) : base(kokazContext)
        {
            _cache = cache;
        }
        public override async Task AddAsync(T entity)
        {
            await base.AddAsync(entity);
            var cashedName = typeof(T).FullName;
            if (_cache.TryGetValue(cashedName, out List<T> cahsedList))
            {
                cahsedList.Add(entity);
            }
        }
        public override async Task<List<T>> GetAll(params string[] propertySelectors)
        {
            var name = typeof(T).FullName;
            if (!_cache.TryGetValue(name, out List<T> entities))
            {
                entities = await base.GetAll(propertySelectors);
                _cache.Set(name, entities);
            }
            return entities;
        }
        public override Task Delete(T entity)
        {
            return base.Delete(entity);

            var cashedName = typeof(T).FullName;
            if (_cache.TryGetValue(cashedName, out List<T> cahsedList))
            {
                cahsedList = cahsedList.Where(c => c.Id != entity.Id).ToList();
                _cache.Set(cashedName, cahsedList);
            }

        }
        public override async Task Update(T entity)
        {
            await base.Update(entity);
            var cashedName = typeof(T).FullName;
            if (_cache.TryGetValue(cashedName, out List<T> cahsedList))
            {
                cahsedList = cahsedList.Where(c => c.Id != entity.Id).ToList();
                cahsedList.Add(entity);
                _cache.Set(cashedName, cahsedList);
            }
        }
        public override Task<List<T>> GetAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors)
        {
            return base.GetAsync(filter, propertySelectors);
        }

    }
}
