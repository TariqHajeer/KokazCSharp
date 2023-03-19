using System.Threading.Tasks;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : AbstractEmployeePolicyController
    {
        private readonly IStatisticsService _statisticsService;
        private readonly INotificationService _notificationService;
        public StatisticsController(IStatisticsService statisticsService, INotificationService notificationService)
        {
            _statisticsService = statisticsService;
            _notificationService = notificationService;
        }

        [HttpGet("MainStatics")]
        public async Task<IActionResult> MainStatics()
        {
            return Ok(await _statisticsService.GetMainStatics());
        }
        [HttpGet("Notification")]
        public async Task<IActionResult> GetAdminNotification()
        {
            return Ok(await _notificationService.GetAdminNotification());
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