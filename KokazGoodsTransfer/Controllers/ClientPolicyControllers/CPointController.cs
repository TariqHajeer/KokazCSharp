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

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPointController : AbstractClientPolicyController
    {
        public CPointController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet("MyPoints")]
        public IActionResult MyPoints()
        {
            var client = this._context.Clients.Find(AuthoticateUserId());
            return Ok(client.Points);
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_mapper.Map<PointSettingsDto[]>(this._context.PointsSettings.ToList()));
        }
    }
}