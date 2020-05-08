using Ionic.Zip;
using Kritzel.HTTPServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel.WebUI
{
    public partial class WebDialog : Form
    {
        public static float ScaleFactor { get; set; } = 1;

        HttpServer server;
        int port;
        ZipFile content, global;
        delegate void closeDialog(DialogResult res);
        public Dictionary<string, string> Vars = new Dictionary<string, string>();
        public Dictionary<string, object> Inputs = new Dictionary<string, object>();
        Stopwatch stp = new Stopwatch();
        long lastDeactivate = 0;

        public object this[string varname]
        {
            set
            {
                if (Inputs.ContainsKey(varname))
                    Inputs[varname] = value;
                else
                    Inputs.Add(varname, value);
            }
        }

        public string this[string varname, string defaultVal]
        {
            get
            {
                if (Vars.ContainsKey(varname))
                    return Vars[varname];
                else
                    return defaultVal;
            }
        }

        public WebDialog(string filename)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;

            content = new ZipFile(filename);
            global = new ZipFile("Interface/global.zip");

            stp.Start();
            Random rnd = new Random();
            int portMin = 49152;
            int portMax = 65535;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1) int.TryParse(args[1], out portMin);
            if (args.Length > 2) int.TryParse(args[2], out portMax);
            do
            {
                port = rnd.Next(portMin, portMax);
                server = new HttpServer(IPAddress.Loopback, port);
                try
                {
                    server.Start();
                }
                catch { }
            }
            while (!server.Active);
            server.ReadTimeout = 1000;
            Console.WriteLine("Server active on Port {0}", port);
            this.FormClosing += WebDialog_FormClosing;
            Log.SetConsoleHandler();

            server.RegisterMethod("cancel", reqCancel);
            server.RegisterMethod("ok", reqOK);
            server.OnRequestRecieved += Server_OnRequestRecieved;

            string exePath = Path.GetDirectoryName(Application.ExecutablePath);

            web.Navigated += WebBrowser1_Navigated;
            web.Navigate(new Uri("http://localhost:" + port + "/index.html"));

            try
            {
                ZipEntry entry = content.SelectEntries("dialog.ini", "").First();
                var stream = entry.OpenReader();
                StreamReader reader = new StreamReader(stream);
                
                while(!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    int sep = line.IndexOf(':');
                    if(sep > 0)
                    {
                        string key = line.Substring(0, sep).Trim();
                        string value = line.Substring(sep + 1).Trim();
                        if (key == "maximizebox")
                            this.MaximizeBox = value == "true";
                        if (key == "minimizebox")
                            this.MinimizeBox = value == "true";
                        if(key == "width")
                        {
                            int w;
                            int.TryParse(value, out w);
                            this.Width = (int)(w * ScaleFactor);
                        }
                        if(key == "height")
                        {
                            int h;
                            int.TryParse(value, out h);
                            this.Height = (int)(h * ScaleFactor);
                        }
                        if (key == "resizable")
                            this.FormBorderStyle = value == "true" ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;
                    }
                }
            }
            catch
            {

            }

            try
            {
                ZipEntry docEntry = content.SelectEntries("index.html", "").First();
                var docStream = docEntry.OpenReader();
                var docReader = new StreamReader(docStream);
                string document = docReader.ReadToEnd();
                docReader.Close();
                docStream.Close();

                string title = DocumentParser.GetTitle(document);
                this.Text = title;
            }
            catch
            {

            }
        }

        private void Server_OnRequestRecieved(HttpServer s, RequestHandler h)
        {
            string name = h.FileName.TrimStart('/');
                ZipFile file = content;
                if (name.StartsWith("global/"))
                {
                    name = name.Substring(7);
                    file = global;
                }
            try
            {
                ZipEntry entry = file.SelectEntries(name, "").First();
                if (entry != null)
                {
                    MemoryStream ms = new MemoryStream((int)entry.UncompressedSize);
                    entry.Extract(ms);
                    h.Response = new HttpResponseStream(s, ms);
                    h.Response.Head.ContentType = MimeType.GetMime(h.FileInfo.Extension);
                    h.Handled = true;
                }
            }
            catch (Exception)
            {

            }
        }

        private void WebBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (web.DocumentTitle.Length > 0)
            {
#if DEBUG
                this.Text = $"{server.Port}: " + web.DocumentTitle;
#else
                this.Text = web.DocumentTitle;
#endif
            }
            if(web.Document.Url.Host.EndsWith(".dll"))
            {
                web.Navigate(web.Url);
            }
            else
            {
                foreach (var input in Inputs)
                    web.Document.InvokeScript("eval", new object[] { $"var {input.Key} = {ObjectConverter.ToJavascriptObject(input.Value)}" });
                
            }
        }

        private void WebDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Stop();
            content.Dispose();
            global.Dispose();
        }

        void reqCancel(HttpServer s, RequestHandler h)
        {
            h.Response = new HttpResponseString(s, "Closing Dialog");
            this.Invoke(new closeDialog(_closeDialog), DialogResult.Cancel);
        }

        void reqOK(HttpServer s, RequestHandler h)
        {
            h.Response = new HttpResponseString(s, "Closing Dialog");
            Vars = h.Head.Get;
            this.Invoke(new closeDialog(_closeDialog), DialogResult.OK);
        }

        void _closeDialog(DialogResult res)
        {
            this.DialogResult = res;
            this.Close();
        }

        private void WebDialog_Resize(object sender, EventArgs e)
        {
#if DEBUG
            ttSize.Show($"Size: ({this.ClientSize.Width}, {this.ClientSize.Height})", this);
#endif
        }

        protected override void WndProc(ref Message m)
        {
            const UInt32 WM_NCACTIVATE = 0x0086;
            if (m.Msg == WM_NCACTIVATE && m.WParam.ToInt32() == 0)
            {
                long millis = stp.ElapsedMilliseconds;
                long delta = millis - lastDeactivate;
                lastDeactivate = millis;
                if (delta > 0 && delta < 500) this.Close();
            }
            base.WndProc(ref m);
        }
    }
}
