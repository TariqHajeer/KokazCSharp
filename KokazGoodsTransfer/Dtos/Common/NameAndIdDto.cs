using KokazGoodsTransfer.Helpers.Extensions;
using KokazGoodsTransfer.Models.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class NameAndIdDto
    {
        public NameAndIdDto(OrderplacedEnum orderplacedEnum)
        {
            this.Id = (int)orderplacedEnum;
            this.Name = orderplacedEnum.GetDescription();
        }
        public NameAndIdDto(MoneyPalcedEnum moneyPalcedEnum)
        {
            this.Id = (int)moneyPalcedEnum;
            this.Name = moneyPalcedEnum.GetDescription();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; }
    }
}
