using Quqaz.Web.Dtos.OrdersTypes;
using Quqaz.Web.Models;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IOrderTypeCashService : ICashService<OrderType, OrderTypeDto, CreateOrderType, UpdateOrderTypeDto>
    {
    }
}
