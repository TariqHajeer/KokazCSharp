using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace KokazGoodsTransfer.Services.Concret
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _accessor;
        public UserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public ClaimsPrincipal GetUser()
        {
            return _accessor?.HttpContext?.User as ClaimsPrincipal;
        }
        public string AuthoticateUserName()
        {
            return GetUser().Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
        }
        public int AuthoticateUserId()
        {
            var userIdClaim = GetUser().Claims.ToList().Where(c => c.Type == "UserID").Single();
            return Convert.ToInt32(userIdClaim.Value);
        }
    }
}
