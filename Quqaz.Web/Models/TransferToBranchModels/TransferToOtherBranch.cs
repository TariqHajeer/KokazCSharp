using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Quqaz.Web.Models.TransferToBranchModels
{
    public class TransferToOtherBranch
    {
        public TransferToOtherBranch()
        {
            CreatedOnUtc = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public int SourceBranchId { get; set; }
        public int DestinationBranchId { get; set; }
        public string DriverName { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string PrinterName { get; set; }

        [ForeignKey(nameof(SourceBranchId))]
        public virtual Branch SourceBranch { get; set; }
        [ForeignKey(nameof(DestinationBranchId))]
        public virtual Branch DestinationBranch { get; set; }
        public virtual List<TransferToOtherBranchDetials> TransferToOtherBranchDetials { get; set; }
    }
}
