using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace KokazGoodsTransfer.Controllers
{
    [EnableCors("EnableCORS")]
    public abstract class AbstractController : ControllerBase
    {
        protected KokazContext Context;
        protected IMapper mapper;
        public AbstractController(KokazContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }
        protected int AuthoticateUserId()
        {
            
            var userIdClaim = User.Claims.ToList().Where(c => c.Type == "UserID").Single();
            return Convert.ToInt32(userIdClaim.Value);
        }
        protected string AuthoticateUserName()
        {
            
            return User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            
        }
    }
}