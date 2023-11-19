using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quqaz.Web.Dtos.MarketDtos;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Quqaz.Web.Dtos.HomeDto;

namespace Quqaz.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : OldAbstractController
    {
        private readonly ICountryCashedService _countryCashedService;
        private readonly IBranchService _branchService;
        public HomeController(KokazContext context, IMapper mapper, ICountryCashedService countryCashedService, IBranchService branchService) : base(context, mapper)
        {
            _countryCashedService = countryCashedService;
            _branchService = branchService;
        }
        [HttpGet("Statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            return Ok(new StatisticsDto()
            {
                ClientCount= 10,
                Employee =120,
                Shipment=500,
                ShipmentFromGlobal = 10,
                ContactCompanies =20
            });
        }
        [HttpPost("RequestShipment")]
        public async Task<IActionResult> RequestShipment(RequestShipmentDto requestShipment)
        {
            return Ok(requestShipment);
        }
        [HttpGet("GetShipmentTracking")]
        public async Task<IActionResult> GetShipmentTracking(ShipmentTrackingRequestDto shipmentTrackingRequest)
        {
            return Ok(shipmentTrackingRequest);
        }
        [HttpPost("ReserveMailBox")]
        public async Task<IActionResult> ReserveMailBox(ReserveMailBoxRequestDto reserveMailBoxRequest)
        {
            return Ok(reserveMailBoxRequest);
        }
        [HttpGet("ClientMessages")]
        public async Task<IActionResult> ClientMessages()
        {
            return Ok(new List<ClientMessageReponse>());
        }
        [HttpGet("Branches")]
        public async Task<IActionResult> GetBranches()
        {
            return Ok(await _branchService.GetBranchPrices());
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