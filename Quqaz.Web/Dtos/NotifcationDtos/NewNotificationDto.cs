using System.Collections.Generic;
using Quqaz.Web.Models.Static;

namespace Quqaz.Web.Dtos.NotifcationDtos
{
    public class NewNotificationDto
    {
        public int Id {get;set;}
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationType NotificationType{get;set;}
        public string NotificationExtraData{get;set;}
        public Dictionary<string, string> Data { get; set; }
        public bool? IsSeen { get; set; }
    }
}