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
        [HttpPut("DisAccpet/{id}")]
        public IActionResult DisAccpet(int id)
        {
            var editRequest = this.Context.EditRequests.Find(id);
            editRequest.Accept = false;
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpPut("Accept/{id}")]
        public IActionResult Accept(int id)
        {
            return Ok();
        }
    }
}