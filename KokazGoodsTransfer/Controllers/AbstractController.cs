using AutoMapper;
using KokazGoodsTransfer.Helpers;
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
        protected KokazContext _context;
        protected IMapper _mapper;
        protected readonly Logging _logging;
        public AbstractController(KokazContext context, IMapper mapper,Logging logging)
        {
            _context = context;
            _mapper = mapper;
            _logging = logging;
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
        public override ConflictResult Conflict()
        {
            return base.Conflict();
        }
        

    }
}