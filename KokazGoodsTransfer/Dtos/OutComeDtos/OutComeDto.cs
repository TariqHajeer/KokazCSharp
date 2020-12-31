using KokazGoodsTransfer.Dtos.Currencies;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Dtos.OutComeTypeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OutComeDtos
{
    public class OutComeDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public CurrencyDto Currency { get; set; }
        public OutComeTypeDto OutComeType { get; set; }
    }
}
