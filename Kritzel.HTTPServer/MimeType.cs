using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public class MimeType
    {
        public static List<MimeType> MimeTypes { get; private set; }

        public static void Init()
        {
            string txt = Properties.Resources.MimeTypes;
            MimeTypes = new List<MimeType>();
            foreach(string line in txt.Split('\n'))
            {
                string l = line.Trim('\r');
                string[] words = l.Split(' ');
                if(words.Length > 1)
                {
                    MimeType mime = new MimeType(words[0], words[1]);
                    MimeTypes.Add(mime);
                }
            }
        }

        public static string GetMime(string extension)
        {
            extension = extension.TrimEnd('.').ToLower();
            foreach(MimeType mime in MimeTypes)
                if (mime.Extension.TrimEnd('.') == extension)
                    return mime.MimeString;
            return "text/plain";
        }

        public string Extension { get; private set; }
        public string MimeString { get; private set; }

        public MimeType(string extension, string mime)
        {
            this.Extension = extension;
            this.MimeString = mime;
        }

        public override string ToString()
        {
            return Extension + ": " + MimeString;
        }
    }
}
