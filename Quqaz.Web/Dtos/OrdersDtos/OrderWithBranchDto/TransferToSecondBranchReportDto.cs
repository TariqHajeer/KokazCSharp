using System;

namespace Quqaz.Web.Dtos.OrdersDtos.OrderWithBranchDto
{
    public class TransferToSecondBranchReportDto
    {
        public int Id { get; set; }
        public string DestinationBranch { get; set; }
        public string DriverName { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string PrinterName { get; set; }
    }
}
