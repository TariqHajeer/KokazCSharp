using System.Threading.Tasks;
using System.Collections.Generic;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Models;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.DAL.Helper;
using System.Linq.Expressions;
using System;
using Quqaz.Web.Dtos.OrdersDtos.OrderWithBranchDto;
using Quqaz.Web.Dtos.OrdersDtos.Queries;
using Quqaz.Web.Dtos.OrdersDtos.Commands;

namespace Quqaz.Web.Services.Interfaces
{
    /// <summary>
    /// Related With Employee
    /// </summary>
    public partial interface IOrderService
    {
        Task<PagingResualt<IEnumerable<OrderDto>>> GetNegativeAlert(PagingDto pagingDto, OrderFilter orderFilter);
        Task<PagingResualt<IEnumerable<OrderDto>>> GetInStockToTransferWithAgent(PagingDto pagingDto, OrderFilter orderFilter);
        Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheDeliveredShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentWithCostDto> receiptOfTheStatusOfTheDeliveredShipmentDtos);
        Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheReturnedShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentDto> receiptOfTheStatusOfTheDeliveredShipmentDtos);
        Task<GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>> GetOrderToReciveFromAgent(string code);
        Task<IEnumerable<CodeStatus>> GetCodeStatuses(int clientId, string[] codes);
        Task<GenaricErrorResponse<ReceiptOfTheOrderStatusDto, string, IEnumerable<string>>> GetReceiptOfTheOrderStatusById(int id);
        Task<PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>> GetReceiptOfTheOrderStatus(PagingDto Paging, string code);
        Task<int> MakeOrderInWay(MakeOrderInWayDto makeOrderInWayDto);
        Task<int> TransferToSecondBranch(TransferToSecondBranchDto transferToSecondBranchDto);
        Task<string> GetTransferToSecondBranchReportAsString(int id);
        Task<string> GetDeleiverMoneyForClient(int id);
        Task<PagingResualt<IEnumerable<OrderDto>>> GetOrderFiltered(PagingDto pagingDto, OrderFilter orderFilter);
        Task<IEnumerable<OrderDto>> GetAll(Expression<Func<Order, bool>> expression, string[] propertySelector = null);
        Task CreateOrder(CreateOrderFromEmployee createOrdersFromEmployee);
        Task CreateOrderForOtherBranch(CreateOrderFromEmployee createOrdersFromEmployee);
        Task CreateOrders(IEnumerable<CreateMultipleOrder> createMultipleOrders);
        Task<bool> Any(Expression<Func<Order, bool>> expression);
        Task<int> DeleiverMoneyForClient(DeleiverMoneyForClientDto2 deleiverMoneyForClientDto2);
        Task<(PagingResualt<IEnumerable<PayForClientDto>> Data, decimal orderCostTotal, decimal deliveryCostTOtal, decimal payForClientTotal)> OrdersDontFinished2(OrderDontFinishedFilter orderDontFinishedFilter, PagingDto pagingDto);
        Task Delete(int id);
        Task<PagingResualt<IEnumerable< OrderDto>>> ForzenInWay(PagingDto pagingDto,FrozenOrder frozenOrder);
        Task<OrderDto> GetById(int id);
        Task<PagingResualt<IEnumerable<PayForClientDto>>> OrdersDontFinished(OrderDontFinishedFilter orderDontFinishedFilter, PagingDto pagingDto);
        Task<IEnumerable<OrderDto>> NewOrderDontSned();
        Task<IEnumerable<OrderDto>> OrderAtClient(OrderFilter orderFilter);
        Task<PayForClientDto> GetByCodeAndClient(int clientId, string code);
        Task ReiveMoneyFromClient(int[] ids);
        Task<EarningsDto> GetEarnings(PagingDto pagingDto, DateFiter dateFiter);
        Task<IEnumerable<OrderDto>> GetAsync(Expression<Func<Order, bool>> expression, string[] propertySelector = null);
        Task Accept(OrderIdAndAgentId idsDto);
        Task AcceptMultiple(IEnumerable<OrderIdAndAgentId> idsDtos);
        Task DisAccept(DateWithId<int> dateWithId);
        Task DisAcceptMultiple(DateWithId<List<int>> dateWithIds);
        Task ReSend(OrderReSend orderReSend);
        Task<OrderDto> MakeOrderCompletelyReturned(int id);
        Task AddPrintNumber(int orderId);
        Task AddPrintNumber(int[] orderIds);
        Task<IEnumerable<OrderDto>> GetOrderByAgent(string orderCode);
        Task TransferOrderToAnotherAgnet(TransferOrderToAnotherAgnetDto transferOrderToAnotherAgnetDto);
        Task Edit(UpdateOrder updateOrder);
        Task EditForOthrBrnach(UpdateOrder updateOrder);
        Task<int> DeleiverMoneyForClientWithStatus(int[] ids);
        Task<PrintOrdersDto> GetOrderByClientPrintNumber(int printNumber);
        Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetClientprint(PagingDto pagingDto, int? number, string clientName, string code);
        Task<PagingResualt<IEnumerable<OrderDto>>> DisAccpted(PagingDto pagingDto, OrderFilter orderFilter);
        Task<int> Count(Expression<Func<Order, bool>> filter = null);
        Task<IEnumerable<ApproveAgentEditOrderRequestDto>> GetOrderRequestEditState();
        Task DisAproveOrderRequestEditState(int[] ids);
        Task AproveOrderRequestEditState(int[] ids);
        Task<IEnumerable<string>> GetCreatedByNames();
        Task<IEnumerable<OrderDto>> GetForReSendMultiple(string code);
        Task ReSendMultiple(List<OrderReSend> orderReSends);
        Task<PagingResualt<IEnumerable<OrderDto>>> GetInStockToTransferToSecondBranch(SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto);
        Task<PagingResualt<IEnumerable<OrderDto>>> GetOrdersComeToMyBranch(PagingDto pagingDto, OrderFilter orderFilter);
        Task ReceiveOrdersToMyBranch(IEnumerable<SetOrderAgentToMyBranchDto> receiveOrdersToMyBranchDtos);
        Task ReceiveOrdersToMyBranch(ReceiveOrdersToMyBranchDto receiveOrdersToMyBranchDto);
        Task DisApproveOrderComeToMyBranch(int id);
        Task<int> SendOrdersReturnedToSecondBranch(ReturnOrderToMainBranchDto returnOrderToMainBranchDto);
        Task<string> GetSendOrdersReturnedToSecondBranchReportAsString(int id);
        Task<PagingResualt<IEnumerable<OrderDto>>> GetOrdersReturnedToMyBranch(PagingDto pagingDto, int currentBranchId);
        Task ReceiveReturnedToMyBranch(SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto);
        Task<PagingResualt<IEnumerable<TransferToSecondBranchReportDto>>> GetPrintsTransferToSecondBranch(PagingDto pagingDto, int destinationBranchId);
        Task<PagingResualt<IEnumerable<TransferToSecondBranchDetialsReportDto>>> GetPrintTransferToSecondBranchDetials(PagingDto pagingDto, int id);
        Task<PagingResualt<IEnumerable<OrderDto>>> GetOrdersReturnedToSecondBranch(SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto);
        Task<PagingResualt<IEnumerable<OrderDto>>> GetDisApprovedOrdersReturnedByBranch(PagingDto pagingDto);
        Task SetDisApproveOrdersReturnByBranchInStore(SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto);
        Task DisApproveReturnedToMyBranch(int id);
        Task DisApproveReturnedToMyBranch(OrderIdAndNote orderIdAndNote);
        Task<IEnumerable<OrderDto>> GetOrdersByAgentRegionAndCode(GetOrdersByAgentRegionAndCodeQuery getOrderByAgentRegionAndCode);
        Task<IEnumerable<OrderDto>> GetOrderInAllBranches(string code);
        Task DeleteNegativeAlert(int id);
    }
}
