using System.ComponentModel;

namespace Quqaz.Web.Models.Static
{
    public enum MoneyPalce
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
