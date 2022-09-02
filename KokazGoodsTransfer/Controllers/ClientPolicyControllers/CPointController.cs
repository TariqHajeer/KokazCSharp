using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.PointSettingsDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPointController : AbstractClientPolicyController
    {
        private readonly IPointSettingService _pointSettingService;
        private readonly IClientCashedService _clientCashedService;
        public CPointController(IPointSettingService pointSettingService, IClientCashedService clientCashedService)
        {
            _pointSettingService = pointSettingService;
            _clientCashedService = clientCashedService;
        }
        [HttpGet("MyPoints")]
        public async Task<IActionResult> MyPoints()
        {
            var client = await _clientCashedService.FirstOrDefualt(c => c.Id == AuthoticateUserId());
            return Ok(client.Points);
        }
        [HttpGet]
        public async Task< IActionResult> Get()
        {
            return Ok(await _pointSettingService.GetAll());
        }
    }
}