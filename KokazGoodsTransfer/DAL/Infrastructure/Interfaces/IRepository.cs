using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface IRepository<T>
    {
        Task Add(T entity);
        Task<List<T>> GetAll();

    }
}
