using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeTypeController : ControllerBase
    {

        KokazContext context;
        public IncomeTypeController(KokazContext context )
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var incomeTypes = this.context.IncomeTypes
                .Include(c=>c.Incomes)
                .ToList();
            List<IncomeTypeDto> incomeTypeDtos = new List<IncomeTypeDto>();
            foreach (var item in incomeTypes)
            {
                incomeTypeDtos.Add( new IncomeTypeDto()
                {
                    Id = item.Id,
                   Name =item.Name,
                    CanDelete =item.Incomes.Count()==0
                });
            }
            return Ok(incomeTypeDtos);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateIncomeTypeDto createIncomeTypeDto)
        {
            var similer = this.context.IncomeTypes.Where(c => c.Name == createIncomeTypeDto.Name).Count();
            if (similer > 0)
                return Conflict();
            IncomeType incomeType = new IncomeType()
            {
                Name = createIncomeTypeDto.Name
            };
            this.context.Add(incomeType);
            this.context.SaveChanges();
            var response = new IncomeTypeDto()
            {
                Id = incomeType.Id,
                CanDelete = true,
                Name = incomeType.Name
            };
            return Ok(response);
        }
    }
}