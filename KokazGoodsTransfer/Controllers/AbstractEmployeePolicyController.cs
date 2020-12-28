using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Employee")]
    public class AbstractEmployeePolicyController : AbstractController
    {
        public AbstractEmployeePolicyController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}