using KokazGoodsTransfer.Dtos.Common;
using System;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class ReceiptOfTheOrderStatusDetaliOrderDto
    {
        public NameAndIdDto MoneyPlaced { get; set; }
        public NameAndIdDto OrderPlaced { get; set; }
        public string Reciver { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ReceiptOfTheOrderStatusId { get; set; }
    }
}
