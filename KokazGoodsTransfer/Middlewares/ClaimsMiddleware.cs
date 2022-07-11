using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Middlewares
{
    public class ClaimsMiddleware
    {
        private readonly RequestDelegate _next;
        public async Task Invoke(HttpContext context)
        {
            //var user = context.User;
            //var claims = user.Claims;
            ////https://stackoverflow.com/questions/22570743/how-do-i-remove-an-existing-claim-from-a-claimsprincipal
            await _next(context);
        }
    }
}
