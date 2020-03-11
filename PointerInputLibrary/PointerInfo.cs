using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PointerInputLibrary
{
    public enum PenFlags
    {
        NONE = 0,
        BARREL = 1,
        INVERTED = 2,
        ERASER = 4
    }

    public class PointerInfo
    { 
        [DllImport("StylusLib.dll")]
        public static extern void SomeFunction();

        [DllImport("user32.dll")]
        public static extern bool GetPointerPenInfo(UInt32 id, out POINTER_PEN_INFO info);
        
        [StructLayout(LayoutKind.Explicit)]
        public struct POINTER_PEN_INFO
        {
            [FieldOffset(96)]  public uint x86pressure;
            [FieldOffset(104)] public uint x64pressure;
            [FieldOffset(88)] public PenFlags x86penFlags;
            [FieldOffset(96)] public PenFlags x64penFlags;
            [FieldOffset(200)]
            private int nil;
        }
    }
}
