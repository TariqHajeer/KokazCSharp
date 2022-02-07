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
    public class CashedRepository<T> : Repository<T>, ICashedRepository<T> where T : class, IIdEntity
    {
        protected readonly IMemoryCache _cache;
        public CashedRepository(KokazContext kokazContext, IMemoryCache cache) : base(kokazContext)
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
        public override async Task<T> GetById(int Id)
        {
            var name = typeof(T).FullName;
            T entity = null;
            if (_cache.TryGetValue(name, out List<T> entities))
            {
                entity = entities.Find(c => c.Id == Id);
            }
            if (entity == null)
                entity = await _kokazContext.Set<T>().FindAsync(Id);
            return entity;
        }


        public override async Task<List<T>> GetAll(params Expression<Func<T, object>>[] propertySelectors)
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
            try
            {
                await base.Update(entity);
            }
            catch (Exception ex)
            {
                await _kokazContext.Entry(entity).ReloadAsync();
                throw ex;
            }
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
        public override async Task Update(IEnumerable<T> entites)
        {
            try
            {
                
                await base.Update(entites);
            }
            catch (Exception ex)
            {
                foreach (var item in entites)
                {
                    await _kokazContext.Entry(item).ReloadAsync();
                }
                throw ex;
            }
            var cahedName = typeof(T).FullName;
            if (_cache.TryGetValue(cahedName, out List<T> cahsedList))
            {
                cahsedList = cahsedList.Where(c => !entites.Select(e => e.Id).Contains(c.Id)).ToList();
                cahsedList.AddRange(entites);
                _cache.Set(cahedName, cahsedList);
            }
        }

        public virtual async Task RefreshCash()
        {
            var name = typeof(T).FullName;
            _cache.Remove(name);
            await GetAll();
        }
    }
}
