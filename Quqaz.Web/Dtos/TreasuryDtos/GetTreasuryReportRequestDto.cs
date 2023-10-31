using Quqaz.Web.Dtos.Common;
using System;

namespace Quqaz.Web.Dtos.TreasuryDtos
{
    public class GetTreasuryReportRequestDto
    {
        public DateRangeFilter DateRangeFilter { get; set; }
        public int TreasuryId { get; set; }
    }
}
