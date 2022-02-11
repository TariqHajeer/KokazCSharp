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

        private readonly ICountryCashedRepository _countryCashedRepository;
        public RegionController(KokazContext context, IMapper mapper, Logging logging, ICountryCashedRepository countryCashedRepository) : base(context, mapper, logging)
        {
            _countryCashedRepository = countryCashedRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var country = await _countryCashedRepository.GetAll();
            var regions = country.SelectMany(c => c.Regions.ToArray()).ToArray();
            return Ok(_mapper.Map<RegionDto[]>(regions));
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegionDto createRegionDto)
        {
            var country = await _countryCashedRepository.GetById(createRegionDto.CountryId);
            var similerRegion = country.Regions.Where(c => c.Name == createRegionDto.Name).FirstOrDefault();
            if (similerRegion != null)
                return Conflict();
            var region = new Region()
            {
                Name = createRegionDto.Name
            };
            country.Regions.Add(region);
            _context.Regions.Attach(region);
            _context.Entry(region).State = EntityState.Added;
            await _countryCashedRepository.Update(country);
            return Ok(_mapper.Map<RegionDto>(region));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var region = await this._context.Regions.FindAsync(id);
                if (region == null)
                    return NotFound();
                _context.Remove(region);
                _context.SaveChanges();
                await _countryCashedRepository.RefreshCash();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateRegion([FromBody] UpdateRegion updateRegion)
        {
            var region = this._context.Regions.Find(updateRegion.Id);
            if (this._context.Regions.Where(c => c.CountryId == region.CountryId && c.Name == updateRegion.Name && c.Id != updateRegion.Id).Any())
            {
                return Conflict();
            }
            region.Name = updateRegion.Name;

            _context.Update(region);
            await _context.SaveChangesAsync();
            await _countryCashedRepository.RefreshCash();
            return Ok();
        }
    }
}