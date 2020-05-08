using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public class ClientConnection
    {
        HttpServer server;
        TcpClient client;
        Thread thread;
        NetworkStream stream;

        public ClientConnection(HttpServer server, TcpClient client)
        {
            Log.WriteLine(LType.NetInfo, "Connection from {0}", client.Client.RemoteEndPoint);
            this.server = server;
            this.client = client;
            this.thread = new Thread(run);
            thread.Start();
        }

        void run(object obj)
        {
            try
            {
                stream = client.GetStream();
                _run();
            }
            catch (Exception ex)
            {
                StackTrace stack = new StackTrace(ex, true);
                var frame = stack.GetFrame(0);

                Log.WriteLine(LType.Error,
                    "Error on Connection Handling: {0}\n{1}@{2}[{3}]",
                    ex.Message, frame.GetMethod(), frame.GetFileName(), frame.GetFileLineNumber());
                try
                {
                    HttpResponse resp = new HttpResponseMessage(this.server, 500);
                    resp.Write(stream);
                    stream.Close();
                }
                catch (Exception) { }
                client.Close();
            }
        }

        void _run()
        {
            byte[] msg = new byte[2048];
            stream.ReadTimeout = server.ReadTimeout;
            int l = stream.Read(msg, 0, msg.Length);
            string txt = Encoding.Default.GetString(msg);
            txt = txt.Trim('\0');
            int headEnd = txt.IndexOf("\r\n\r\n");
            if (headEnd < 0) headEnd = txt.Length - 1;
            string head = txt.Substring(0, headEnd);
            HttpRequestHead httpHead = new HttpRequestHead(head);
            Log.WriteLine(LType.HttpInfo, "{2}: {0} {1}{3}User-Agent: {4}", 
                httpHead.Method, httpHead.Path, client.Client.RemoteEndPoint, Environment.NewLine, httpHead.UserAgent);

            RequestHandler handler = new RequestHandler(this.server, httpHead);
            this.server.Handle(handler);
            if (!handler.Handled)
                handler.Handle();
            HttpResponse resp = handler.Response;
            resp.Write(stream);
            stream.Close();

            client.Close();
            Log.WriteLine(LType.NetInfo, "Connection closed");
        }
    }
}
