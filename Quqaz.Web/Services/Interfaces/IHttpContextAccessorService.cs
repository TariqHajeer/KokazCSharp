using System.Collections.Generic;
using System.Security.Claims;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IHttpContextAccessorService
    {
        List<Claim> UserBranches();
        int CurrentBranchId();
        int[] Branches();
        int AuthoticateUserId();
        string AuthoticateUserName();
        int CurrentCountryId();

    }
}
