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

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class IndexRepository<T> : Repository<T>, IIndexRepository<T> where T : class, IIndex
    {
        public IndexRepository(KokazContext kokazContext) : base(kokazContext)
        {
        }

        public virtual async Task<List<IndexEntity>> GetLiteList()
        {
            var query = _kokazContext.Set<T>().Select(c => new IndexEntity() { Id = c.Id, Name = c.Name }).ToListAsync();
            return await query;
            
        }
    }
}
