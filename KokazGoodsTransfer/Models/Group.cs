using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupPrivileges = new HashSet<GroupPrivilege>();
            UserGroups = new HashSet<UserGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<GroupPrivilege> GroupPrivileges { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }

    }
}
