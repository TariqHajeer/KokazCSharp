using KokazGoodsTransfer.Dtos.Currencies;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.IncomesDtos
{
    public class IncomeDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }
        public decimal Earining { get; set; }
        public string Note { get; set; }
        public IncomeTypeDto IncomeType { get; set; }
        public string CreatedBy { get; set; }
        public CurrencyDto Currency { get; set; }
    }
}
