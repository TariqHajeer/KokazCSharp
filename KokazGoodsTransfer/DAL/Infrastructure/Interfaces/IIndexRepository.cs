using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface IIndexRepository<T>:IRepository<T> where T: class,IIndex
    {
        Task<IEnumerable<IndexEntity>> GetLiteList();
    }
}
