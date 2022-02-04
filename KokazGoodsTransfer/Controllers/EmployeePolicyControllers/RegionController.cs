using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
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

        private readonly ICashedRepository<Country> _cashedRepository;
        public RegionController(KokazContext context, IMapper mapper, Logging logging, ICashedRepository<Country> cashedRepository) : base(context, mapper, logging)
        {
            _cashedRepository = cashedRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var country = await _cashedRepository.GetAll(c => c.Regions, c => c.Clients, c => c.AgentCountrs);
            var regions = country.SelectMany(c => c.Regions.ToArray()).ToArray();
            return Ok(mapper.Map<RegionDto[]>(regions));
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegionDto createRegionDto)
        {

            var similerRegion = Context.Regions.Where(c => c.Name == createRegionDto.Name && c.CountryId == createRegionDto.CountryId).FirstOrDefault();
            if (similerRegion != null)
                return Conflict();
            var country = await _cashedRepository.GetById(createRegionDto.CountryId);
            var region = new Region()
            {
                Name = createRegionDto.Name
            };
            country.Regions.Add(region);
            await _cashedRepository.Update(country);
            return Ok(mapper.Map<RegionDto>(region));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var region = await this.Context.Regions.FindAsync(id);
                if (region == null)
                    return NotFound();
                var country = await _cashedRepository.GetById(region.CountryId);
                country.Regions = country.Regions.Where(c => c.Id != id).ToList();
                await _cashedRepository.Update(country);
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