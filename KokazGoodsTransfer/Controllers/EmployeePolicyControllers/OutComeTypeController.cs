using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OutComeType;
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
        public OutComeTypeController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetALl()
        {
            var outComeTypes = this.Context.OutComeTypes
                .Include(c => c.OutComes)
                .ToList();
            List<OutComeTypeDto> response = new List<OutComeTypeDto>();
            foreach (var item in outComeTypes)
            {
                response.Add(new OutComeTypeDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CanDelete = item.OutComes.Count() == 0
                });
            }
            return Ok();
        }
        [HttpPost]
        public IActionResult Create([FromBody]CreateOutComeTypeDto createOutComeTypeDto)
        {
            var similer = this.Context.OutComeTypes.Where(c => c.Name == createOutComeTypeDto.Name).Count();
            if (similer > 0)
                return Conflict();
            OutComeType outComeType = new OutComeType()
            {
                Name = createOutComeTypeDto.Name
            };
            this.Context.Add(outComeType);
            this.Context.SaveChanges();
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
                var outComeType = this.Context.OutComeTypes.Find(updateOutComeTypeDto.Id);
                if (outComeType == null)
                    return NotFound();
                
                if (Context.OutComeTypes.Where(c=>c.Name==updateOutComeTypeDto.Name&&c.Id!=updateOutComeTypeDto.Id).Any())
                    return Conflict();
                outComeType.Name = updateOutComeTypeDto.Name;
                this.Context.Update(outComeType);
                this.Context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
                
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var outComeType = this.Context.OutComeTypes.Find(id);
                if (outComeType == null)
                    return NotFound();
                this.Context.Entry(outComeType).Collection(c => c.OutComes).Load();
                if (outComeType.OutComes.Any())
                    return Conflict();
                this.Context.Remove(outComeType);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}