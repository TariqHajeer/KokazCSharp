using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.EditRequestDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditRequestController : AbstractEmployeePolicyController
    {
        public EditRequestController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet("NewEditReuqet")]
        public IActionResult NewEditRequest()
        {
            var newEditRquests = this.Context.EditRequests.Where(c => c.Accept == null).ToList();
            return Ok(mapper.Map<EditRequestDto[]>(newEditRquests));
        }
        [HttpPut("DisAccpet")]
        public IActionResult DisAccpet([FromBody] int id)
        {
            var editRequest = this.Context.EditRequests.Find(id);
            editRequest.Accept = false;
            editRequest.UserId = AuthoticateUserId();
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpPut("Accept")]
        public IActionResult Accept([FromBody]int id)
        {
            var editRequest = this.Context.EditRequests.Find(id);
            editRequest.Accept = true;
            editRequest.UserId = AuthoticateUserId();
            this.Context.SaveChanges();
            return Ok();
        }
    }
}