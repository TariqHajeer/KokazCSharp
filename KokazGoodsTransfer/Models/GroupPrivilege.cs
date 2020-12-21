using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class GroupPrivilege
    {
        public int GroupId { get; set; }
        public int PrivilegId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Privilege Privileg { get; set; }
    }
}
