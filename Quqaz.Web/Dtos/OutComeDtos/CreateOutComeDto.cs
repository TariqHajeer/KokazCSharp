﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.OutComeDtos
{
    public class CreateOutComeDto
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public int OutComeTypeId { get; set; }
    }
}
