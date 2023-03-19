using System.Security.Claims;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IUserService
    {
        ClaimsPrincipal GetUser();
        string AuthoticateUserName();
        int AuthoticateUserId();
    }
}
