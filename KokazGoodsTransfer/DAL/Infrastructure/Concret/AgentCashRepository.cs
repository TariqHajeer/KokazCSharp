using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class AgentCashRepository : CashedRepository<User>, IAgentCashRepository
    {
        public AgentCashRepository(KokazContext kokazContext, IMemoryCache cache) : base(kokazContext, cache)
        {
            Query = this._kokazContext.Users.Where(c => c.CanWorkAsAgent == true);
        }
        public override Task<List<User>> Get(Expression<Func<User, bool>> filter = null, params Expression<Func<User, object>>[] propertySelectors)
        {
            return base.Get(c => c.CanWorkAsAgent == true && c.IsActive == true, c => c.UserPhones, c => c.AgentCountrs.Select(c => c.Country.Regions));
        }
        public override async Task RefreshCash()
        {
            var name = typeof(User).FullName;
            _cache.Remove(name);
            await Get();
        }

    }
}
