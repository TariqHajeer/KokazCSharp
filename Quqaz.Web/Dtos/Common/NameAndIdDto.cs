using Quqaz.Web.Helpers.Extensions;
using Quqaz.Web.Models.Static;

namespace Quqaz.Web.Dtos.Common
{
    public class NameAndIdDto
    {
        public NameAndIdDto()
        {

        }
        private NameAndIdDto(OrderPlace orderplacedEnum)
        {
            this.Id = (int)orderplacedEnum;
            this.Name = orderplacedEnum.GetDescription();
        }
        private NameAndIdDto(MoneyPalce moneyPalcedEnum)
        {
            this.Id = (int)moneyPalcedEnum;
            this.Name = moneyPalcedEnum.GetDescription();
        }
        public static implicit operator NameAndIdDto (OrderPlace orderplacedEnum)
        {
            return new NameAndIdDto(orderplacedEnum);
        }
        public static implicit operator NameAndIdDto(MoneyPalce moneyPalcedEnum)
        {
            return new NameAndIdDto(moneyPalcedEnum);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; }
    }
}
