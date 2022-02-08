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
    public class ClientCashedRepository : CashedRepository<Client>, IClientCahedRepository
    {
        public ClientCashedRepository(KokazContext kokazContext, IMemoryCache cache) : base(kokazContext, cache)
        {
        }
        public override Task<List<Client>> GetAll(params Expression<Func<Client, object>>[] propertySelectors)
        {
            return base.GetAll(c=>c.Country,c=>c.ClientPhones);
        }
        
    }
}
