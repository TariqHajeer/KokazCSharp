using Quqaz.Web.Models.Static;

namespace Quqaz.Web.Dtos.NotifcationDtos
{
    public class CreateNotificationDto
    {
        public string Title { set; get; }
        public string Body { set; get; }
        public int ClientId { set; get; }
        public NotificationType NotificationType{get;set;}
        public string NotificationExtraData{get;set;}
    }
}