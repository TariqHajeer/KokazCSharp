using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeTypeController : AbstractEmployeePolicyController
    {



        private readonly IIncomeTypeSerive _incomeTypeSerive;
        public IncomeTypeController(IIncomeTypeSerive incomeTypeSerive, KokazContext context, IMapper mapper) : base(context, mapper)
        {
            _incomeTypeSerive = incomeTypeSerive;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var incomeTypes = await _incomeTypeSerive.GetAll(c => c.Incomes);
            return Ok(incomeTypes);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncomeTypeDto createIncomeTypeDto)
        {
            var response = await _incomeTypeSerive.AddAsync(createIncomeTypeDto);
            if (response.Errors.Any())
                return Conflict();
            return Ok(response.Data);
        }
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateIncomeTypeDto updateIncomeTypeDto)
        {
            var result = await _incomeTypeSerive.Update(updateIncomeTypeDto);
            if (result.Errors.Any())
            {
                return Conflict();
            }
            return Ok(result.Data);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _incomeTypeSerive.Delete(id);
            if (result.Sucess)
                return Ok(result.Data);
            if (result.NotFound)
                return NotFound();
            if (result.CantDelete)
                return Conflict();
            return Ok();

        }
    }
}