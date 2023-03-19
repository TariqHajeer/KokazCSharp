using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class PaymentWay : IIndex, IHaveBranch
    {
        public PaymentWay()
        {
            PaymentRequests = new HashSet<PaymentRequest>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PaymentRequest> PaymentRequests { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
