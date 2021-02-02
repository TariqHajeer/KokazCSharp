using System;
using System.Linq;
using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
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