using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("EnableCORS")]
    public class EmptyInjectionController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("it's work");
        }
    }
}