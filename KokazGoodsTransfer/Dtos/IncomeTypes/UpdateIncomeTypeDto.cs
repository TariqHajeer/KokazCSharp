using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.IncomeTypes
{
    public class UpdateIncomeTypeDto: IIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
