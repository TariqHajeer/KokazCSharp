﻿using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.Countries;
using Quqaz.Web.Dtos.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Clients
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime FirstDate { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public NameAndIdDto Country { get; set; }
        public bool CanDelete { get; set; } = true;
        public decimal Total { get; set; }
        public List<PhoneDto> Phones { get; set; }
        public string Mail { get; set; }
        public int Points { get; set; }
    }
    
}
