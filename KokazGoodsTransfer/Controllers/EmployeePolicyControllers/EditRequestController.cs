using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            var newEditRquest = this.Context.EditRequests.Where(c => c.Accept == null).ToList();
            return Ok();
        }
    }
}