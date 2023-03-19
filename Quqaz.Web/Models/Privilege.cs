using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class Privilege
    {
        public Privilege()
        {
            GroupPrivileges = new HashSet<GroupPrivilege>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string SysName { get; set; }

        public virtual ICollection<GroupPrivilege> GroupPrivileges { get; set; }
    }
}
