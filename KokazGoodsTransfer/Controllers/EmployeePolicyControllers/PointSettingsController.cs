using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.PointSettingsDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointSettingsController : AbstractEmployeePolicyController
    {
        public PointSettingsController(KokazContext context, IMapper mapper, Logging logging) : base(context, mapper, logging)
        {
        }
        [HttpGet]
        public IActionResult Get()
        {
            var points= this._context.PointsSettings.ToList();
            return Ok(_mapper.Map<PointSettingsDto[]>(points));
        }
        [HttpPost]
        public IActionResult Create([FromBody]CreatePointSetting createPointSetting)
        {
            var isValid = IsPointValid(createPointSetting) as OkObjectResult;
            if (!(bool)isValid.Value)
            {
                return Conflict();
            }
            var point = _mapper.Map<PointsSetting>(createPointSetting);
            this._context.Add(point);
            this._context.SaveChanges();
            return Ok(_mapper.Map<PointSettingsDto>(point));
        }
        [HttpGet("GetSettingLessThanPoint/{points}")]
        public IActionResult GetByMoneyByPoint(int points)
        {
            var pointsSettings = this._context.PointsSettings.Where(c => c.Points <= points);
            return Ok(_mapper.Map<PointSettingsDto[]>(pointsSettings));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pointSetting= this._context.PointsSettings.Find(id);
            this._context.Remove(pointSetting);
            this._context.SaveChanges();
            return Ok();
        }
        [HttpGet("IsPointValid")]
        public IActionResult IsPointValid([FromQuery]CreatePointSetting createPointSetting)
        {
            if (createPointSetting.Points == 0 || createPointSetting.Money == 0)
                return Ok(false);
            return Ok(!this._context.PointsSettings.Where(c => c.Money == createPointSetting.Money || c.Points == createPointSetting.Points).Any());
        }
        
    }
}