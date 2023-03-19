using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.PayemntRequestDtos
{
    public class CreatePaymentRequestDto
    {
        public int PaymentWayId { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
    }
}
