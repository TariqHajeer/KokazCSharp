using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        KokazContext Context;
        public AuthController(KokazContext context )
        {
            this.Context = context;
        }
        [HttpPost]
        public IActionResult Login ([FromBody] LoginDto loginDto)
        {
            return Ok();    
        }
    }
}