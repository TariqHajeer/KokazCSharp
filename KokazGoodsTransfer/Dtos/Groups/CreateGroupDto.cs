using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Groups
{
    public class CreateGroupDto
    {
        public string Name;
        public int[] PrivilegesId { get; set; }
    }
}
