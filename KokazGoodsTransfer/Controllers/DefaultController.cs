using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : AbstractController
    {
        public DefaultController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            try
            {
                return Ok(Context.Database.CanConnect().ToString());
            }
            catch (Exception ex)
            {
                return Ok("False");
            }
        }
    }
}