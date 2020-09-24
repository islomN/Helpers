using System.Text.RegularExpressions;

namespace Helpers
{
    public class TinHelper
    {
        private const string TinRegex = @"\b^[2|3|4|5|6]{1}\d{8}$\b";
        private const string IndividualTinRegex = @"\b^[4|5|6]{1}\d{8}$\b";
        private const string LegalTinRegex = @"\b^[2|3]{1}\d{8}$\b";

        public static bool IsTin(string value)
        {
            return Check(value, TinRegex);
        }
        
        public static bool IsIndividualTin(string value)
        {
            return Check(value, IndividualTinRegex);
        }

        public static bool IsLegalTin(string value)
        {
            return Check(value, LegalTinRegex);
        }

        private static bool Check(string value, string regex)
        {
            if (value == null)
            {
                return false;
            }

            var rx = new Regex(regex,RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return rx.IsMatch(value);
        }
    }
}
