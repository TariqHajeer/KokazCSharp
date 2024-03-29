﻿using System.Linq;
using System.Threading.Tasks;
using Quqaz.Web.Dtos.OutComeTypeDtos;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutComeTypeController : AbstractEmployeePolicyController
    {
        private readonly IOutcomeTypeService _outcomeTypeService;
        public OutComeTypeController(IOutcomeTypeService outcomeTypeService)
        {
            _outcomeTypeService = outcomeTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetALl()
        {
            var result = await _outcomeTypeService.GetAll(c => c.OutComes);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOutComeTypeDto createOutComeTypeDto)
        {
            var response = await _outcomeTypeService.AddAsync(createOutComeTypeDto);
            if (response.Errors.Any())
                return Conflict();
            return Ok(response.Data);
        }
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateOutComeTypeDto updateOutComeTypeDto)
        {
            var result = await _outcomeTypeService.Update(updateOutComeTypeDto);
            if (result.Errors.Any())
            {
                return Conflict();
            }
            return Ok(result.Data);


        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _outcomeTypeService.Delete(id);
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