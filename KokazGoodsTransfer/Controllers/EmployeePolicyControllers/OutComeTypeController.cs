using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OutComeTypeDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutComeTypeController : AbstractEmployeePolicyController
    {
        public OutComeTypeController(KokazContext context, IMapper mapper, Logging logging) : base(context, mapper, logging)
        {
        }

        [HttpGet]
        public IActionResult GetALl()
        {
            var outComeTypes = this._context.OutComeTypes
                .Include(c => c.OutComes)
                .ToList();
            return Ok(_mapper.Map<OutComeTypeDto[]>(outComeTypes));

        }
        [HttpPost]
        public IActionResult Create([FromBody]CreateOutComeTypeDto createOutComeTypeDto)
        {
            var similer = this._context.OutComeTypes.Where(c => c.Name == createOutComeTypeDto.Name).Count();
            if (similer > 0)
                return Conflict();
            if (createOutComeTypeDto.Name == "")
                return Conflict();

            OutComeType outComeType = new OutComeType()
            {
                Name = createOutComeTypeDto.Name
            };
            this._context.Add(outComeType);
            this._context.SaveChanges();
            OutComeTypeDto outeComeTypeDto = new OutComeTypeDto()
            {
                Id = outComeType.Id,
                Name = outComeType.Name,
                CanDelete = true
            };
            return Ok(outeComeTypeDto);
        }
        [HttpPatch]
        public IActionResult Update([FromBody] UpdateOutComeTypeDto updateOutComeTypeDto)
        {
            try
            {
                var outComeType = this._context.OutComeTypes.Find(updateOutComeTypeDto.Id);
                if (outComeType == null)
                    return NotFound();
                
                if (_context.OutComeTypes.Where(c=>c.Name==updateOutComeTypeDto.Name&&c.Id!=updateOutComeTypeDto.Id).Any())
                    return Conflict();
                outComeType.Name = updateOutComeTypeDto.Name;
                this._context.Update(outComeType);
                this._context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
                
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var outComeType = this._context.OutComeTypes.Find(id);
                if (outComeType == null)
                    return NotFound();
                this._context.Entry(outComeType).Collection(c => c.OutComes).Load();
                if (outComeType.OutComes.Any())
                    return Conflict();
                this._context.Remove(outComeType);
                this._context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
    }
}