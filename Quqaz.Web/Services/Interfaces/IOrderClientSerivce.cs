using System.Threading.Tasks;
using System.Collections.Generic;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using Quqaz.Web.Dtos.Statics;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IOrderClientSerivce
    {
        Task Delete(int id);
        Task Send(int[] ids);
        Task<PagingResualt<IEnumerable<OrderDto>>> NonSendOrder(PagingDto paging);
        Task<PagingResualt<IEnumerable<OrderDto>>> Get(PagingDto pagingDto, COrderFilter orderFilter);
        Task<bool> CodeExist(string code);
        Task<bool> CheckOrderTypesIdsExists(int[] ids);
        public Task<string> GetOrderDoseNotFinish(OrderDontFinishFilter orderDontFinishFilter);
        Task<PagingResualt<List<PayForClientDto>>> OrdersDontFinished(OrderDontFinishFilter orderDontFinishFilter, PagingDto paging);
        Task<OrderDto> GetById(int id);
        Task CorrectOrderCountry(List<KeyValuePair<int, int>> pairs);
        Task<IEnumerable<OrderFromExcel>> OrdersNeedToRevision();
        Task<OrderResponseClientDto> Create(CreateOrderFromClient createOrderFromClient);
        Task<List<string>> Validate(CreateOrderFromClient createOrderFromClient);
        Task<bool> ImportFromExcel(IFormFile file, DateTime dateTime);
        Task Edit(EditOrder editOrder);
        Task<string> GetReceipt(int id);
        Task<List<ClientTrackShipmentDto>> GetShipmentTracking(int id);
        Task<List<AccountReportDto>> AccountReport(DateRangeFilter dateRangeFilter);
        Task<List<AccountReportDto>> GetPhoneOrderStatusCount(string phone);
        Task<ClientOrderReportDto> GetOrderStaticsReport(DateRangeFilter dateRangeFilter);

    }
}
