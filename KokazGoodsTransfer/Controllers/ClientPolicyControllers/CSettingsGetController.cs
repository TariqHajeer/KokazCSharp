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
        public CSettingsGetController(KokazContext context, IMapper mapper, Logging logging, IIndexRepository<MoenyPlaced> indexMoneyPlacedRepository, IIndexRepository<OrderPlaced> indexOrderPlacedRepository) : base(context, mapper, logging)
        {
            _indexMoneyPlacedRepository = indexMoneyPlacedRepository;
            _indexOrderPlacedRepository = indexOrderPlacedRepository;
        }
        
        [HttpGet("Countries")]
        public IActionResult GetCountreis()
        {
            var countries = _context.Countries
                .Include(c => c.Clients)
                .Include(c => c.Regions)
                .ToList();
            return Ok(_mapper.Map<CountryDto[]>(countries));
        }
        /// <summary>
        /// مناطق
        /// </summary>
        /// <returns></returns>
        [HttpGet("Regions")]
        public IActionResult GetRegions()
        {
            return Ok(_mapper.Map<RegionDto[]>(_context.Regions.Include(c => c.Country)));
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