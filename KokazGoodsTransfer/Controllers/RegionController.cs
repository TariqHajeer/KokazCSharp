using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        KokazContext Context;
        public RegionController(KokazContext context )
        {
            this.Context = context;
        }
        [HttpPost]
        public IActionResult Create(CreateRegionDto createRegionDto)
        {
             
            return Ok();
        }
    }
}