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
        private readonly IIndexRepository<MoenyPlaced> _indexMoneyPlacedRepository;
        private readonly IIndexRepository<OrderPlaced> _indexOrderPlacedRepository;
        private readonly ICountryCashedService _countryCashedService;
        private readonly IRegionCashedService _regionCashedService;
        public CSettingsGetController(KokazContext context, IMapper mapper, Logging logging, IIndexRepository<MoenyPlaced> indexMoneyPlacedRepository, IIndexRepository<OrderPlaced> indexOrderPlacedRepository, ICountryCashedService countryCashedService, IRegionCashedService regionCashedService) : base(context, mapper, logging)
        {
            _indexMoneyPlacedRepository = indexMoneyPlacedRepository;
            _indexOrderPlacedRepository = indexOrderPlacedRepository;
            _countryCashedService = countryCashedService;
            _regionCashedService = regionCashedService;
        }

        [HttpGet("Countries")]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountreis()
        {
            var countries = await _countryCashedService.GetAll();
            return Ok(countries);
        }
        /// <summary>
        /// مناطق
        /// </summary>
        /// <returns></returns>
        [HttpGet("Regions")]
        public async Task<ActionResult<IEnumerable<RegionDto>>> GetRegions()
        {
            var regions = await _regionCashedService.GetAll();
            return Ok(regions);
        }
        [HttpGet("orderType")]
        public IActionResult GetOrderType()
        {
            var ordertypes = this._context.OrderTypes.ToList();
            return Ok(_mapper.Map<NameAndIdDto[]>(ordertypes));
        }
        [HttpGet("OrderPlaced")]
        public async Task<IActionResult> GetOrderPalce()
        {
            var orderPlaceds = await _indexOrderPlacedRepository.GetLiteList();
            return Ok(_mapper.Map<NameAndIdDto[]>(orderPlaceds));
        }
        [HttpGet("MoenyPlaced")]
        public async Task<IActionResult> GetMoenyPlaced()
        {
            var moneyPlaceds = await _indexMoneyPlacedRepository.GetLiteList();
            return Ok(_mapper.Map<NameAndIdDto[]>(moneyPlaceds));
        }

    }
}