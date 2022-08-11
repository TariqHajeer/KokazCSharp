using System.Collections.Generic;
using System.Security.Claims;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IHttpContextAccessorService
    {
        List<Claim> UserBranches();
        int CurrentBranchId();
        int[] Branches();
        int AuthoticateUserId();
        string AuthoticateUserName();

    }
}
