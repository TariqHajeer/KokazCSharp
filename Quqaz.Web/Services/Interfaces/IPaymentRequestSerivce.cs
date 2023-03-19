using Quqaz.Web.Dtos.PayemntRequestDtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IPaymentRequestSerivce
    {
        Task<bool> CanClientRequest(int clientId);
        Task<PaymentRequestDto> Create(CreatePaymentRequestDto createPaymentRequestDto);
        Task Delete(int id);
        Task<PagingResualt<IEnumerable<PaymentRequestDto>>> GetByClient(PagingDto pagingDto,int clientId);
        Task<IEnumerable<PaymentRequestDto>> New();
        Task Accept(int id);
        Task DisAccept(int id);
        Task<PagingResualt<IEnumerable<PaymentRequestDto>>>Get(PagingDto pagingDto, PaymentFilterDto paymentFilterDto);
    }
}
