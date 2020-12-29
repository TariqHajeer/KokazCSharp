using System;
using System.Linq;
using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "Employee")]
    public class AbstractEmployeePolicyController : AbstractController
    {
        public AbstractEmployeePolicyController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        protected int AuthoticateUserId()
        {
            var userIdClaim = User.Claims.ToList().Where(c => c.Type == "UserID").Single();
            return Convert.ToInt32(userIdClaim.Value);
        }
    }
}