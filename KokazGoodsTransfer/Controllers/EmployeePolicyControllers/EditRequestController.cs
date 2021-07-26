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
            return Ok();
        }
    }
}