using System.Threading.Tasks;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Quqaz.Web.Dtos.Common;
using System.Collections.Generic;
using Quqaz.Web.Dtos.Statics;
using Quqaz.Web.Models.Static;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CStaticsController : AbstractClientPolicyController
    {
        private readonly INotificationService _notificationService;
        private readonly IStatisticsService _statisticsService;
        private readonly IOrderClientSerivce _orderClientSerivce;
        private readonly ICountryCashedService countryCashedService;
        public CStaticsController(INotificationService notificationService, IStatisticsService statisticsService, IOrderClientSerivce orderClientSerivce, ICountryCashedService countryCashedService)
        {
            _notificationService = notificationService;
            _statisticsService = statisticsService;
            _orderClientSerivce = orderClientSerivce;
            this.countryCashedService = countryCashedService;
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
            var result = await _orderClientSerivce.GetOrderStaticsReport(dateRangeFilter);
            if (result.HighestDeliveredCountryMapId != -1)
            {
                result.HighestDeliveredCountryMapId = Consts.CountryMap[result.HighestDeliveredCountryMapId];
            }
            if (result.HighestRequestedCountryMapId != -1)
            {
                result.HighestRequestedCountryMapId = Consts.CountryMap[result.HighestRequestedCountryMapId];
            }
            if (result.HighestReturnedCountryMapId != -1)
            {
                result.HighestReturnedCountryMapId = Consts.CountryMap[result.HighestReturnedCountryMapId];
            }
            return Ok(result);
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