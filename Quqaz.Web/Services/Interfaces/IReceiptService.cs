using Quqaz.Web.Dtos.ReceiptDtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.Clients;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IReceiptService
    {
        Task Delete(int id);
        Task<ReceiptDto> GetById(int id);
        Task<IEnumerable<ReceiptDto>> UnPaidRecipt(int clientId);
        Task<PagingResualt<IEnumerable<ReceiptDto>>> Get(PagingDto pagingDto, AccountFilterDto accountFilterDto);

    }
}
