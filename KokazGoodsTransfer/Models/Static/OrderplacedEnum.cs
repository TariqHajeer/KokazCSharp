using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Models.Static
{
    public enum OrderplacedEnum
    {
        Client=1,
        Store=2,
        Way=3,
        Delivered=4,
        CompletelyReturned=5,
        PartialReturned= 6,
        Unacceptable =7 ,
        Delayed=8
    }
}
