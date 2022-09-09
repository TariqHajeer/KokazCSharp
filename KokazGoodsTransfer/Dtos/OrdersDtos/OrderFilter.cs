using KokazGoodsTransfer.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class OrderFilter
    {
        public string Code { get; set; }
        public string Phone { get; set; }
        public int? CountryId { get; set; }
        public int? RegionId { get; set; }
        public int? ClientId { get; set; }
        public string RecipientName { get; set; }
        public int? MonePlacedId { get; set; }
        public int? OrderplacedId { get; set; }
        public int? AgentId { get; set; }
        public bool? IsClientDiliverdMoney { get; set; }
        public int? AgentPrintNumber { get; set; }
        public int? ClientPrintNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateRangeFilter CreatedDateRangeFilter { get; set; }
        public string Note { get; set; }
        public DateTime? AgentPrintStartDate { get; set; }
        public DateTime? AgentPrintEndDate { get; set; }

    }
}
