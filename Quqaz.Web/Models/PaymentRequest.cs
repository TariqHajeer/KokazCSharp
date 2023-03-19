using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class PaymentRequest:IHaveBranch
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PaymentWayId { get; set; }
        public string Note { get; set; }
        public bool? Accept { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Client Client { get; set; }
        public virtual PaymentWay PaymentWay { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
