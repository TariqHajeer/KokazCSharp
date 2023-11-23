using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quqaz.Web.Dtos.MarketDtos;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Quqaz.Web.Dtos.HomeDto;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.Additional.ClientMessageDtos;
using Quqaz.Web.Services.Interfaces.Additional;
using Microsoft.EntityFrameworkCore;
using Quqaz.Web.Dtos.Additional.ExternalShipment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Quqaz.Web.Dtos.OrdersDtos;

namespace Quqaz.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : OldAbstractController
    {
        private readonly IIndexService<Country> _countrySerivce;
        private readonly IBranchService _branchService;
        private readonly IEmailService _emailService;
        private readonly IClientMessageService _clientMessageService;
        private readonly IWebHostEnvironment _environment;
        public HomeController(KokazContext context, IMapper mapper, IBranchService branchService, IEmailService emailService, IClientMessageService clientMessageService, IIndexService<Country> countrySerivce, IWebHostEnvironment environment) : base(context, mapper)
        {
            _branchService = branchService;
            _emailService = emailService;
            _clientMessageService = clientMessageService;
            _countrySerivce = countrySerivce;
            _environment = environment;
        }
        [HttpGet("Statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            return Ok(new StatisticsDto()
            {
                ClientCount = await _context.Clients.CountAsync(),
                Employee = await _context.Users.CountAsync(),
                Shipment = await _context.Orders.CountAsync(),
                ShipmentFromGlobal = await _context.ExternalShipments.CountAsync(),
                ContactCompanies = await _context.Markets.CountAsync()
            });
        }
        [HttpPost("RequestExternalShipment")]
        public async Task<IActionResult> RequestExternalShipment(CreateExternalShipment createExternalShipment)
        {
            var template = await System.IO.File.ReadAllTextAsync(Path.Combine(_environment.WebRootPath, "HtmlTemplate/HomeTemplate/externalShipment.html"));
            template = template.Replace("{{from}}", createExternalShipment.From);
            template = template.Replace("{{to}}", createExternalShipment.To);
            template = template.Replace("{{email}}", createExternalShipment.Phone);
            template = template.Replace("{{phone}}", createExternalShipment.Email);
            await _emailService.SendEmailAsHtml("ExternalShipment@quqaz.com", "postmaster@quqaz.com", "P@ssw0rd@123", "طلب شحنة خارجية", template);
            return Ok();
        }
        [HttpGet("GetShipmentTracking")]
        public async Task<ActionResult<ShipmentTrackingResponse>> GetShipmentTracking([FromQuery] ShipmentTrackingRequestDto shipmentTrackingRequest)
        {
            
            return Ok(shipmentTrackingRequest);
        }
        [HttpPost("ReserveMailBox")]
        public async Task<IActionResult> ReserveMailBox(ReserveMailBoxRequestDto reserveMailBoxRequest)
        {
            var template = await System.IO.File.ReadAllTextAsync(Path.Combine(_environment.WebRootPath, "HtmlTemplate/HomeTemplate/mailBox.html"));
            template = template.Replace("{{name}}", reserveMailBoxRequest.Name);
            template = template.Replace("{{email}}", reserveMailBoxRequest.Email);
            template = template.Replace("{{phone}}", reserveMailBoxRequest.Phone);
            await _emailService.SendEmailAsHtml("mailbox@quqaz.com", "postmaster@quqaz.com", "P@ssw0rd@123", "طلب صندوق بريدي", template);
            return Ok();
        }

        [HttpPost("CreateClientMessages"), DisableRequestSizeLimit]
        public async Task<IActionResult> CreateClientMessages([FromForm] string message, [FromForm] string name, IFormFile logo)
        {
            await _clientMessageService.AddAsync(new CreateClientMessageDto()
            {
                Message = message,
                Name = name,
                Logo = logo
            });
            return Ok();
        }

        [HttpGet("ClientMessages")]
        public async Task<IActionResult> ClientMessages([FromQuery] PagingDto paging)
        {
            return Ok(await _clientMessageService.GetAsync(paging: paging, filter: c => c.IsPublished == true));
        }
        [HttpPost("JoinToTeam"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadCv([FromForm] string fistNamem, [FromForm] string lastName, [FromForm] string phone, [FromForm] string email, IFormFile file)
        {
            var template = await System.IO.File.ReadAllTextAsync(Path.Combine(_environment.WebRootPath, "HtmlTemplate/HomeTemplate/JoinToTeam.html"));
            template = template.Replace("{{firstName}}", fistNamem);
            template = template.Replace("{{lastName}}", lastName);
            template = template.Replace("{{email}}", email);
            template = template.Replace("{{phone}}", phone);
            await _emailService.SendEmailAsHtml("hr@quqaz.com", "postmaster@quqaz.com", "P@ssw0rd@123", "طلب انضمام", template, file);
            return Ok();
        }
        [HttpGet("Branches")]
        public async Task<IActionResult> GetBranches()
        {
            return Ok(await _branchService.GetBranchPrices());
        }
        [HttpGet("Country")]
        public async Task<IActionResult> GetCountry() => Ok(await _countrySerivce.GetAllLite());
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