using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public class HttpClient
    {
        int port;
        IPAddress ip;
        IPEndPoint ipEp;
        TcpClient client;
        StreamReader reader;
        public string Path { get; private set; }
        public HttpMethod Method { get; set; } = HttpMethod.GET;
        public string UserAgent { get; set; } = "SimpleHttpClient";
        Thread thread;
        List<byte> data;
        public string Head { get; private set; }
        public int StatusCode { get; private set; }
        public byte[] Content { get; private set; }
        public string Message { get; private set; } = "";
        public string Headline { get; private set; }
        public bool ProtocolError { get; private set; } = false;
        public bool Ready { get; private set; } = false;
        public string Host { get; set; } = "null";
        public static Encoding UTF8 { get; private set; } = Encoding.UTF8;
        public string Location { get; private set; } = null;
        public Dictionary<string, string> MessageInfos { get; private set; } = new Dictionary<string, string>();

        public HttpClient(IPAddress ip, int port, string path)
        {
            this.ip = ip;
            this.port = port;
            this.Path = path;
            ipEp = new IPEndPoint(ip, port);
            client = new TcpClient();
        }

        public HttpClient(IPAddress ip, string path)
            : this(ip, 80, path) { }

        public async void Send()
        {
            client.Connect(ipEp);

            thread = new Thread(asyncRecieve);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1} HTTP/1.1\r\n", Method, Path);
            sb.AppendFormat("Host: {0}\r\n", Host);
            sb.AppendFormat("User-Agent: {0} ({1})\r\n", UserAgent, Environment.OSVersion.VersionString);
            sb.Append("Content-Length: 0\r\n");
            sb.Append("Connection: close\r\n");
            sb.Append("\r\n\r\n");
            byte[] header = Encoding.UTF8.GetBytes(sb.ToString());
            await client.GetStream().WriteAsync(header, 0, header.Length);
            
            reader = new StreamReader(client.GetStream());
            thread.Start();
        }

        void asyncRecieve(object obj)
        {
            data = new List<byte>();

            while(!reader.EndOfStream)
            {
                data.Add((byte)reader.Read());
            }

            int i;
            for(i = 3; i < data.Count; i++)
            {
                if(data[i-3] == '\r' && data[i-2] == '\n' && data[i-1] == '\r' && data[i] == '\n')
                {
                    break;
                }
            }
            int hlength = i;

            bool headline = true;
            for(int j = 0; j < hlength - 4;j++)
            {
                Head += (char)data[j];
                if(headline)
                {
                    if (data[j] == '\r' || data[j] == '\n')
                        headline = false;
                    else
                    {
                        Headline += (char)data[j];
                    }
                }
            }

            string[] words = Headline.Split(' ');
            int statusCode = 0;
            if(words.Length < 2 || !int.TryParse(words[1], out statusCode))
            {
                ProtocolError = true;
            }
            else
            {
                this.StatusCode = statusCode;
                if (words.Length > 2)
                    Message = words[2];
            }

            string[] lines = Head.Split('\n');
            for(int j = 1; j < lines.Length; j++)
            {
                string line = lines[j].Trim(' ', '\n', '\r');
                int sep = line.IndexOf(':');
                if(sep >= 0)
                {
                    string key = line.Substring(0, sep);
                    string value = line.Substring(sep + 2);
                    if(!MessageInfos.ContainsKey(key)) MessageInfos.Add(key, value);
                }
            }
            if (MessageInfos.ContainsKey("Location")) Location = MessageInfos["Location"];

            Content = new byte[data.Count - hlength - 1];
            for (int j = hlength + 1; j < data.Count; j++)
            {
                Content[j - hlength - 1] = data[j];
            }

            Ready = true;
        }

        public string GetContentAsString()
        {
            return Encoding.UTF8.GetString(Content);
        }

        public static HttpClient FromHref(string href)
        {
            int protocolPos = href.IndexOf("://");
            if(protocolPos >= 0)
            {
                string protocol = href.Substring(0, protocolPos);
                if (protocol.ToLower() != "http")
                    throw new WrongProtocolException(protocol);
                href = href.Substring(protocolPos + 3);
            }

            string server = "";
            string path = "/";
            int port = 80;

            int slashI = href.IndexOf('/');
            if (slashI >= 0)
            {
                server = href.Substring(0, slashI);
                path = href.Substring(slashI);
            }
            else
            {
                server = href;
            }

            string portS = "80";
            string serverName = "";

            int colonI = server.IndexOf(':');
            if(colonI >= 0)
            {
                serverName = server.Substring(0, colonI);
                portS = server.Substring(colonI + 1);
            }
            else
            {
                serverName = server;
            }

            if(!int.TryParse(portS, out port))
            {
                throw new ParsingException("Invalid number " + portS, href);
            }

            IPAddress ip;
            if (!IPAddress.TryParse(serverName, out ip))
            {
                IPAddress[] list = Dns.GetHostAddresses(serverName);
                List<IPAddress> listv4 = new List<IPAddress>();
                foreach(IPAddress add in list)
                {
                    if (add.AddressFamily == AddressFamily.InterNetwork)
                        listv4.Add(add);
                }
                if(listv4.Count == 0)
                {
                    throw new ParsingException("Invalid Hostname " + serverName, href);
                }
                else
                {
                    ip = listv4[0];
                }
            }
            
            HttpClient client = new HttpClient(ip, port, path);
            client.Host = serverName;
            client.UserAgent = "Mozilla/5.0";
            return client;
        }

        public static byte[] GetHttpData(string href, string hostname = null)
        {
            HttpClient client = null;
            bool r = true;
            while (r)
            {
                client = FromHref(href);
                if (hostname != null) client.Host = hostname;
                client.Send();
                while (!client.Ready) ;
                if (client.Location != null) href = client.Location;
                else r = false;
            }

            return client.Content;
        }

        public static string GetHttpString(string href, string hostname = null)
        {
            return UTF8.GetString(GetHttpData(href, hostname));
        }

        public abstract class ClientException : Exception
        {
            public ClientException(string message) : base(message) { }
        }

        public class ParsingException : ClientException
        {
            public string HRef { get; private set; }

            public ParsingException(string message, string href)
                : base(message)
            {
                this.HRef = href;
            }
        }

        public class WrongProtocolException : ClientException
        {
            public WrongProtocolException(string protocol)
                : base("Unknown protocol type \"" + protocol + "\"")
            {
                
            }
        }
    }
}
