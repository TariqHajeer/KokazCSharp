using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : AbstractEmployeePolicyController
    {

        private readonly IRegionCashedService _regionCashedService;
        public RegionController(KokazContext context, IMapper mapper, Logging logging, IRegionCashedService regionCashedService) : base(context, mapper, logging)
        {
            _regionCashedService = regionCashedService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _regionCashedService.GetCashed());
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegionDto createRegionDto)
        {
            try
            {
                var result = await _regionCashedService.AddAsync(createRegionDto);
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result=  await _regionCashedService.Delete(id);
                if (result.Errors.Any())
                    return Conflict();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpPatch]
        //public async Task<IActionResult> UpdateRegion([FromBody] UpdateRegionDto updateRegion)
        //{
        //    var region = this._context.Regions.Find(updateRegion.Id);
        //    if (this._context.Regions.Where(c => c.CountryId == region.CountryId && c.Name == updateRegion.Name && c.Id != updateRegion.Id).Any())
        //    {
        //        return Conflict();
        //    }
        //    region.Name = updateRegion.Name;

        //    _context.Update(region);
        //    await _context.SaveChangesAsync();
        //    await _countryCashedRepository.RefreshCash();
        //    return Ok();
        //}
    }
}