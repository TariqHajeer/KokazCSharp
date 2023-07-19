using Quqaz.Web.Dtos.OutComeDtos;
using Quqaz.Web.Models;
using System.Threading.Tasks;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.DAL.Helper;
using System.Collections.Generic;
namespace Quqaz.Web.Services.Interfaces
{
    public interface IOutcomeService : ICRUDService<OutCome, OutComeDto, CreateOutComeDto, UpdateOuteComeDto>
    {
        Task<PagingResualt<IEnumerable<OutComeDto>>> GetAsync(Filtering filtering, PagingDto pagingDto);
    }
}
