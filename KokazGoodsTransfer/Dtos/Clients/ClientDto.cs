using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime FirstDate { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public CountryDto Country { get; set; }
        public string CreatedBy { get; set; }
        public bool CanDelete { get; set; } = true;
        public decimal Total { get; set; }
        public List<PhoneDto> Phones { get; set; }
        public string Mail { get; set; }
        public int Points { get; set; }
    }
    
}
