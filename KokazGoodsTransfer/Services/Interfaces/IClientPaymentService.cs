using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IClientPaymentService
    {
        Task<PrintOrdersDto> GetFirst(Expression<Func<ClientPayment, bool>> filter, string[] includes);
    }
}
