using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class DisAcceptOrder:IHaveBranch
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ClientId { get; set; }
        public int CountryId { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Cost { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhones { get; set; }
        public int? RegionId { get; set; }
        public string Address { get; set; }
        public string ClientNote { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Date { get; set; }
        public bool IsDollar { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Client Client { get; set; }
        public virtual Country Country { get; set; }
        public virtual Region Region { get; set; }
        public int BranchId { get ; set ; }
        public Branch Branch { get; set; }
    }
}
