using System;

namespace KokazGoodsTransfer.Dtos.OrdersDtos.OrderWithBranchDto
{
    public class TransferToSecondBranchDetialsReportDto
    {
        public string Code { get; set; }
        public decimal Total { get; set; }
        public string CountryName { get; set; }
        public string ClientName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
    }
}
