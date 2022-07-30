using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace KokazGoodsTransfer.Services.Concret
{
    public class HttpContextAccessorService : IHttpContextAccessorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextAccessorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Claim> UserBranches()
        {
            var userBranches = _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == "branchId").ToList();
            return userBranches;
        }
        public int[] Branches()
        {
            var userBranches = UserBranches();
            if (userBranches.Any())
            {
                return userBranches.Select(c => int.Parse(c.Value)).ToArray();
            }
            return new int[0];
        }
        public int CurrentBranchId()
        {
            int branchId;
            var userBranches = UserBranches();
            if (userBranches.Any())
            {
                branchId = int.Parse(userBranches.First().Value);
                if (userBranches.Count > 1)
                {
                    string branch = _httpContextAccessor.HttpContext.Request.Headers["branchId"];
                    if (string.IsNullOrEmpty(branch))
                        branchId = int.Parse(branch);
                    return branchId;
                }
                return branchId;
            }
             throw new System.Exception("Branch Not Exist");
        }

        public int AuthoticateUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.ToList().Where(c => c.Type == "UserID").Single();
            return int.Parse(userIdClaim.Value);
        }

        public string AuthoticateUserName()
        {
            return _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
        }
    }
}
