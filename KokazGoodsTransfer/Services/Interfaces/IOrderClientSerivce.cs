using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IOrderClientSerivce
    {
        Task Delete(int id);
        Task Send(int[] ids);
        Task<IEnumerable<OrderDto>> NonSendOrder();
        Task<PagingResualt<IEnumerable<OrderDto>>> Get(PagingDto pagingDto, COrderFilter orderFilter);
        Task<bool> CodeExist(string code);
        Task<bool> CheckOrderTypesIdsExists(int[] ids);
        Task<IEnumerable<PayForClientDto>> OrdersDontFinished(OrderDontFinishFilter orderDontFinishFilter);
        Task<OrderDto> GetById(int id);
        Task CorrectOrderCountry(List<KeyValuePair<int, int>> pairs);
        Task<IEnumerable<OrderFromExcel>> OrdersNeedToRevision();
        Task<OrderResponseClientDto> Create(CreateOrderFromClient createOrderFromClient);
        Task<List<string>> Validate(CreateOrderFromClient createOrderFromClient);
        Task<bool> CreateFromExcel(IFormFile file,DateTime dateTime);
        Task Edit(EditOrder editOrder);
    }
}
