using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeGL
{
    public class NativeGL
    {
        public const string LIB = "nativegl.dll";
        public const CallingConvention CDECL = CallingConvention.Cdecl;
        public const int SHADER_NOERROR = 0;
        public const int SHADER_ERROR_VERTEX = 1;
        public const int SHADER_ERROR_FRAGMENT = 2;
        public const int SHADER_ERROR_LINK = 3;

        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern int nglInit();
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern uint nglCreateFBO(int tex, int width, int height);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglBindFramebuffer(uint fbo);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglBindFramebufferExt(int target, uint fbo);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglDeleteFramebuffer(uint fbo);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglVertex2f(int n, float[] x, float[] y);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglVertexf(int n, float[] v);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglARGBtoRGBA(IntPtr dst, IntPtr src, int pixels);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern uint nglCreateFBOMultisampling(ref int tex, int w, int h, int samples);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglBlitFBO(uint fbo_read, uint fbo_draw, int width, int height);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglProjection(int w, int h);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglVertex2fTrans(int n, float[] x, float[] y, float dx, float dy, float fx, float fy);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern int nglCreateProgram(ref uint prog, string vertex, string fragment);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglUseProgram(uint prog);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern void nglLineWidth(float width);
        [DllImport(LIB, CallingConvention = CDECL)]
        public static extern IntPtr nglGetInfoLog(ref int len);
    }
}
