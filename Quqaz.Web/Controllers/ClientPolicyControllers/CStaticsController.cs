using System.Threading.Tasks;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CStaticsController : AbstractClientPolicyController
    {
        private readonly INotificationService _notificationService;
        private readonly IStatisticsService _statisticsService;
        public CStaticsController(INotificationService notificationService, IStatisticsService statisticsService)
        {
            _notificationService = notificationService;
            _statisticsService = statisticsService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _statisticsService.GetClientStatistic());
        }

        [HttpGet("GetNo")]
        public async Task<IActionResult> GetNo()
        {
            await _notificationService.SendClientNotification();
            return Ok();
        }
    }
}