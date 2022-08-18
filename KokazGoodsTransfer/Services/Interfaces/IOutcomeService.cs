using KokazGoodsTransfer.Dtos.OutComeDtos;
using KokazGoodsTransfer.Models;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.DAL.Helper;
using System.Collections.Generic;
namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IOutcomeService : ICRUDService<OutCome, OutComeDto, CreateOutComeDto, UpdateOuteComeDto>
    {
        Task<PagingResualt<IEnumerable<OutComeDto>>> GetAsync(Filtering filtering, PagingDto pagingDto);
    }
}
