using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeTypeController : AbstractEmployeePolicyController
    {



        public IncomeTypeController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var incomeTypes = this.Context.IncomeTypes
                .Include(c => c.Incomes)
                .ToList();
            List<IncomeTypeDto> incomeTypeDtos = new List<IncomeTypeDto>();
            foreach (var item in incomeTypes)
            {
                incomeTypeDtos.Add(new IncomeTypeDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CanDelete = item.Incomes.Count() == 0
                });
            }
            return Ok(incomeTypeDtos);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateIncomeTypeDto createIncomeTypeDto)
        {
            var similer = this.Context.IncomeTypes.Where(c => c.Name == createIncomeTypeDto.Name).Count();
            if (similer > 0)
                return Conflict();
            IncomeType incomeType = new IncomeType()
            {
                Name = createIncomeTypeDto.Name
            };
            this.Context.Add(incomeType);
            this.Context.SaveChanges();
            var response = new IncomeTypeDto()
            {
                Id = incomeType.Id,
                CanDelete = true,
                Name = incomeType.Name
            };
            return Ok(response);
        }
        [HttpPatch]
        public IActionResult Update([FromBody]UpdateIncomeTypeDto updateIncomeTypeDto)
        {
            try
            {
                var incomeType = this.Context.IncomeTypes.Where(c => c.Id == updateIncomeTypeDto.Id).FirstOrDefault();
                if (this.Context.IncomeTypes.Where(c => c.Name == updateIncomeTypeDto.Name && c.Id != updateIncomeTypeDto.Id).Any())
                    return Conflict();
                incomeType.Name = updateIncomeTypeDto.Name;
                this.Context.Update(incomeType);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var incomeType = this.Context.IncomeTypes.Find(id);
                if (incomeType == null)
                    return NotFound();
                this.Context.Entry(incomeType).Collection(c => c.Incomes).Load();
                if (incomeType.Incomes.Count() != 0)
                    return Conflict();
                this.Context.Remove(incomeType);
                this.Context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
    }
}