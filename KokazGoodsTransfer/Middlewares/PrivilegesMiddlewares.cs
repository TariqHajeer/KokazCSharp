using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Middlewares
{
    public class PrivilegesMiddlewares
    {
        private readonly RequestDelegate _next;
        public PrivilegesMiddlewares(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var user = context.User;
            if (user?.Identity.IsAuthenticated==true)
            {
                var groups = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => int.Parse(c.Value));
                var httpContextAccessSerivce = context.RequestServices.GetService<IHttpContextAccessorService>();
                var groupSerivce = context.RequestServices.GetService<IGroupService>();
                var privliegs = await groupSerivce.GetGroupsPrviligesByGroupsIds(groups, httpContextAccessSerivce.CurrentBranchId());
                var claims = privliegs.Select(c => new Claim(ClaimTypes.Role, c.SysName));
                var appIdentity = new ClaimsIdentity(claims);
                user.AddIdentity(appIdentity);
            }
            await _next(context);

        }
    }
}
