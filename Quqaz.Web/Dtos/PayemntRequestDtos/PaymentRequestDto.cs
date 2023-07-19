using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.PayemntRequestDtos
{
    public class PaymentRequestDto
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public bool? Accept { get; set; }
        public DateTime CreateDate { get; set; }
        public ClientDto Client { get; set; }
        public NameAndIdDto PaymentWay { get; set; }
    }
}
