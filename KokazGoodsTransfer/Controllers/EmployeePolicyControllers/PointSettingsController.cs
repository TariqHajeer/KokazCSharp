using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.PointSettingsDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointSettingsController : AbstractEmployeePolicyController
    {
        private readonly IPointSettingService _pointSettingService;
        public PointSettingsController(IPointSettingService pointSettingService)
        {
            _pointSettingService = pointSettingService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _pointSettingService.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePointSetting createPointSetting)
        {
            var isValid = await IsPointValid(createPointSetting) as OkObjectResult;
            if (!(bool)isValid.Value)
            {
                return Conflict();
            }
            var result = await _pointSettingService.AddAsync(createPointSetting);
            return Ok(result.Data);
        }
        [HttpGet("GetSettingLessThanPoint/{points}")]
        public async Task<IActionResult> GetByMoneyByPoint(int points)
        {
            return Ok(await _pointSettingService.GetAsync(c => c.Points <= points));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pointSettingService.Delete(id);
            return Ok();
        }
        [HttpGet("IsPointValid")]
        public async Task<IActionResult> IsPointValid([FromQuery] CreatePointSetting createPointSetting)
        {
            if (createPointSetting.Points == 0 || createPointSetting.Money == 0)
                return Ok(false);
            return Ok(await _pointSettingService.Any(c => c.Money == createPointSetting.Money || c.Points == createPointSetting.Points));
        }

    }
}