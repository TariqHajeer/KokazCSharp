using System.Threading.Tasks;
using System.Collections.Generic;
using Quqaz.Web.Models;
using Quqaz.Web.Dtos.NotifcationDtos;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Services.Interfaces
{
    public interface INotificationService
    {
        Task<PagingResualt<List<NewNotificationDto>>> GetNotifications(PagingDto paging);
        Task SendOrderReciveNotifcation(IEnumerable<Order> orders);
        Task<AdminNotification> GetAdminNotification();
        Task SendClientNotification();
        Task SeeNotifactions(int[] ids);
        Task<IEnumerable<NotificationDto>> GetClientNotifcations();
        Task<int> NewNotfiactionCount();
    }
}
