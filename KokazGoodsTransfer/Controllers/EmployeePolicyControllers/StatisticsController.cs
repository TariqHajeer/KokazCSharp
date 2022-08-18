using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : OldAbstractEmployeePolicyController
    {
        private readonly IStatisticsService _statisticsService;
        public StatisticsController(KokazContext context, IMapper mapper, IStatisticsService statisticsService) : base(context, mapper)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("MainStatics")]
        public async Task<IActionResult> MainStatics()
        {
            return Ok(await _statisticsService.GetMainStatics());
        }
        [HttpGet("Notification")]
        public async Task<IActionResult> GetAdminNotification()
        {
            return Ok(await _statisticsService.GetAdminNotification());
        }
        [HttpGet("GetAggregate")]
        public async Task<IActionResult> GetAggregate([FromQuery] DateFiter dateFiter)
        {
            return Ok(await _statisticsService.GetAggregate(dateFiter));
        }
        [HttpGet("AgnetStatics")]
        public async Task<IActionResult> AgnetStatics()
        {
            return Ok(await _statisticsService.AgnetStatics());
        }

        [HttpGet("ClientBalance")]
        public async Task<IActionResult> ClientBalance()
        {
            return Ok(await _statisticsService.GetClientBalance());
        }

    }
}