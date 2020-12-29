﻿using KokazGoodsTransfer.Dtos.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int? RegionId { get; set; }
        public string Address { get; set; }
        public DateTime FirstDate { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public RegionDto Region { get; set; }
        public string CreatedBy { get; set; }
        public bool CanDelete { get; set; } = true;
        public List<ClientPhoneDto> Phnoes { get; set; }
    }
    public class ClientPhoneDto 
    {
        public int Id { get; set; }
        public string Phone { get; set; }
    }
}