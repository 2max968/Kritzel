using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public enum HttpMethod { GET,POST,PUT,PATCH,DELETE,HEAD,Unknown};

    public class HttpRequestHead
    {
        public HttpMethod Method { get; private set; }
        public string Path { get; private set; }
        public string Content { get; private set; }
        public Dictionary<string, string> ClientInfo { get; private set; }
        public Dictionary<string,string> Cookies { get; private set; }
        public string UserAgent { get; private set; }
        public Dictionary<string, string> Get { get; private set; }
        public Dictionary<string, string> Post { get; private set; }

        public HttpRequestHead(string head)
        {
            this.Content = head;
            string[] lines = head.Split('\n');
            for(int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd('\r');
            }

            string[] words = lines[0].Split(' ');
            switch(words[0].Trim().ToLower())
            {
                case "get": Method = HttpMethod.GET; break;
                case "post": Method = HttpMethod.POST; break;
                case "put": Method = HttpMethod.PUT; break;
                case "patch": Method = HttpMethod.PATCH; break;
                case "delete": Method = HttpMethod.DELETE; break;
                case "head": Method = HttpMethod.HEAD; break;
                default:
                    Method = HttpMethod.Unknown;
                    Log.WriteLine(LType.Warning, "Unknwon method {0}", words[0]);
                    break;
            }

            ClientInfo = new Dictionary<string, string>();
            for(int i = 1; i < lines.Length; i++)
            {
                int colon = lines[i].IndexOf(':');
                if (colon >= 0)
                {
                    string name = lines[i].Substring(0, colon);
                    string value = lines[i].Substring(colon + 1);
                    if (!ClientInfo.ContainsKey(name))
                        ClientInfo[name] = value;
                }
            }

            if(ClientInfo.ContainsKey("Cookie"))
            {
                Cookies = HttpParse.ParseArgumentLine(ClientInfo["Cookie"], ';');
            }

            UserAgent = ClientInfo.ContainsKey("User-Agent") ? ClientInfo["User-Agent"] : "";
            Path = words[1];

            if (Path.Contains('?'))
            {
                int pos = Path.IndexOf('?');
                string argline = Path.Substring(pos + 1);
                Path = Path.Substring(0, pos);
                Get = HttpParse.ParseArgumentLine(argline);
            }
            else
            {
                Get = new Dictionary<string, string>();
            }
        }
    }
}
