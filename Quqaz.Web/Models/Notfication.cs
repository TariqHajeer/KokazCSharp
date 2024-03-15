#nullable disable

using System;
using Quqaz.Web.Models.Static;

namespace Quqaz.Web.Models
{
    public partial class Notfication
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationType NotificationType{get;set;}
        public string NotificationExtraData{get;set;}
        public string Note { get; set; }
        public int? OrderCount { get; set; }
        public OrderPlace? OrderPlace { get; set; }
        public MoneyPalce? MoneyPlace { get; set; }
        public bool? IsSeen { get; set; }
        public DateTime? CreatedDate { set; get; }

        public virtual Client Client { get; set; }
    }
}
