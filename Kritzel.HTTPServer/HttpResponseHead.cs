using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public class HttpResponseHead
    {
        public int StatusCode = 200;
        public string ContentType = null;
        public string Location = null;
        public List<KeyValuePair<string,string>> Cookies = new List<KeyValuePair<string,string>>();
        public bool RequestLogin { get; set; } = false;
        public string LoginText { get; set; } = "";

        public HttpResponseHead() { }
        public HttpResponseHead(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public void Write(Stream stream, int length)
        {
            stream.WriteS("HTTP/1.1 {0} {1}\r\n", 
                StatusCode, HttpStatusCode.GetName(StatusCode));
            if (Location != null)
                stream.WriteS("Location: {0}\r\n", Location);
            stream.WriteS("Server: SimpleHTTP ({0})\r\n",
                Environment.OSVersion.VersionString);
            if (ContentType != null)
                stream.WriteS("Content-Type: {0}\r\n", ContentType);
            stream.WriteS("Content-Length: {0}\r\n", length);
            foreach (var cookie in Cookies)
                stream.WriteS("Set-Cookie: {0}={1}\r\n", cookie.Key, cookie.Value);
            if(RequestLogin)
                stream.WriteS("WWW-Authenticate: Basic realm=\"" + LoginText + "\", charset=\"UTF - 8\"");
            stream.WriteS("Connection: Closed\r\n");
        }
    }
}
