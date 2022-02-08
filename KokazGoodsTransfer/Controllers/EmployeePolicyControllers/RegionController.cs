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
        private readonly ICountryCashedService _countryCashedService;
        public RegionController(KokazContext context, IMapper mapper, Logging logging, IRegionCashedService regionCashedService, ICountryCashedService countryCashedService) : base(context, mapper, logging)
        {
            _regionCashedService = regionCashedService;
            _countryCashedService = countryCashedService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _regionCashedService.GetCashed());
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegionDto createRegionDto)
        {
            try
            {
                var result = await _regionCashedService.AddAsync(createRegionDto);
                await _countryCashedService.RefreshCash();
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
                await _countryCashedService.RefreshCash();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateRegion([FromBody] UpdateRegionDto updateRegion)
        {
            try
            {
                var result = await _regionCashedService.Update(updateRegion);
                if (result.Errors.Any())
                {
                    return Conflict();
                }
                await _countryCashedService.RefreshCash();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}