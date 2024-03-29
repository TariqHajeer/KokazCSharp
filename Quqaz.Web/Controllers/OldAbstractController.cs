﻿using AutoMapper;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Quqaz.Web.Controllers
{
    [EnableCors("EnableCORS")]
    public abstract class OldAbstractController : ControllerBase
    {
        protected KokazContext _context;
        protected IMapper _mapper;
        public OldAbstractController(KokazContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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