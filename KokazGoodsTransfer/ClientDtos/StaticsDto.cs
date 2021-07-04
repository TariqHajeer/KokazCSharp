using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.ClientDtos
{
    public class StaticsDto
    {
        /// <summary>
        /// إجمالي الطرود
        /// </summary>
        public int TotalOrder { get; set; }
        /// <summary>
        /// طلبات مبالغها داخل الشركة
        /// </summary>
        public int OrderMoneyInCompany { get; set; }
        /// <summary>
        /// طرود تم تسليمها للزبون
        /// </summary>
        public int OrderDeliverdToClient { get; set; }

        /// <summary>
        /// طرود تم إستلام مبالغها
        /// </summary>
        public int OrderMoneyDelived { get; set; }
        /// <summary>
        /// طرود في الطريق
        /// </summary>
        public int OrderInWat { get; set; }
        /// <summary>
        /// طرود في المخزن
        /// </summary>
        public int OrderInStore { get; set; }
        /// <summary>
        /// في إنتظار استلامها من الشركة 
        /// </summary>
        public int OrderWait { get; set; }
        
    }
}
