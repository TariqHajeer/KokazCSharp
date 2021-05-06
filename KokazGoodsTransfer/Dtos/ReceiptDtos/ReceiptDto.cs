using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.ReceiptDtos
{
    public class ReceiptDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public string About { get; set; }
        public string Manager { get; set; }
        public bool IsPay { get; set; }
        public int? PrintId { get; set; }
        public string ClientName { get; set; }
    }
}
