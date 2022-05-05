namespace KokazGoodsTransfer.Models
{
    public partial class TreasuryHistory
    {
        public string Type
        {
            get
            {
                if (ClientPaymentId != null)
                    return "تسديد";
                if(ReceiptId!=null)
                {
                    if (Receipt.IsPay)
                        return "صرف";
                    return "قبض";
                }    
                if (CashMovmentId != null)
                    if (Amount > 0)
                        return "إعطاء";
                    else if (Amount < 0)
                        return "اخذ";
                    else
                        return "0";
                return "غير معلوم";
            }
        }
    }
}
