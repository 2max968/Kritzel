using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeGL.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PIXELFORMATDESCRIPTOR
    {
        UInt16 nSize;
        UInt16 nVersion;
        UInt32 dwFlags;
        byte iPixelType;
        byte cColorBits;
        byte cRedBits;
        byte cRedShift;
        byte cGreenBits;
        byte cGreenShift;
        byte cBlueBits;
        byte cBlueShift;
        byte cAlphaBits;
        byte cAlphaShift;
        byte cAccumBits;
        byte cAccumRedBits;
        byte cAccumGreenBits;
        byte cAccumBlueBits;
        byte cAccumAlphaBits;
        byte cDepthBits;
        byte cStencilBits;
        byte cAuxBuffers;
        byte iLayerType;
        byte bReserved;
        UInt32 dwLayerMask;
        UInt32 dwVisibleMask;
        UInt32 dwDamageMask;

        public static PIXELFORMATDESCRIPTOR Create()
        {
            PIXELFORMATDESCRIPTOR pfd = new PIXELFORMATDESCRIPTOR();
            pfd.nSize = (UInt16)Marshal.SizeOf(typeof(PIXELFORMATDESCRIPTOR));
            pfd.nVersion = 1;
            pfd.dwFlags = GdiConsts.PFD_DRAW_TO_WINDOW | GdiConsts.PFD_SUPPORT_OPENGL | GdiConsts.PFD_DOUBLEBUFFER;
            pfd.iPixelType = GdiConsts.PFD_TYPE_RGBA;
            pfd.cColorBits = 24;
            pfd.cDepthBits = 32;
            pfd.iLayerType = GdiConsts.PFD_MAIN_PLANE;
            return pfd;
        }
    }
}