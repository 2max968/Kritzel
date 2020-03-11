using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeGL
{
    public class Gdi32
    {
        public const string LIB = "gdi32.dll";

        [DllImport(LIB)]
        public static extern bool SwapBuffers(IntPtr hdc);
        [DllImport(LIB)]
        public static extern int ChoosePixelFormat(IntPtr hdc, ref Structs.PIXELFORMATDESCRIPTOR pfd);
        [DllImport(LIB)]
        public static extern bool SetPixelFormat(IntPtr hdc, int pixelFormat, ref Structs.PIXELFORMATDESCRIPTOR pfd);

    }
}
