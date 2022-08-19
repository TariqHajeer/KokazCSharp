using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.DAL.Helper;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IOrderClientSerivce
    {
        Task Delete(int id);
        Task Send(int[] ids);
        Task<IEnumerable<OrderDto>> NonSendOrder();
        Task<PagingResualt<IEnumerable<OrderDto>>> Get(PagingDto pagingDto, COrderFilter orderFilter);
        Task<bool> CodeExist(string code);
        Task<bool> CheckOrderTypesIdsExists(int[] ids);
    }
}
