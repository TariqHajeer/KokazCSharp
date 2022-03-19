using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface ICashedRepository<T> : IRepository<T> where T : class, IIdEntity
    {
        Task RefreshCash();
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] propertySelectors);
    }
}
