using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Quqaz.Web.Controllers
{
    [EnableCors("EnableCORS")]
    public abstract class AbstractController : ControllerBase
    {
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
