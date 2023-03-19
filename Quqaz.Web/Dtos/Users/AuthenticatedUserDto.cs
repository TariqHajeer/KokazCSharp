using Quqaz.Web.Dtos.BranchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Users
{
    public class AuthenticatedUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public List<UserPrivilegeDto> Privileges { get; set; }
        public string Policy { get; set; }
        public bool HaveTreasury { get; set; }
        public IEnumerable<BranchDto> Branches { get; set; }
    }
    public class UserPrivilegeDto
    {
        public string Name { get; set; }
        public string SysName { get; set; }
    }
}
