﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.IncomesDtos
{
    public class CreateIncomeDto
    {
        
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public decimal Earining { get; set; }
        public string Note { get; set; }
        public int IncomeTypeId { get; set; }
        
    }
}
