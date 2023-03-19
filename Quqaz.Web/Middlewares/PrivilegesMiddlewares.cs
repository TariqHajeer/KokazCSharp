using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Quqaz.Web.Middlewares
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
            if (user?.Identity.IsAuthenticated == true)
            {
                var userId = int.Parse(user.Claims.First(c => c.Type == "UserID").Value);
                var groupSerivce = context.RequestServices.GetService<IGroupService>();
                var httpContextAccessSerivce = context.RequestServices.GetService<IHttpContextAccessorService>();
                var privliegs = await groupSerivce.GetPrviligesByUserAndBranchId(userId, httpContextAccessSerivce.CurrentBranchId());
                var claims = privliegs.Select(c => new Claim(ClaimTypes.Role, c.SysName));
                var appIdentity = new ClaimsIdentity(claims);
                user.AddIdentity(appIdentity);
            }
            await _next(context);

        }
    }
}
