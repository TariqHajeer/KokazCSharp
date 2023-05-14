namespace Quqaz.Web.Models.Static
{
    public enum OrderState
    {
        //قيد المعالجة
        Processing=1,
        //يوجب اخذ نقود من العميل
        ShortageOfCash=2,
        //الطلب منتهي
        Finished=3
    }
}
