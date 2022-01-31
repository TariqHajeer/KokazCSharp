using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : AbstractEmployeePolicyController
    {

        public RegionController(KokazContext context, IMapper mapper, Logging logging) : base(context, mapper, logging)
        {

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await Context.Regions.Include(c => c.Country).ToListAsync();
            return Ok(mapper.Map<RegionDto[]>(regions));
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
            this.Context.Entry(region).Reference(c => c.Country).Load();

            return Ok(mapper.Map<RegionDto>(region));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var region = this.Context.Regions.Find(id);
                if (region == null)
                    return NotFound();
                //if (region.Clients.Any())
                //{
                //    return Conflict();
                //}

                this.Context.Regions.Remove(region);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch]
        public IActionResult UpdateRegion([FromBody] UpdateRegion updateRegion)
        {
            var region = this.Context.Regions.Find(updateRegion.Id);
            if (this.Context.Regions.Where(c => c.CountryId == region.CountryId && c.Name == updateRegion.Name && c.Id != updateRegion.Id).Any())
            {
                return Conflict();
            }
            region.Name = updateRegion.Name;
            this.Context.Update(region);
            this.Context.SaveChanges();
            return Ok();
        }
    }
}