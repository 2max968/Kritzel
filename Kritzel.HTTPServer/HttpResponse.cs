using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public abstract class HttpResponse
    {
        HttpServer server;
        public HttpResponseHead Head { get; set; }

        public HttpResponse(HttpServer server)
        {
            this.server = server;
            Head = new HttpResponseHead();
        }

        public void Write(Stream stream)
        {
            if(Head != null)
                Head.Write(stream, GetLength());
            stream.WriteS("\r\n");
            WriteContent(stream);
        }

        public string GetString()
        {
            byte[] buffer = new byte[2048];
            MemoryStream ms = new MemoryStream(buffer);
            return Encoding.Default.GetString(buffer);
        }

        public abstract int GetLength();
        public abstract void WriteContent(Stream stream);
    }

    public class HttpResponseString : HttpResponse
    {
        byte[] buffer;
        public HttpResponseString(HttpServer server, string content)
            : base(server)
        {
            buffer = Encoding.Default.GetBytes(content);
            Head.ContentType = "text/plain";
        }

        public override int GetLength()
        {
            return buffer.Length;
        }

        public override void WriteContent(Stream stream)
        {
            stream.Write(buffer, 0, buffer.Length);
        }
    }

    public class HttpResponseEmpty : HttpResponse
    {
        public HttpResponseEmpty(HttpServer server, int code)
            :base(server)
        {
            Head.StatusCode = code;
        }

        public override int GetLength()
        {
            return 0;
        }

        public override void WriteContent(Stream stream)
        {
            
        }
    }

    public class HttpResponseMessage : HttpResponse
    {
        byte[] buffer;

        public HttpResponseMessage(HttpServer server, int code)
            : base(server)
        {
            string txt = "" + code + " " + HttpStatusCode.GetName(code);
            buffer = Encoding.Default.GetBytes(txt);
            Head.StatusCode = code;
        }

        public override int GetLength()
        {
            return buffer.Length;
        }

        public override void WriteContent(Stream stream)
        {
            stream.Write(buffer, 0, buffer.Length);
        }
    }

    public class HttpResponseFile : HttpResponse
    {
        FileInfo info;
        bool blocked = false;
        byte[] errorBuffer;

        public HttpResponseFile(HttpServer server, string path)
            :base(server)
        {
            info = new FileInfo(path);
            if (!BlockedDirs.IsBlocked(info))
            {
                Head.ContentType = MimeType.GetMime(info.Extension);

                Log.WriteLine(LType.IO, "Request File: {1}: \"{0}\"",
                    info.FullName, Head.ContentType);
            }
            else
            {
                Head.ContentType = "text/plain";
                Head.StatusCode = 404;
                blocked = true;
                errorBuffer = Encoding.UTF8.GetBytes("404 Not Found");
                Log.WriteLine(LType.IO, "Blocked File: {1}: \"{0}\"",
                    info.FullName, Head.ContentType);
            }
        }

        public override int GetLength()
        {
            if (blocked) return errorBuffer.Length;
            if (info == null) return 0;
            return (int)info.Length;
        }

        public override void WriteContent(Stream stream)
        {
            if(blocked)
            {
                stream.Write(errorBuffer, 0, errorBuffer.Length);
                return;
            }
            FileStream fs = info.OpenRead();
            byte[] buffer = new byte[1024];
            int l;
            while((l = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                stream.Write(buffer, 0, l);
            }
            fs.Close();
            fs.Dispose();
        }
    }

    public class HttpResponseRedirect : HttpResponseMessage
    {
        public HttpResponseRedirect(HttpServer server, string location)
            :base(server, 307)
        {
            Head.Location = location;
        }
    }

    public class HttpResponseBitmap : HttpResponse
    {
        Bitmap bmp;
        MemoryStream stream;

        public HttpResponseBitmap(HttpServer server, Bitmap bmp)
            : base(server)
        {
            this.bmp = bmp;
            this.Head.ContentType = "image/jpeg";
            stream = new MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public override int GetLength()
        {
            return (int)stream.Position;
        }

        public override void WriteContent(Stream stream)
        {
            this.stream.WriteTo(stream);
            this.stream.Close();
            this.stream.Dispose();
        }
    }

    public class HttpResponsePNG : HttpResponse
    {
        Bitmap bmp;
        MemoryStream stream;

        public HttpResponsePNG(HttpServer server, Bitmap bmp)
            : base(server)
        {
            this.bmp = bmp;
            this.Head.ContentType = "image/png";
            stream = new MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        }

        public override int GetLength()
        {
            return (int)stream.Position;
        }

        public override void WriteContent(Stream stream)
        {
            this.stream.WriteTo(stream);
            this.stream.Close();
            this.stream.Dispose();
        }
    }

    public class HttpResponseStream : HttpResponse
    {
        MemoryStream stream;

        public HttpResponseStream(HttpServer server, MemoryStream stream)
            : base(server)
        {
            this.Head.ContentType = "image/png";
            this.stream = stream;
        }

        public override int GetLength()
        {
            return (int)this.stream.Length;
        }

        public override void WriteContent(Stream stream)
        {
            this.stream.WriteTo(stream);
            this.stream.Close();
            this.stream.Dispose();
        }
    }
}
