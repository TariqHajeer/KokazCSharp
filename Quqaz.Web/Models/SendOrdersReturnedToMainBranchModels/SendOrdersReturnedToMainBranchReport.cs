using Quqaz.Web.Models.Infrastrcuter;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Quqaz.Web.Models.SendOrdersReturnedToMainBranchModels
{
    public class SendOrdersReturnedToMainBranchReport : IHaveBranch
    {
        public SendOrdersReturnedToMainBranchReport()
        {
            this.SendOrdersReturnedToMainBranchDetalis = new HashSet<SendOrdersReturnedToMainBranchDetali>();
        }
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int DriverId { get; set; }
        public Branch Branch { get; set; }
        public int MainBranchId { get; set; }
        [ForeignKey(nameof(MainBranchId))]
        public Branch MainBranch { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedBy { get; set; }
        public string PrinterName { get; set; }
        [ForeignKey(nameof(DriverId))]
        public Driver Driver { get; set; }
        public ICollection<SendOrdersReturnedToMainBranchDetali> SendOrdersReturnedToMainBranchDetalis { get; set; }

    }

}
