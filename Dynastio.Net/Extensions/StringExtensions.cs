using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string Value)
        {
            return string.IsNullOrEmpty(Value);
        }
        public static string RemoveLines(this string Value)
        {
            var x = Regex.Replace(Value, @"\t|\n|\r|\r\n|\n\r|", "");
            //x = x.Replace(System.Environment.NewLine, "")
            //    .Replace(@"\r\n", "")
            //    .Replace(@"\n\r", "");
            return x;
        }
        public static string Summarizing(this string text, int MaxLength, bool Dots = true)
        {
            if (text.Length > MaxLength) return text.Substring(0, MaxLength - 3) + (Dots ? ".." : "");
            return text;
        }
        public static string AlignCenter(this string text, int Width)
        {
            text = text.Length > Width ? text.Substring(0, Width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text)) return new string(' ', Width);
            return text.PadRight(Width - (Width - text.Length) / 2).PadLeft(Width);
        }
        public static bool HasAnyCyrillic(this string text)
        {
            return Regex.IsMatch(text, @"\p{IsCyrillic}");
        }
    }
}
