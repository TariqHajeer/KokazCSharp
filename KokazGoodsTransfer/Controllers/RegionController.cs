using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : AbstractController
    {
        
        public RegionController(KokazContext context ,IMapper mapper):base(context, mapper)
        {
            
        }
        [HttpGet]
        public IActionResult GetAll()
        {

            return Ok(mapper.Map<RegionDto[]>(Context.Regions.Include(c => c.Country).Include(c => c.Clients)));
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
            
            return Ok(mapper.Map<RegionDto>(region));
        }
    }
}