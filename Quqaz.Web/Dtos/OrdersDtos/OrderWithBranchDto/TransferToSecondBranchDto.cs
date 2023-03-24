namespace Quqaz.Web.Dtos.OrdersDtos.OrderWithBranchDto
{
    public class TransferToSecondBranchDto
    {
        public SelectedOrdersWithFitlerDto SelectedOrdersWithFitlerDto { get; set; }
        public int? DriverId { get; set; }
        public string DriverName { get; set; }
    }
}
