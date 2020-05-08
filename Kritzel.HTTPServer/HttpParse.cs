using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public class HttpParse
    {
        public static Dictionary<string, string> ParseArgumentLine(string args, char chr = '&')
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            foreach (string arg in args.Split(chr))
            {
                int eqP = arg.IndexOf('=');
                string key, value;
                if (eqP >= 0)
                {
                    key = arg.Substring(0, eqP);
                    value = arg.Substring(eqP + 1);
                }
                else
                {
                    key = arg;
                    value = "";
                }
                ret.Add(key.Trim(), value.Trim());
            }
            return ret;
        }
    }
}
