using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.ClientDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CStaticsController : AbstractClientPolicyController
    {
        public CStaticsController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet]
        public IActionResult Get()
        {
            StaticsDto staticsDto = new StaticsDto();
            
            return Ok(staticsDto);
        }
    }
}