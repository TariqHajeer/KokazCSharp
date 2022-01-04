using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly KokazContext _kokazContext;

        public Repository(KokazContext kokazContext)
        {
            this._kokazContext = kokazContext;
        }
        public async Task AddAsyc(T entity)
        {
            await _kokazContext.AddAsync(entity);
        }

        public IQueryable<T> GetIQueryable()
        {
            return _kokazContext.Set<T>().AsQueryable();
        }
    }
}
