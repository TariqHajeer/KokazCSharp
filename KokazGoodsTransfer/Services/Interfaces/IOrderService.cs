using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Dtos.Common;
namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheDeliveredShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentWithCostDto> receiptOfTheStatusOfTheDeliveredShipmentDtos);
        Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheReturnedShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentDto> receiptOfTheStatusOfTheDeliveredShipmentDtos);
        Task<GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>> GetOrderToReciveFromAgent(string code);
    }
}
