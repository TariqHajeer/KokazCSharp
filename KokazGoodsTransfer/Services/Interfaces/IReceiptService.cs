using KokazGoodsTransfer.Dtos.ReceiptDtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Clients;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IReceiptService
    {
        Task Delete(int id);
        Task<ReceiptDto> GetById(int id);
        Task<IEnumerable<ReceiptDto>> UnPaidRecipt(int clientId);
        Task<PagingResualt<IEnumerable<ReceiptDto>>> Get(PagingDto pagingDto, AccountFilterDto accountFilterDto);

    }
}
