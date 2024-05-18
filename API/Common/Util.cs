using System.Text.RegularExpressions;

namespace API.Common
{
    public static class Util
    {
        public  static string MaskCardNumber(string holderNumber)
        {
            holderNumber = Regex.Replace(holderNumber, "[^0-9]", "");

            var firstSixDigits = holderNumber.Substring(0, 6);
            var lastFourDigits = holderNumber.Substring(holderNumber.Length - 4, 4);

            var requiredMask = new string('X', holderNumber.Length - firstSixDigits.Length - lastFourDigits.Length);

            var maskedCardNumber = string.Concat(firstSixDigits, requiredMask, lastFourDigits);

            return maskedCardNumber;
        }
    }
}
