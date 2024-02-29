using System.Collections.Generic;
using System.Threading.Tasks;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.Countries;
using Quqaz.Web.Dtos.Regions;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CSettingsGetController : AbstractClientPolicyController
    {

        private readonly ICountryCashedService _countryCashedService;
        private readonly IRegionCashedService _regionCashedService;
        private readonly IOrderTypeCashService _orderTypeCashService;
        public CSettingsGetController(ICountryCashedService countryCashedService, IRegionCashedService regionCashedService, IOrderTypeCashService orderTypeCashService)
        {

            _countryCashedService = countryCashedService;
            _regionCashedService = regionCashedService;
            _orderTypeCashService = orderTypeCashService;
        }

        [HttpGet("Countries")]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountreis() => Ok(await _countryCashedService.GetCashed());

        /// <summary>
        /// مناطق
        /// </summary>
        /// <returns></returns>
        [HttpGet("Regions")]
        public async Task<ActionResult<IEnumerable<RegionDto>>> GetRegions() => Ok(await _regionCashedService.GetAll());

        [HttpGet("orderType")]
        public async Task<IActionResult> GetOrderType()
        {
            var orderTypes = await _orderTypeCashService.GetAll();
            return Ok(orderTypes);
        }
        [HttpGet("OrderPlaced")]
        public ActionResult<IEnumerable<NameAndIdDto>> GetOrderPalce() => Ok(Models.Static.Consts.OrderPlaceds);
        [HttpGet("MoenyPlaced")]
        public ActionResult<IEnumerable<NameAndIdDto>> GetMoenyPlaced() => Ok(Models.Static.Consts.MoneyPlaceds);

    }
}