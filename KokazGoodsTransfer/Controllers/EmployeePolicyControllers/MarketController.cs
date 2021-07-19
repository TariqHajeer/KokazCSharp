using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.MarketDtos;
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
            var transaction = this.Context.Database.BeginTransaction();
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
                this.Context.Add(market);
                this.Context.SaveChanges();
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
                this.Context.Update(market);
                this.Context.SaveChanges();
                transaction.Commit();
                stream.Close();
                stream.Dispose();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Conflict();
            }
            return Ok();
        }
        [HttpGet]
        public IActionResult Get()
        {
            List<MarketDto> markets = new List<MarketDto>();
            foreach (var item in this.Context.Markets.ToList())
            {
                markets.Add(new MarketDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    ClientId = item.ClientId,
                    Description = item.Description,
                    IsActive = item.IsActive,
                    MarketUrl = item.MarketUrl,
                    LogoPath = "MarketLogo/" + item.LogoPath
                });
            }
            return Ok(markets);
        }
    }
}