using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        KokazContext Context;
        public RegionController(KokazContext context )
        {
            this.Context = context;
        }
        [HttpPost]
        public IActionResult Create(CreateRegionDto createRegionDto)
        {

            var similerRegion = Context.Regions.Where(c => c.Name == createRegionDto.Name && c.CountryId == createRegionDto.CountryId).FirstOrDefault();
            if (similerRegion != null)
                return Conflict();

            Region region = new Region()
            {
                CountryId = createRegionDto.CountryId,
                Name = createRegionDto.Name
            };
            Context.Add(region);
            Context.SaveChanges();
            RegionDto response = new RegionDto()
            {
                Id = region.Id,
                Name = region.Name,
                CanDelete = true
            };
            return Ok(response);
        }
    }
}