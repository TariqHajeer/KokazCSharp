using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendOrderReciveNotifcation(IEnumerable<Order> orders);
    }
}
