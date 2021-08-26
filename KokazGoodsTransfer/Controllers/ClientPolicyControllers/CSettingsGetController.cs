using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CSettingsGetController : AbstractClientPolicyController
    {
        public CSettingsGetController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet("Countries")]
        public IActionResult GetCountreis()
        {
            var countries = Context.Countries
                .Include(c=>c.Clients)
                .Include(c => c.Regions)
                .ToList();
            return Ok(mapper.Map<CountryDto[]>(countries));
        }
        [HttpGet("Regions")]
        public IActionResult GetRegions()
        {
            return Ok(mapper.Map<RegionDto[]>(Context.Regions.Include(c => c.Country)));
        }
        [HttpGet("orderType")]
        public IActionResult GetOrderType()
        {   
            var ordertypes = this.Context.OrderTypes.ToList();
            return Ok(mapper.Map<NameAndIdDto[]>(ordertypes));
        }
        [HttpGet("OrderPlaced")]
        public IActionResult GetOrderPalce()
        {
            return Ok(mapper.Map<NameAndIdDto[]>(this.Context.OrderPlaceds.ToList()));
        }
        [HttpGet("MoenyPlaced")]
        public IActionResult GetMoenyPlaced()
        {
            return Ok(mapper.Map<NameAndIdDto[]>(this.Context.MoenyPlaceds.ToList()));
        }

    }
}