using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.MarketDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : AbstractEmployeePolicyController
    {
        public MarketController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpPost]
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
                var path = Path.Combine(Directory.GetCurrentDirectory(), "MarketLogo", market.Id.ToString() + "." + fileName[fileName.Length - 1]);
                var stream = new FileStream(path, FileMode.Create);
                createMarket.Logo.CopyToAsync(stream);
                market.LogoPath = market.Id.ToString() + "." + fileName[fileName.Length - 1];
                this.Context.Update(market);
                this.Context.SaveChanges();
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                return Conflict();
            }
            return Ok();
        }
    }
}