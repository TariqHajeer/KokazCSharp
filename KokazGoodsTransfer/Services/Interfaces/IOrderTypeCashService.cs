using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IOrderTypeCashService : ICashService<OrderType, OrderTypeDto, CreateOrderType, UpdateOrderTypeDto>
    {
    }
}
