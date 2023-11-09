namespace Quqaz.Web.Dtos.TreasuryDtos
{
    public class TreasuryReportResponseDto
    {
        public TreasuryReportResponseDto()
        {

        }
        public TreasuryReportItemResponseDto ClientPayment { get; set; }
        public TreasuryReportItemResponseDto Income { get; set; }
        public TreasuryReportItemResponseDto OutCome { get; set; }
        public TreasuryReportItemResponseDto Take { get; set; }
        public TreasuryReportItemResponseDto Give { get; set; }
        public TreasuryReportItemResponseDto PayReceipt { get; set; }
        public TreasuryReportItemResponseDto GetReceipt { get; set; }
        public TreasuryReportItemResponseDto ReciveOrder { get; set; }
    }
    public class TreasuryReportItemResponseDto
    {
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }
}
