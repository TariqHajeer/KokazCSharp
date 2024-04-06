using System;
using System.Globalization;

namespace Quqaz.Web.Services.Helper
{
    public static class DecimalExtensions
    {
        public static string ToCurrencyStringWithoutSymbol(this decimal amount)
        {
            // Get the integer part
            int integerPart = (int)Math.Truncate(amount);

            // Get the fractional part
            decimal fractionalPart = amount - integerPart;

            // Format the integer part with commas
            string formattedIntegerPart = Math.Abs(integerPart).ToString("#,##0");

            // If the amount is negative, add the negative sign to the formatted integer part
            if (amount < 0)
            {
                formattedIntegerPart = "-" + formattedIntegerPart;
            }

            // Concatenate the integer part and the fractional part (if any)
            string formattedNumber = formattedIntegerPart;
            if (fractionalPart != 0)
            {
                formattedNumber += "." + fractionalPart.ToString().TrimStart('-');
            }

            return formattedNumber;
        }
    }
}