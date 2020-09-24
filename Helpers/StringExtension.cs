using System;
using System.Text.RegularExpressions;

namespace Helpers
{
    public static class StringExtension
    {
        public static string MultiPregReplace(this string input, string[] pattern, string[] replacements)
        {
            if (replacements.Length != pattern.Length)
                throw new ArgumentException("Replacement and Pattern Arrays must be balanced");

            for (var i = 0; i < pattern.Length; i++)
            {
                var regex = new Regex(pattern[i], RegexOptions.Singleline);
                input = regex.Replace(input, replacements[i]);
            }

            return input;
        }
    }
}