using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quqaz.Web.Dtos.MarketDtos;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : OldAbstractController
    {
        private readonly ICountryCashedService _countryCashedService;
        public HomeController(KokazContext context, IMapper mapper, ICountryCashedService countryCashedService) : base(context, mapper)
        {
            _countryCashedService = countryCashedService;
        }
        [HttpGet("Country")]
        public async Task<IActionResult> GetCountry() => Ok(await _countryCashedService.GetCashed());
        [HttpGet("Market")]
        public IActionResult GetMarket()
        {
            List<MarketDto> markets = new List<MarketDto>();
            foreach (var item in this._context.Markets.Where(c => c.IsActive == true).ToList())
            {
                var temp = new MarketDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    IsActive = (bool)item.IsActive,
                    MarketUrl = item.MarketUrl,
                    LogoPath = "MarketLogo/" + item.LogoPath
                };
                if (item.ClientId != null)
                {
                    var client = this._context.Clients.Find(item.ClientId);
                    temp.Client = new Dtos.Clients.ClientDto()
                    {
                        Name = client.Name,
                        Id = client.Id
                    };
                }
                markets.Add(temp);

            }
            return Ok(markets);
        }
    }
}