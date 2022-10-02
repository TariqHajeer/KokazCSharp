using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Models.Static
{
    public enum OrderplacedEnum
    {
        [Description("عند العميل")]
        Client=1,
        [Description("في المخزن")]
        Store=2,
        [Description("في الطريق")]
        Way=3,
        [Description("تم التسليم")]
        Delivered=4,
        [Description("مرتجع كلي")]
        CompletelyReturned=5,
        [Description("مرتجع جزئي")]
        PartialReturned= 6,
        [Description("مرفوض")]
        Unacceptable =7 ,
        [Description("مؤجل")]
        Delayed=8,
    }
}
