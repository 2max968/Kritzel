using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public class HttpStatusCode
    {
        public static List<HttpStatusCode> Codes { get; private set; }

        public static void Init()
        {
            Codes = new List<HttpStatusCode>();
            string txt = Properties.Resources.StatusCodes;
            foreach(string line in txt.Split('\n'))
            {
                if(line.Length > 4)
                {
                    int code = 0;
                    string scode = line.Substring(0, 3);
                    string text = line.Substring(4).TrimEnd('\r');
                    if(int.TryParse(scode, out code))
                    {
                        Codes.Add(new HttpStatusCode(code, text));
                    }
                }
            }
        }

        public static string GetName(int code)
        {
            foreach(HttpStatusCode c in Codes)
                if (c.code == code)
                    return c.name;
            return "";
        }

        int code;
        string name;

        public HttpStatusCode(int code, string name)
        {
            this.code = code;
            this.name = name;
        }
    }
}
