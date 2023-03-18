using System.Collections.Generic;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CSettingsGetController : AbstractClientPolicyController
    {

        private readonly IIndexService<MoenyPlaced> _moneyPlacedIndexService;
        private readonly IIndexService<OrderPlaced> _orderPlacedIndexService;
        private readonly ICountryCashedService _countryCashedService;
        private readonly IRegionCashedService _regionCashedService;
        private readonly IOrderTypeCashService _orderTypeCashService;
        public CSettingsGetController(ICountryCashedService countryCashedService, IRegionCashedService regionCashedService, IIndexService<MoenyPlaced> moneyPlacedIndexService, IIndexService<OrderPlaced> orderPlacedIndexService, IOrderTypeCashService orderTypeCashService)
        {

            _countryCashedService = countryCashedService;
            _regionCashedService = regionCashedService;
            _orderPlacedIndexService = orderPlacedIndexService;
            _moneyPlacedIndexService = moneyPlacedIndexService;
            _orderTypeCashService = orderTypeCashService;
        }

        [HttpGet("Countries")]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountreis() => Ok(await _countryCashedService.GetAll());

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
        public async Task<ActionResult<IEnumerable<NameAndIdDto>>> GetOrderPalce() => Ok(await _orderPlacedIndexService.GetAllLite());
        [HttpGet("MoenyPlaced")]
        public async Task<ActionResult<IEnumerable<NameAndIdDto>>> GetMoenyPlaced() => Ok(await _moneyPlacedIndexService.GetAllLite());

    }
}