using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IClientPaymentService
    {
        Task<PrintOrdersDto> GetFirst(Expression<Func<ClientPayment, bool>> filter, string[] includes);
    }
}
