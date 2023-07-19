using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class PayForClientWithTotalDto
    {
        public decimal TotalOrdersCost { get; set; }
        public decimal TotalDeliveyCost { get; set; }
        public IEnumerable<PayForClientDto> PayForClientDtos { get; set; }
    }
}
