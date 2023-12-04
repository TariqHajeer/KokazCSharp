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
        [HttpGet("AccountReport")]
        public async Task<IActionResult> AccountReport([FromQuery] DateRangeFilter dateRangeFilter)
        {
            return Ok(new List<AccountReportDto>()
            {
                new AccountReportDto()
                {
                    Text ="100 دع",
                    Title ="تستديد"
                },
                new AccountReportDto()
                {
                    Text ="100 دع",
                    Title ="تستديد"
                }

            });
        }
        [HttpGet("GetPhoneOrderStatusCount")]
        public async Task<IActionResult> GetPhoneOrderStatusCount([FromQuery] string phone)
        {
            var list = new List<AccountReportDto>
            {
                new AccountReportDto()
                {
                    Text = "10",
                    Title="مرتجع كلي"
                },
                new AccountReportDto()
                {
                    Text = "50",
                    Title="تم التسليم"
                },
                new AccountReportDto()
                {
                    Text = "20",
                    Title="مرتجع جزئي "
                },
            };
            return Ok(list);
        }

        [HttpGet("GetNo")]
        public async Task<IActionResult> GetNo()
        {
            await _notificationService.SendClientNotification();
            return Ok();
        }
    }
}