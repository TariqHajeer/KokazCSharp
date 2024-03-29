﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class AgentPrintDetail
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string ClientName { get; set; }
        public string Note { get; set; }
        public string Region { get; set; }
        public int AgentPrintId { get; set; }
        public string ClientNote { get; set; }
        public string Address { get; set; }
        public DateTime? Date { get; set; }

        public virtual AgentPrint AgentPrint { get; set; }
    }
}
