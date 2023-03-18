using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Models.Static
{
    public enum OrderState
    {
        //قيد المعالجة
        Processing=1,
        //يوجب اخذ نقود من العميل
        ShortageOfCash=2,
        //الطلب منتهي
        Finished=3
    }
}
