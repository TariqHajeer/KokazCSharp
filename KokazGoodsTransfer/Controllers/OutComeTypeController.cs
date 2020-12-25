using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.OutComeType;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutComeTypeController : ControllerBase
    {
        KokazContext context;
        public OutComeTypeController(KokazContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult GetALl()
        {
            var outComeTypes = this.context.OutComeTypes
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
            var similer = this.context.OutComeTypes.Where(c => c.Name == createOutComeTypeDto.Name).Count();
            if (similer > 0)
                return Conflict();
            OutComeType outComeType = new OutComeType()
            {
                Name = createOutComeTypeDto.Name
            };
            this.context.Add(outComeType); 
            this.context.SaveChanges();
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