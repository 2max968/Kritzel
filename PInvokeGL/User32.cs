using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeGL
{
    public class User32
    {
        public const string LIB = "user32.dll";

        [DllImport(LIB)]
        public static extern IntPtr GetDC(IntPtr hwnd);
    }
}
