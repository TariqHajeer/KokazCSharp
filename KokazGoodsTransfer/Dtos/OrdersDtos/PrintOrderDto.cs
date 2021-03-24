using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class PrintOrdersDto
    {
        public int PrintNmber { get; set; }
        public string PrinterName { get; set; }
        public DateTime Date { get; set; }
        public string DestinationName { get; set; }
        public string DestinationPhone { get; set; }
        public List<PrintDto> Orders { get; set; }

    }
    public abstract class PrintDto
    {
        public string Code { get; set; }
        public decimal Total { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
    }
    public class AgentPrintDto : PrintDto
    {   
        public string ClientName { get; set; }
        public string Note { get; set; }

    }
    public class ClientprintDto: PrintDto
    {
        public string LastTotal { get; set; }

        public decimal DeliveCost { get; set; }

    }
}
