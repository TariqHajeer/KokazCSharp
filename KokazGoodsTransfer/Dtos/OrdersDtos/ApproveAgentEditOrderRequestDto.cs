using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class ApproveAgentEditOrderRequestDto
    {
        public int Id { get; set; }
        public decimal NewAmount { get; set; }
        public virtual UserDto Agent { get; set; }
        public virtual OrderDto Order { get; set; }
        public virtual NameAndIdDto OrderPlaced { get; set; }
    }
}
