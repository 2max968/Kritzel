using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public class HttpServer
    {
        public delegate void RequestRecieved(HttpServer s, RequestHandler h);
        public event RequestRecieved OnRequestRecieved;
        public delegate void MethodRunner(HttpServer s, RequestHandler h);

        public int Port { get; private set; }
        TcpListener listener;
        Thread listenThread;
        bool running = false;
        public string WwwDir { get; set; } = Environment.CurrentDirectory;
        public bool Active { get; private set; } = false;
        public bool Listen { get; private set; } = false;
        Dictionary<string, MethodRunner> methods = new Dictionary<string, MethodRunner>();
        Stopwatch stopwatch;
        public int ReadTimeout { get; set; } = 30000; 

        public HttpServer(int port)
            : this(IPAddress.Any, port)
        {
            
        }

        public HttpServer(IPAddress address, int port)
        {
            Util.Init();
            this.Port = port;
            this.listener = new TcpListener(address, this.Port);
            this.listenThread = new Thread(listen);
        }

        public void Start()
        {
            running = true;
            listener.Start();
            listenThread.Start();
            Log.WriteLine(LType.Important, "Server Started");
            Active = true;
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        void listen(object obj)
        {
            while(running)
            {
                try
                {
                    Listen = true;
                    TcpClient client = listener.AcceptTcpClient();
                    if (stopwatch == null || stopwatch.ElapsedMilliseconds < 500)
                        client.Close();
                    else
                        new ClientConnection(this, client);
                }
                catch(SocketException ex)
                {
                    if(ex.SocketErrorCode != SocketError.Interrupted)
                    {
                        throw ex;
                    }
                }
            }
            Log.WriteLine(LType.Important, "Server Stopped");
            Active = false;
            Listen = false;
        }

        public void Handle(RequestHandler req)
        {
            OnRequestRecieved?.Invoke(this, req);
        }

        public void Stop()
        {
            Log.WriteLine(LType.Important, "Shutting down");
            running = false;
            listener.Stop();
            stopwatch.Stop();
            stopwatch.Reset();
        }

        public void RegisterMethod(string name, MethodRunner method)
        {
            if (!methods.ContainsKey(name))
                methods.Add(name, method);
            else
                methods[name] = method;
        }

        public MethodRunner GetMethod(string name)
        {
            if (methods.ContainsKey(name))
                return methods[name];
            return null;
        }

        public TcpListener GetListener()
        {
            return listener;
        }
    }
}
