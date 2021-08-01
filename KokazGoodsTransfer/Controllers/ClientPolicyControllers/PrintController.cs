﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintController : AbstractClientPolicyController
    {
        public PrintController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            //var print = this.Context.Printeds
            //    .Include(c=>c.)
            //    .Find(id);
            return Ok();
        }
    }
}