using KokazGoodsTransfer.Dtos.Common;

namespace KokazGoodsTransfer.Models.Static
{
    public static class Consts
    {
        public readonly static NameAndIdDto[] OrderPlaceds = new NameAndIdDto[] { Orderplaced.Client, Orderplaced.Store, Orderplaced.Way, Orderplaced.Delivered, Orderplaced.CompletelyReturned, Orderplaced.PartialReturned, Orderplaced.Unacceptable, Orderplaced.Delayed };
        public readonly static NameAndIdDto[] MoneyPlaceds = new NameAndIdDto[] { MoneyPalced.OutSideCompany, MoneyPalced.WithAgent, MoneyPalced.InsideCompany, MoneyPalced.Delivered };
    }
}
