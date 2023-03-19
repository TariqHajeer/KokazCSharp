using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Models.Infrastrcuter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IIndexService<TEntity> where TEntity : class, IIndex
    {
        Task<IEnumerable<NameAndIdDto>> GetAllLite();
    }
}
