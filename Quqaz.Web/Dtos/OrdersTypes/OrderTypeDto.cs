﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.OrdersTypes
{
    public class OrderTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; }
    }
}
