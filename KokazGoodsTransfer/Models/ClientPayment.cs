using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class ClientPayment : IHaveBranch
    {
        public ClientPayment()
        {
            ClientPaymentDetails = new HashSet<ClientPaymentDetail>();
            Discounts = new HashSet<Discount>();
            OrderClientPaymnets = new HashSet<OrderClientPaymnet>();
            Receipts = new HashSet<Receipt>();
            TreasuryHistories = new HashSet<TreasuryHistory>();
        }

        public int Id { get; set; }
        public string PrinterName { get; set; }
        public DateTime Date { get; set; }
        public string DestinationName { get; set; }
        public string DestinationPhone { get; set; }

        public virtual ICollection<ClientPaymentDetail> ClientPaymentDetails { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
        public virtual ICollection<OrderClientPaymnet> OrderClientPaymnets { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
        public virtual ICollection<TreasuryHistory> TreasuryHistories { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
