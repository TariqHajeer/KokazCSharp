using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class ClientPayment
    {
        public ClientPayment()
        {
            ClientPaymentDetails = new HashSet<ClientPaymentDetail>();
        }

        public int Id { get; set; }
        public string PrinterName { get; set; }
        public DateTime Date { get; set; }
        public string DestinationName { get; set; }
        public string DestinationPhone { get; set; }

        public virtual ICollection<ClientPaymentDetail> ClientPaymentDetails { get; set; }
    }
}
