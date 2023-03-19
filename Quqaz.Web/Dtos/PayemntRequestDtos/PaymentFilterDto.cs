using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.PayemntRequestDtos
{
    public class PaymentFilterDto
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public int? PaymentWayId { get; set; }
        public bool? Accept { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
