using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Helpers;
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
        public CSettingsGetController(KokazContext context, IMapper mapper, ICountryCashedService countryCashedService, IRegionCashedService regionCashedService, IIndexService<MoenyPlaced> moneyPlacedIndexService, IIndexService<OrderPlaced> orderPlacedIndexService) : base(context, mapper)
        {

            _countryCashedService = countryCashedService;
            _regionCashedService = regionCashedService;
            _orderPlacedIndexService = orderPlacedIndexService;
            _moneyPlacedIndexService = moneyPlacedIndexService;
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
        public IActionResult GetOrderType()
        {
            var ordertypes = this._context.OrderTypes.ToList();
            return Ok(_mapper.Map<NameAndIdDto[]>(ordertypes));
        }
        [HttpGet("OrderPlaced")]
        public async Task<ActionResult<IEnumerable<NameAndIdDto>>> GetOrderPalce() => Ok(await _orderPlacedIndexService.GetAllLite());
        [HttpGet("MoenyPlaced")]
        public async Task<ActionResult<IEnumerable<NameAndIdDto>>> GetMoenyPlaced() => Ok(await _moneyPlacedIndexService.GetAllLite());

    }
}