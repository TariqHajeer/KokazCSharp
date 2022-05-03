using AutoMapper;
using KokazGoodsTransfer.Dtos.TreasuryDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
    }
}
