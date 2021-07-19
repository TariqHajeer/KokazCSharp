using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.MarketDtos
{
    public class CreateMarketDto
    {
        public string Name { get; set; }
        public string MarketUrl { get; set; }
        public IFormFile Logo { get; set; }
        public string Description { get; set; }
        public int? ClientId { get; set; }
        public bool IsActive { get; set; }
    }
}
