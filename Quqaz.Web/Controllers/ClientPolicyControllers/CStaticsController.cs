using System.Threading.Tasks;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Quqaz.Web.Dtos.Common;
using System.Collections.Generic;
using Quqaz.Web.Dtos.Statics;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CStaticsController : AbstractClientPolicyController
    {
        private readonly INotificationService _notificationService;
        private readonly IStatisticsService _statisticsService;
        private readonly IOrderClientSerivce _orderClientSerivce;
        public CStaticsController(INotificationService notificationService, IStatisticsService statisticsService, IOrderClientSerivce orderClientSerivce)
        {
            _notificationService = notificationService;
            _statisticsService = statisticsService;
            _orderClientSerivce = orderClientSerivce;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _statisticsService.GetClientStatistic());
        }
        [HttpGet("AccountReport")]
        public async Task<IActionResult> AccountReport([FromQuery] DateRangeFilter dateRangeFilter)
        {
            return Ok(await _orderClientSerivce.AccountReport(dateRangeFilter));
        }
        [HttpGet("GetOrderStaticsReport")]
        public async Task<IActionResult> GetOrderStaticsReport([FromQuery] DateRangeFilter dateRangeFilter)
        {
            return Ok(await _orderClientSerivce.GetOrderStaticsReport(dateRangeFilter));
        }
        [HttpGet("GetPhoneOrderStatusCount")]
        public async Task<IActionResult> GetPhoneOrderStatusCount([FromQuery] string phone)
        {
            return Ok(await _orderClientSerivce.GetPhoneOrderStatusCount(phone));
        }

        [HttpGet("GetNo")]
        public async Task<IActionResult> GetNo()
        {
            await _notificationService.SendClientNotification();
            return Ok();
        }
    }
}