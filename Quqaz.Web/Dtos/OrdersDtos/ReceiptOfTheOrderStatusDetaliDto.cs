using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.Users;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class ReceiptOfTheOrderStatusDetaliDto
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public decimal Cost { get; set; }
        public decimal? OldCost { get; set; }
        public decimal AgentCost { get; set; }  
        public NameAndIdDto Client { get; set; }
        public NameAndIdDto Agent { get; set; }
        public NameAndIdDto MoneyPlaced { get; set; }
        public NameAndIdDto OrderPlaced { get; set; }
        
    }
}
