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
        public decimal AgentTotal { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal TreasuryIncome { get; set; }
    }
}
