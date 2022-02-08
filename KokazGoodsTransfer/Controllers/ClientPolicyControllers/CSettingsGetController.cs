using System;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CSettingsGetController : AbstractClientPolicyController
    {
        private readonly IIndexRepository<MoenyPlaced> _indexMoneyPlacedRepository;
        private readonly IIndexRepository<OrderPlaced> _indexOrderPlacedRepository;
        private readonly ICashedRepository<Country> _countryCashedRepository;
        public CSettingsGetController(KokazContext context, IMapper mapper, Logging logging, IIndexRepository<MoenyPlaced> indexMoneyPlacedRepository, IIndexRepository<OrderPlaced> indexOrderPlacedRepository, ICashedRepository<Country> countryCashedRepository) : base(context, mapper, logging)
        {
            _indexMoneyPlacedRepository = indexMoneyPlacedRepository;
            _indexOrderPlacedRepository = indexOrderPlacedRepository;
            _countryCashedRepository = countryCashedRepository;
        }

        [HttpGet("Countries")]
        public async Task<IActionResult> GetCountreis()
        {
            var countries = await _countryCashedRepository.GetAll();
            return Ok(_mapper.Map<CountryDto[]>(countries));
        }
        /// <summary>
        /// مناطق
        /// </summary>
        /// <returns></returns>
        [HttpGet("Regions")]
        public async Task<IActionResult> GetRegions()
        {
            var region = (await _countryCashedRepository.GetAll()).SelectMany(c => c.Regions).ToArray();
            return Ok(_mapper.Map<RegionDto[]>(region));
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