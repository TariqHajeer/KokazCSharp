using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace KokazGoodsTransfer.Services.Concret
{
    public class OrderTypeCashService : CashService<OrderType, OrderTypeDto, CreateOrderType, UpdateOrderTypeDto>,IOrderTypeCashService
    {
        public OrderTypeCashService(IRepository<OrderType> repository, IMapper mapper, IMemoryCache cache, Logging logging, IHttpContextAccessorService httpContextAccessorService) : base(repository, mapper, cache, logging, httpContextAccessorService)
        {
        }

    }
}
