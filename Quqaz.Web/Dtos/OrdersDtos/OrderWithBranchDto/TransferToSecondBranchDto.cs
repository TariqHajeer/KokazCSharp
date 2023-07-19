using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Dtos.OrdersDtos.OrderWithBranchDto
{
    public class TransferToSecondBranchDto
    {
        public SelectedOrdersWithFitlerDto SelectedOrdersWithFitlerDto { get; set; }
        public DriverDto Driver { get; set; }
    }
}
