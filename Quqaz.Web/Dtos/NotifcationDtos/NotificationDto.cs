using Quqaz.Web.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.NotifcationDtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public int? OrderCount { get; set; }
        public bool? IsSeen { get; set; }
        public NameAndIdDto MoneyPlaced { get; set; }
        public NameAndIdDto OrderPlaced { get; set; }

    }
}
