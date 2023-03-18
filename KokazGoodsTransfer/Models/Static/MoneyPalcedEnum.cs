using System.ComponentModel;

namespace KokazGoodsTransfer.Models.Static
{
    public enum MoneyPalced
    {
        [Description("خارج الشركة")]
        OutSideCompany =1,
        [Description("مندوب")]
        WithAgent=2,
        [Description("داخل الشركة")]
        InsideCompany=3,
        [Description("تم التسليم")]
        Delivered=4
    }
}
