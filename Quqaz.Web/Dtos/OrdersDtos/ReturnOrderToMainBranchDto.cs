using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class ReturnOrderToMainBranchDto : SelectedOrdersWithFitlerDto
    {
        public DriverDto Driver { get; set; }
    }
}
