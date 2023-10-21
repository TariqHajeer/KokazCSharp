using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Models.Static;
using System;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class OrderFilter
    {
        public string Code { get; set; }
        public string Phone { get; set; }
        public int? CountryId { get; set; }
        public int? RegionId { get; set; }
        public int? ClientId { get; set; }
        public string RecipientName { get; set; }
        public MoneyPalce? MoneyPalced { get; set; }
        public OrderPlace? Orderplaced { get; set; }
        public int? AgentId { get; set; }
        public bool? IsClientDiliverdMoney { get; set; }
        public int? AgentPrintNumber { get; set; }
        public int? ClientPrintNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateRangeFilter CreatedDateRangeFilter { get; set; }
        public string Note { get; set; }
        public DateTime? AgentPrintStartDate { get; set; }
        public DateTime? AgentPrintEndDate { get; set; }
        public OrderState? OrderState { get; set; }
        public bool? HaveScoundBranch { get; set; }
        public int? OriginalBranchId { get; set; }
        public int? SecoundBranchId { get; set; }
        public int? CurrentBranchId { get; set; }
        public int? NextBranchId { get; set; }


    }
}
