using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Users;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class ReceiptOfTheOrderStatusDetaliDto
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public decimal Cost { get; set; }
        public decimal AgentCost { get; set; }  
        public NameAndIdDto Client { get; set; }
        public NameAndIdDto Agent { get; set; }
        public NameAndIdDto MoneyPlaced { get; set; }
        public NameAndIdDto OrderPlaced { get; set; }
        
    }
}
