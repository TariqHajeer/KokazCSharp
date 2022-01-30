using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.NotifcationDtos
{
    public class AdminNotification
    {
        public int NewOrdersCount { get; set; }
        public int NewOrdersDontSendCount { get; set; }
        public int OrderRequestEditStateCount { get; set; }
        public int NewEditRquests { get; set;  }
        public int NewPaymentRequetsCount { get; set; }
    }
}
