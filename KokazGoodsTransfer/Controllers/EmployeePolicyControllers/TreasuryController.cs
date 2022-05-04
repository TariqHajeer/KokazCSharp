using AutoMapper;
using KokazGoodsTransfer.Dtos.TreasuryDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Services.Helper;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreasuryController : AbstractEmployeePolicyController
    {
        private readonly ITreasuryService _treasuryService;
        public TreasuryController(KokazContext context, IMapper mapper, Logging logging, ITreasuryService treasuryService) : base(context, mapper, logging)
        {
            _treasuryService = treasuryService;
        }
        [HttpPost]
        public async Task<ActionResult<TreasuryDto>> CreateTreasury(CreateTreasuryDto dto)
        {
            var response = await _treasuryService.Create(dto);
            if (response.Sucess)
                return Ok(response.Data);
            return Conflict(response.Errors);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TreasuryDto>> Get(int id)
        {
            return Ok(await _treasuryService.GetById(id));
        }
        [HttpGet("Hisotry/{treasuryId:int}")]
        public async Task<ActionResult<PagingResualt<IEnumerator<TreasuryHistoryDto>>>> History(int treasuryId, [FromQuery] PagingDto pagingDto)
        {
            return Ok(await _treasuryService.GetTreasuryHistory(treasuryId, pagingDto));
        }
        [HttpPatch("GiveMoney")]
        public async Task<ActionResult<ErrorRepsonse<TreasuryHistoryDto>>> GiveMoney(int id, [FromBody] decimal amount)
        {
            var treasury = _treasuryService.GetById(id);
            if (treasury == null)
                return NotFound();
            var data = await _treasuryService.IncreaseAmount(id, amount);
            return Ok(data);
        }
        [HttpPatch("GetMoney")]
        public async Task<ActionResult<ErrorRepsonse<TreasuryHistoryDto>>> GetMoney(int id, [FromBody] decimal amount)
        {
            var treasury = _treasuryService.GetById(id);
            if (treasury == null)
                return NotFound();
            var data = await _treasuryService.DecreaseAmount(id, amount);
            return Ok(data);
        }
        [HttpPatch("DisActive")]
        public async Task<IActionResult> DisActive(int id)
        {
            await _treasuryService.DisActive(id);
            return Ok();
        }
        [HttpPatch("Active")]
        public async Task<IActionResult> Active(int id)
        {
            await _treasuryService.Active(id);
            return Ok();
        }
    }
}
