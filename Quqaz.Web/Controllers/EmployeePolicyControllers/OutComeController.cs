using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OutComeDtos;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutComeController : AbstractEmployeePolicyController
    {
        private readonly IOutcomeService _outcomeService;
        public OutComeController(IOutcomeService outcomeService)
        {
            _outcomeService = outcomeService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Filtering filtering, [FromQuery] PagingDto pagingDto)
        {
            return Ok(await _outcomeService.GetAsync(filtering, pagingDto));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _outcomeService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<OutComeDto>> Create([FromBody] CreateOutComeDto createOutComeDto)
        {

            var result = await _outcomeService.AddAsync(createOutComeDto);
            if (result.Sucess)
                return Ok(result.Data);
            return Conflict();

        }
        [HttpPost("CreateMulitpleOutCome")]
        public async Task<IActionResult> CreateMultiple([FromBody] IList<CreateOutComeDto> createOutComeDtos)
        {

            await _outcomeService.AddRangeAsync(createOutComeDtos);
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _outcomeService.Delete(id);
            return Ok();
        }
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateOuteComeDto dto)
        {
            var data = await _outcomeService.Update(dto);
            return Ok(data.Data);
        }
    }
}