using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Get()
        {
            return Ok("it's work");
        }
        [HttpGet("connection")]
        public IActionResult GetConnectonString()
        {
            return Ok(this.Context.Database.GetDbConnection().ConnectionString);
        }
        [HttpGet("status")]
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
        [HttpGet("De")]
        public IActionResult GetDepartmnetsName()
        {
            try
            {
                return Ok(this.Context.Departments.Select(c => c.Name));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}