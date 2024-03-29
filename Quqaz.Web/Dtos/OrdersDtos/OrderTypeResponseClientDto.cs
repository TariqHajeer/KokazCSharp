﻿using Quqaz.Web.Dtos.Countries;
using Quqaz.Web.Dtos.Regions;
using System;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class OrderResponseClientDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Cost { get; set; }
        public string RecipientName { get; set; }
        public string[] RecipientPhones { get; set; }
        public string Address { get; set; }
        public string ClientNote { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public string MoenyPlaced { get; set; }
        public string OrderPlaced { get; set; }
        public CountryDto Country { get; set; }
        public RegionDto  Region { get; set; }
        public bool CanUpdateOrDelete { get; set; }
    }

}
