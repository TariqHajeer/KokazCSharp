using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Privilege
    {
        public Privilege()
        {
            GroupPrivileges = new HashSet<GroupPrivilege>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<GroupPrivilege> GroupPrivileges { get; set; }
    }
}
