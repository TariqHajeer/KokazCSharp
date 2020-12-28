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
namespace KokazGoodsTransfer.Controllers
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
                .Include(c=>c.OutComes)
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
        public IActionResult Create([FromBody]CreateOutComeTypeDto createOutComeTypeDto  )
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
    }
}