using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Dtos.OrdersDtos.Queries.AgentQueries
{
    public class OrderInWayFilterDto
    {
        public string Code { get; set; }
        public DateRangeFilter DateRange { get; set; }
        public string RecipientPhones { get; set; }
    }
}
