using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.DAL.Helper;
using System.Linq.Expressions;
using System;

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
        Task<IEnumerable<CodeStatus>> GetCodeStatuses(int clientId, string[] codes);
        Task<GenaricErrorResponse<ReceiptOfTheOrderStatusDto, string, IEnumerable<string>>> GetReceiptOfTheOrderStatusById(int id);
        Task<PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>> GetReceiptOfTheOrderStatus(PagingDto Paging, string code);
        Task<GenaricErrorResponse<int, string, string>> MakeOrderInWay(int[] ids);
        Task<PagingResualt<IEnumerable<OrderDto>>> GetOrderFiltered(PagingDto pagingDto, OrderFilter orderFilter);
        Task<IEnumerable<OrderDto>> GetAll(Expression<Func<Order, bool>> expression, string[] propertySelector = null);
        Task CreateOrder(CreateOrdersFromEmployee createOrdersFromEmployee);
        Task CreateOrders(IEnumerable<CreateMultipleOrder> createMultipleOrders);
        Task<bool> Any(Expression<Func<Order, bool>> expression);
        Task<int> DeleiverMoneyForClient(DeleiverMoneyForClientDto deleiverMoneyForClientDto);
        Task Delete(int id);
        Task<IEnumerable<OrderDto>> ForzenInWay(FrozenOrder frozenOrder);
        Task<OrderDto> GetById(int id);
        Task<IEnumerable<PayForClientDto>> OrdersDontFinished(OrderDontFinishedFilter orderDontFinishedFilter);
        Task<IEnumerable<OrderDto>> NewOrderDontSned();
        Task<IEnumerable<OrderDto>> OrderAtClient(OrderFilter orderFilter);
        Task<PayForClientDto> GetByCodeAndClient(int clientId, string code);
        Task ReiveMoneyFromClient(int[] ids);
        Task<EarningsDto> GetEarnings(PagingDto pagingDto, DateFiter dateFiter);
        Task<IEnumerable<OrderDto>> GetAsync(Expression<Func<Order, bool>> expression, string[] propertySelector = null);
        Task Accept(IdsDto idsDto);
        Task AcceptMultiple(IEnumerable<IdsDto> idsDtos);
        Task DisAccept(DateWithId<int> dateWithId);
        Task DisAcceptMultiple(DateWithId<List<int>> dateWithIds);
        Task ReSend(OrderReSend orderReSend);
        Task<OrderDto> MakeOrderCompletelyReturned(int id);
        Task AddPrintNumber(int orderId);
        Task AddPrintNumber(int[] orderIds);
        Task<IEnumerable<OrderDto>> GetOrderByAgent(string orderCode);
        Task TransferOrderToAnotherAgnet(TransferOrderToAnotherAgnetDto transferOrderToAnotherAgnetDto);
        Task Edit(UpdateOrder updateOrder);
    }
}
