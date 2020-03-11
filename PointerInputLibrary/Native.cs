using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PointerInputLibrary
{
    public class Native
    {
        public enum TWF
        {
            FINETOUCH = 1,
            WANTPALM = 2
        }

        [DllImport("User32.dll")]
        public static extern bool RegisterTouchWindow(IntPtr hWnd, int uFlags);
    }
}
