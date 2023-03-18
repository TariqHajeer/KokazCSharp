using KokazGoodsTransfer.Dtos.Common;

namespace KokazGoodsTransfer.Models.Static
{
    public static class Consts
    {
        public readonly static NameAndIdDto[] OrderPlaceds = new NameAndIdDto[] { OrderPlace.Client, OrderPlace.Store, OrderPlace.Way, OrderPlace.Delivered, OrderPlace.CompletelyReturned, OrderPlace.PartialReturned, OrderPlace.Unacceptable, OrderPlace.Delayed };
        public readonly static NameAndIdDto[] MoneyPlaceds = new NameAndIdDto[] { MoneyPalce.OutSideCompany, MoneyPalce.WithAgent, MoneyPalce.InsideCompany, MoneyPalce.Delivered };
    }
}
