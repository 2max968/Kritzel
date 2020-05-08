using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public enum LType { Std, NetInfo, Error, HttpInfo, Warning, Important, Script, IO};

    public class Log
    {
        public delegate void WriteEvent(LType type, string text);
        public static event WriteEvent OnWrite;
        public static FileStream OutputStream = null;

        public static bool HideNet = false;

        public static void Write(LType type, string txt)
        {
            OnWrite?.Invoke(type, txt);
            if(type == LType.HttpInfo)
                OutputStream?.WriteS("[{2}][{0}] {1}", type.ToString(), txt, DateTime.Now.ToLongTimeString());
        }

        public static void Write(LType type, string format, params object[] args)
        {
            Write(type, string.Format(format, args));
        }

        public static void WriteLine(LType type, string txt)
        {
            Write(type, txt + Environment.NewLine);
        }

        public static void WriteLine(LType type, string format, params object[] args)
        {
            Write(type, string.Format(format, args) + Environment.NewLine);
        }

        public static void SetConsoleHandler()
        {
            OnWrite += Log_OnWrite;
        }

        private static void Log_OnWrite(LType type, string text)
        {
            string pfix = "";
            Console.ResetColor();
            ConsoleColor color = Console.ForegroundColor;
            switch (type)
            {
                case LType.NetInfo:
                    if (HideNet) return;
                    color = ConsoleColor.DarkGray;
                    pfix = "[NetInfo]\t";
                    break;
                case LType.HttpInfo:
                    color = ConsoleColor.White;
                    pfix = "[Http]\t";
                    break;
                case LType.Warning:
                    color = ConsoleColor.DarkYellow;
                    pfix = "[Warning]\t";
                    break;
                case LType.Error:
                    color = ConsoleColor.DarkRed;
                    pfix = "[Error]\t";
                    break;
                case LType.Important:
                    color = ConsoleColor.DarkCyan;
                    break;
                case LType.IO:
                    color = ConsoleColor.DarkYellow;
                    pfix = "[IO]\t\t";
                    break;
            }
            Console.Write("[{0}]", DateTime.Now.ToLongTimeString());
            Console.ForegroundColor = color;
            Console.Write("{0}{1}", pfix, text);
            Console.ResetColor();
        }
    }
}
