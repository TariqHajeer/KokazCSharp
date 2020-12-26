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

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeTypeController : AbstractController
    {

        

        public IncomeTypeController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var incomeTypes = this.Context.IncomeTypes
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
    }
}