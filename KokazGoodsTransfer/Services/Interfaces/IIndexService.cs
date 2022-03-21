using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models.Infrastrcuter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IIndexService<TEntity> where TEntity : class, IIndex
    {
        Task<IEnumerable<NameAndIdDto>> GetAllLite();
    }
}
