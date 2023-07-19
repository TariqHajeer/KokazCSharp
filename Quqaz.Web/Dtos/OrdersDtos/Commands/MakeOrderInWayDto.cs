using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Dtos.OrdersDtos.Commands
{
    public class MakeOrderInWayDto
    {
        public int[] Ids { get; set; }
        public DriverDto Driver { get; set; }
    }
}
