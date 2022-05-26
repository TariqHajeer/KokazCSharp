using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.DAL.Helper;

namespace KokazGoodsTransfer.Services.Interfaces
{
    /// <summary>
    /// Related With Employee
    /// </summary>
    public partial interface IOrderService
    {
        Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheDeliveredShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentWithCostDto> receiptOfTheStatusOfTheDeliveredShipmentDtos);
        Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheReturnedShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentDto> receiptOfTheStatusOfTheDeliveredShipmentDtos);
        Task<GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>> GetOrderToReciveFromAgent(string code);
        Task<GenaricErrorResponse<ReceiptOfTheOrderStatusDto, string, IEnumerable<string>>> GetReceiptOfTheOrderStatusById(int id);
        Task<PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>> GetReceiptOfTheOrderStatus(PagingDto Paging, string code);
        Task<GenaricErrorResponse<int, string, string>> MakeOrderInWay(DateWithId<int[]> dateWithId);
    }
    /// <summary>
    /// Related With Agent
    /// </summary>
    public partial interface IOrderService
    {
    }
    /// <summary>
    /// Realted with Client
    /// </summary>
    public partial interface IOrderService
    {
        Task<GenaricErrorResponse<int, string, string>> DeleiverMoneyForClient(DeleiverMoneyForClientDto deleiverMoneyForClientDto);
    }
}
