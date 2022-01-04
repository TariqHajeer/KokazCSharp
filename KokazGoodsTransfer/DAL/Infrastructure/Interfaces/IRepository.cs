using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface IRepository<T> where T: class
    {
        Task AddAsyc(T entity);
        IQueryable<T> GetIQueryable();


    }
}
