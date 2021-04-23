using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("EnableCORS")]
    public class MapperIjectControllerController : ControllerBase
    {
        IMapper mapper;
        public MapperIjectControllerController(IMapper mapper)
        {
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("it's work");
        }
    }
}