using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.PointSettingsDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointSettingsController : AbstractEmployeePolicyController
    {
        public PointSettingsController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet]
        public IActionResult Get()
        {
            var points= this.Context.PointsSettings.ToList();
            return Ok(mapper.Map<PointSettingsDto[]>(points));
        }
        [HttpPost]
        public IActionResult Create([FromBody]CreatePointSetting createPointSetting)
        {
            var isValid = IsPointValid(createPointSetting) as OkObjectResult;
            if (!(bool)isValid.Value)
            {
                return Conflict();
            }
            var point = mapper.Map<PointsSetting>(createPointSetting);
            this.Context.Add(point);
            this.Context.SaveChanges();
            return Ok(mapper.Map<PointSettingsDto>(point));
        }
        [HttpGet("GetSettingLessThanPoint/{points}")]
        public IActionResult GetByMoneyByPoint(int points)
        {
            var pointsSettings = this.Context.PointsSettings.Where(c => c.Points <= points);
            return Ok(mapper.Map<PointSettingsDto[]>(pointsSettings));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pointSetting= this.Context.PointsSettings.Find(id);
            this.Context.Remove(pointSetting);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpGet("IsPointValid")]
        public IActionResult IsPointValid([FromQuery]CreatePointSetting createPointSetting)
        {
            if (createPointSetting.Points == 0 || createPointSetting.Money == 0)
                return Ok(false);
            return Ok(!this.Context.PointsSettings.Where(c => c.Money == createPointSetting.Money || c.Points == createPointSetting.Points).Any());
        }
        
    }
}