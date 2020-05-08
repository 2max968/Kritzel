using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kritzel.WebUI
{
    public class DocumentParser
    {
        public static string GetTitle(string content)
        {
            string pattern = @"<title>(?<title>.*)</title>";
            Regex rx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match match = rx.Match(content);
            if (match.Success)
                return match.Groups["title"].Value;
            return "";
        }
    }
}
