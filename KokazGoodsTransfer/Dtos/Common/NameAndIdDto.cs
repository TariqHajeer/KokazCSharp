using KokazGoodsTransfer.Helpers.Extensions;
using KokazGoodsTransfer.Models.Static;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class NameAndIdDto
    {
        public NameAndIdDto()
        {

        }
        private NameAndIdDto(OrderplacedEnum orderplacedEnum)
        {
            this.Id = (int)orderplacedEnum;
            this.Name = orderplacedEnum.GetDescription();
        }
        private NameAndIdDto(MoneyPalcedEnum moneyPalcedEnum)
        {
            this.Id = (int)moneyPalcedEnum;
            this.Name = moneyPalcedEnum.GetDescription();
        }
        public static implicit operator NameAndIdDto (OrderplacedEnum orderplacedEnum)
        {
            return new NameAndIdDto(orderplacedEnum);
        }
        public static implicit operator NameAndIdDto(MoneyPalcedEnum moneyPalcedEnum)
        {
            return new NameAndIdDto(moneyPalcedEnum);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; }
    }
}
