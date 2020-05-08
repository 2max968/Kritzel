using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public class RequestHandler
    {
        public HttpResponse Response { get; set; }
        public bool Handled { get; set; } = false;
        public Dictionary<string, string> Args { get; private set; }
        public string FileName { get; private set; }
        public HttpRequestHead Head { get; private set; }
        public HttpServer Server { get; private set; }
        public FileInfo FileInfo { get; private set; }

        public RequestHandler(HttpServer server, HttpRequestHead head)
        {
            string fName;
            string argLine;
            int pos = head.Path.IndexOf('?');
            this.Head = head;
            this.Server = server;
            if (pos >= 0)
            {
                fName = head.Path.Substring(0, pos);
                argLine = head.Path.Substring(pos + 1);
            }
            else
            {
                fName = head.Path;
                argLine = "";
            }
            Args = HttpParse.ParseArgumentLine(argLine);
            FileName = fName;
            
            FileInfo = new FileInfo(Path.Combine(server.WwwDir, fName.TrimStart('/')));
        }

        public void Handle()
        {
            HttpServer.MethodRunner runner = Server.GetMethod(FileName.TrimStart('/'));

            if (runner != null)
            {
                Log.WriteLine(LType.Script, "Running script \"{0}\"", FileName);
                runner.Invoke(Server, this);

                if(Response == null)
                {
                    Log.WriteLine(LType.Error, "Script {0} didn't handled the request", FileName);
                    Response = new HttpResponseMessage(Server, 500);
                }
            }
            else
            {
                if (FileInfo.Exists)
                {
                    Response = new HttpResponseFile(Server, FileInfo.FullName);
                }
                else
                {
                    Response = new HttpResponseMessage(Server, 404);
                }
            }
        }
    }
}
