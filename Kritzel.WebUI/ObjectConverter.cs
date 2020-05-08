using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.WebUI
{
    public class ObjectConverter
    {
        public static CultureInfo culture = null;

        public static string ToJavascriptObject(object obj)
        {
            if (culture == null)
                culture = CultureInfo.GetCultureInfo("en");

            if (obj.GetType().IsArray)
            {
                string text = "[";
                Array a = (Array)obj;
                for (int i = 0; i < a.Length; i++)
                {
                    if (i < a.Length - 1)
                        text += ToJavascriptObject(a.GetValue(i)) + ",";
                    else
                        text += ToJavascriptObject(a.GetValue(i));
                }
                return text + "]";
            }
            else
            {
                if (obj is int || obj is short || obj is long)
                {
                    long i = (long)obj;
                    return i.ToString(culture);
                }
                else if (obj is uint || obj is ushort || obj is ulong)
                {
                    ulong i = (ulong)obj;
                    return i.ToString(culture);
                }
                else if (obj is float || obj is double)
                {
                    double f = (double)obj;
                    return f.ToString(culture);
                }
                else if (obj is char)
                {
                    return $"'{obj}'";
                }
                else if(obj is Color)
                {
                    Color c = (Color)obj;
                    return $"{{'r':{c.R}, 'g': {c.G}, 'b': {c.B}}}";
                }
                else
                {
                    return $"'{obj}'";
                }
            }
        }
    }
}
