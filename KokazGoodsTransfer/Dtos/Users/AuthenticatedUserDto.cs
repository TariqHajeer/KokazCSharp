using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Users
{
    public class AuthenticatedUserDto
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public List<UserPrivilegeDto> Privileges { get; set; }
        public string Policy { get; set; }
    }
    public class UserPrivilegeDto
    {
        public string Name { get; set; }
        public string SysName { get; set; }
    }
}
