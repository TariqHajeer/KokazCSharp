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
        [HttpGet("GetCountries")]
        public IActionResult GetAll()
        {
            var countries = Context.Countries
                .Include(c => c.Users)
                .Include(c => c.Regions)
                    .ThenInclude(c => c.Clients)
                .ToList();
            return Ok(mapper.Map<CountryDto[]>(countries));
        }
        [HttpGet("GetRegions")]
        public IActionResult GetRegions()
        {
            return Ok(mapper.Map<RegionDto[]>(Context.Regions.Include(c => c.Country).Include(c => c.Clients)));
        }
        [HttpGet("orderType")]
        public IActionResult GetOrderType()
        {   
            var ordertypes = this.Context.OrderTypes.ToList();
            return Ok(mapper.Map<NameAndIdDto[]>(ordertypes));
        }

    }
}