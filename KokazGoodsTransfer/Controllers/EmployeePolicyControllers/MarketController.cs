using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.MarketDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : AbstractEmployeePolicyController
    {
        IWebHostEnvironment env;
        public MarketController(KokazContext context, IMapper mapper, IWebHostEnvironment env) : base(context, mapper)
        {
            this.env = env;
        }
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Create([FromForm] CreateMarketDto createMarket)
        {
            var transaction = this._context.Database.BeginTransaction();
            var marketUrl = createMarket.MarketUrl;
            if (marketUrl.Contains("https"))
            {
                marketUrl = marketUrl.Split("https://")[1];
            }
            else if (marketUrl.Contains("http"))
            {
                marketUrl = marketUrl.Split("http://")[1];
            }
            createMarket.MarketUrl = marketUrl;
            try
            {
                Market market = new Market()
                {
                    ClientId = createMarket.ClientId,
                    Description = createMarket.Description,
                    IsActive = createMarket.IsActive,
                    MarketUrl = createMarket.MarketUrl,
                    Name = createMarket.Name,
                };
                this._context.Add(market);
                this._context.SaveChanges();
                var fileName = createMarket.Logo.FileName.Split('.');
                var folderDir = Path.Combine(env.WebRootPath, "MarketLogo");
                if (!Directory.Exists(Path.Combine(env.WebRootPath, "MarketLogo")))
                {
                    Directory.CreateDirectory(Path.Combine(folderDir));
                }

                var path = Path.Combine(folderDir, market.Id.ToString() + "." + fileName[fileName.Length - 1]);
                var stream = new FileStream(path, FileMode.Create);
                createMarket.Logo.CopyToAsync(stream);
                market.LogoPath = market.Id.ToString() + "." + fileName[fileName.Length - 1];
                this._context.Update(market);
                this._context.SaveChanges();
                transaction.Commit();
                stream.Close();
                stream.Dispose();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Conflict(new { ex.Message, inner = ex.InnerException.Message });
            }
            return Ok();
        }
        [HttpGet]
        public IActionResult Get()
        {
            List<MarketDto> markets = new List<MarketDto>();
            foreach (var item in this._context.Markets.ToList())
            {
                var temp = new MarketDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    //IsActive = item.IsActive,
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
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var market = this._context.Markets.Find(id);
            this._context.Markets.Remove(market);
            this._context.SaveChanges();
            return Ok();
        }
    }
}