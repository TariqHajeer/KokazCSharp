using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IClientPaymentService
    {
        
        Task<PrintOrdersDto> GetFirst(Expression<Func<ClientPayment, bool>> filter, params string[] includes);
        Task<PagingResualt<PrintDto>> GetOrdersByPrintNumberId(int printNumberId, PagingDto paging);
        Task<string> GetReceiptAsHtml(int id);
        Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetClientprints(PagingDto paging,int? number,string code);
    }
}
