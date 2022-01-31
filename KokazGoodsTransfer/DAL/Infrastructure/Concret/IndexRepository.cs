using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class IndexRepository<T> : Repository<T>, IIndexRepository<T> where T : class, IIndex
    {
        private readonly IMemoryCache _cache;
        public IndexRepository(KokazContext kokazContext, IMemoryCache cache) : base(kokazContext)
        {
            _cache = cache;
        }

        public virtual async Task<List<IndexEntity>> GetLiteList()
        {

            if (!_cache.TryGetValue(nameof(T), out List<IndexEntity> entities))
            {
                var list = await _kokazContext.Set<T>().Select(c => new IndexEntity() { Id = c.Id, Name = c.Name }).ToListAsync();
                entities = list;
                _cache.Set(nameof(T), entities);
            }
            return entities;
        }
    }
}
