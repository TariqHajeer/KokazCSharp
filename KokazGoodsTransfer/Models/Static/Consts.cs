using KokazGoodsTransfer.Dtos.Common;

namespace KokazGoodsTransfer.Models.Static
{
    public static class Consts
    {
        public readonly static NameAndIdDto[] OrderPlaceds = new NameAndIdDto[] { OrderplacedEnum.Client, OrderplacedEnum.Store, OrderplacedEnum.Way, OrderplacedEnum.Delivered, OrderplacedEnum.CompletelyReturned, OrderplacedEnum.PartialReturned, OrderplacedEnum.Unacceptable, OrderplacedEnum.Delayed };
        public readonly static NameAndIdDto[] MoneyPlaceds = new NameAndIdDto[] { MoneyPalcedEnum.OutSideCompany, MoneyPalcedEnum.WithAgent, MoneyPalcedEnum.InsideCompany, MoneyPalcedEnum.Delivered };
    }
}
