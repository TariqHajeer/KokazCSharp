﻿using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.DiscountDtos;
using Quqaz.Web.Dtos.ReceiptDtos;
using System;
using System.Collections.Generic;

namespace Quqaz.Web.Dtos.OrdersDtos
{

    public class PrintOrdersDto
    {
        public int Id { get; set; }
        public int PrintNmber { get; set; }
        public string PrinterName { get; set; }
        public DateTime Date { get; set; }
        public string DestinationName { get; set; }
        public string DestinationPhone { get; set; }
        public List<ReceiptDto> Receipts { get; set; }
        public List<PrintDto> Orders { get; set; }
        public DiscountDto Discount { get; set; }

    }
    public class AgentPrintDetailDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string ClientName { get; set; }
        public string Note { get; set; }
        public string Region { get; set; }
    }
    public class PrintDto
    {
        public string Code { get; set; }
        public decimal Total { get; set; }

        public string Phone { get; set; }
        public string Country { get; set; }
        public string LastTotal { get; set; }
        public string ClientNote { get; set; }
        public decimal DeliveCost { get; set; }
        public string ClientName { get; set; }
        public string Note { get; set; }
        public string Region { get; set; }
        public DateTime? Date { get; set; }
        public string Address { get; set; }
        public int? MoneyPlacedId { get; set; }
        public int? OrderPlacedId { get; set; }
        public decimal PayForClient { get; set; }
        public NameAndIdDto Orderplaced { get; set; }
    }

}
