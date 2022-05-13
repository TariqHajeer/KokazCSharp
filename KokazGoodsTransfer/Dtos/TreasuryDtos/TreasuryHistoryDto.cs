using System;

namespace KokazGoodsTransfer.Dtos.TreasuryDtos
{
    public class TreasuryHistoryDto
    {
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public int? ClientPaymentId { get; set; }
        public int? CashMovmentId { get; set; }
        public int? ReceiptId { get; set; }
        public int? ReceiptOfTheOrderStatusId { get; set; }
    }
}
