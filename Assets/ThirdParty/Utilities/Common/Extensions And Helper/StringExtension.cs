/**
 * Author NBear - nbhung71711 @gmail.com - 2018
 **/

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities.Common
{
    public static class StringExtension
    {
        public static string ToSentenceCase(this string pString)
        {
            var lowerCase = pString.ToLower();
            // matches the first sentence of a string, as well as subsequent sentences
            var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
            // MatchEvaluator delegate defines replacement of setence starts to uppercase
            var result = r.Replace(lowerCase, s => s.Value.ToUpper());
            return result;
        }

        public static string ToCapitalizeEachWord(this string pString)
        {
            // Creates a TextInfo based on the "en-US" culture.
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(pString);
        }
    }

    public static class StringHelper
    {
        public static string JoinString(string seperator, params string[] strs)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < strs.Length; i++)
            {
                if (!string.IsNullOrEmpty(strs[i]))
                    list.Add(strs[i]);
            }
            return string.Join(seperator, list.ToArray());
        }

        public static void SeparateStringAndNum(string pStr, out string pNumberPart, out string pStringPart)
        {
            pNumberPart = "";
            var regexObj = new Regex(@"[-+]?[0-9]*\.?[0-9]+(?:[eE][-+]?[0-9]+)?", RegexOptions.IgnorePatternWhitespace);
            var match = regexObj.Match(pStr);
            pNumberPart = match.ToString();

            pStringPart = pStr.Replace(pNumberPart, "");
        }

        public static string formatDollarNumber(long pNumber)
        {
            var mStringBuilder = new StringBuilder();
            if (pNumber >= 1000)
            {
                if (mStringBuilder == null)
                {
                    mStringBuilder = new StringBuilder();
                }
                mStringBuilder.Length = (0);
                char[] chars = pNumber.ToString().ToCharArray();
                int index = 1;
                for (int i = chars.Length - 1; i >= 0; i--)
                {
                    mStringBuilder.Append(chars[i]);
                    if (index % 3 == 0 && i > 0)
                    {
                        mStringBuilder.Append(".");
                    }
                    index++;
                }
                Reverse(mStringBuilder);
                return mStringBuilder.ToString();
            }
            return pNumber.ToString();
        }

        public static void Reverse(StringBuilder sb)
        {
            char t;
            int end = sb.Length - 1;
            int start = 0;

            while (end - start > 0)
            {
                t = sb[end];
                sb[end] = sb[start];
                sb[start] = t;
                start++;
                end--;
            }
        }
    }
}
