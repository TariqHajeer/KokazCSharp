﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Groups
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> PrivilegesId { get; set; }
    }
}
