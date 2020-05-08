using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public static class Util
    {
        static bool initialized = false;

        public static void Init()
        {
            if(!initialized)
            {
                initialized = true;
                HttpStatusCode.Init();
                MimeType.Init();
            }
        }

        public static void WriteS(this Stream stream, string txt)
        {
            byte[] bytes = Encoding.Default.GetBytes(txt);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteS(this Stream stream, string format, params object[] args)
        {
            WriteS(stream, string.Format(format, args));
        }

        public static void Add<tKey,tVal>(this List<KeyValuePair<tKey,tVal>> list, tKey key, tVal val)
        {
            KeyValuePair<tKey, tVal> kvp = new KeyValuePair<tKey, tVal>(key, val);
            list.Add(kvp);
        }
    }
}
