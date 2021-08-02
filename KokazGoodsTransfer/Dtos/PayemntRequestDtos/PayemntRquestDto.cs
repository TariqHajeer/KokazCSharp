using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.PayemntRequestDtos
{
    public class PayemntRquestDto
    {
        public int Id { get; set; }
        //public int ClientId { get; set; }
        //public int PaymentWayId { get; set; }
        public string Note { get; set; }
        public bool? Accept { get; set; }
        public ClientDto Client { get; set; }
        public NameAndIdDto PaymentWay { get; set; }
    }
}
