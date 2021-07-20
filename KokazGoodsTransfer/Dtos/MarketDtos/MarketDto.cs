using KokazGoodsTransfer.Dtos.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.MarketDtos
{
    public class MarketDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MarketUrl { get; set; }
        public string LogoPath { get; set; }
        public string Description { get; set; }
        public ClientDto Client { get; set; }
        public bool IsActive { get; set; }
    }
}
