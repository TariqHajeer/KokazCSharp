using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class CountryCashedRepository : CashedRepository<Country>, ICountryCashedRepository
    {
        public CountryCashedRepository(KokazContext kokazContext, IMemoryCache cache) : base(kokazContext, cache)
        {
            Query = this._kokazContext.Countries.AsQueryable();
        }
        public override async Task<List<Country>> GetAll(params Expression<Func<Country, object>>[] propertySelectors)
        {
            return await base.GetAll(c => c.Clients, c => c.Regions, c => c.AgentCountrs.Select(c => c.Agent));
        }
        public override async Task RefreshCash()
        {
            var name = typeof(Country).FullName;
            _cache.Remove(name);
            await GetAll(c => c.Clients, c => c.Regions, c => c.AgentCountrs.Select(c => c.Agent));
        }
    }
}
