using System;
using System.Collections.Generic;
namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class ReceiptOfTheOrderStatusDto
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ReciverName { get; set; }
        public IEnumerable<ReceiptOfTheOrderStatusDetaliDto> ReceiptOfTheOrderStatusDetalis { get; set; }
    }
}
