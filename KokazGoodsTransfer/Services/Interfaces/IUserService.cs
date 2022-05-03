using System.Security.Claims;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IUserService
    {
        ClaimsPrincipal GetUser();
        string AuthoticateUserName();
        int AuthoticateUserId();
    }
}
