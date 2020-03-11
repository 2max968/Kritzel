using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeGL
{
    public class Util
    {
        public static int LoadTexture(Bitmap bmp)
        {
            BitmapData dat = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr rgba = Marshal.AllocHGlobal(4 * bmp.Width * bmp.Height);
            NativeGL.nglARGBtoRGBA(rgba, dat.Scan0, bmp.Width * bmp.Height);
            int tex = LoadTexture(rgba, bmp.Width, bmp.Height);
            bmp.UnlockBits(dat);
            Marshal.FreeHGlobal(rgba);
            return tex;
        }

        public static int LoadTexture(IntPtr data, int w, int h)
        {
            int tex = 0;
            Opengl32.glGenTextures(1, ref tex);
            Opengl32.glBindTexture(GLConsts.GL_TEXTURE_2D, tex);
            Opengl32.glTexImage2D(GLConsts.GL_TEXTURE_2D, 0, GLConsts.GL_RGBA, w, h,
                0, GLConsts.GL_RGBA, GLConsts.GL_UNSIGNED_BYTE, data);
            Opengl32.glTexParameteri(GLConsts.GL_TEXTURE_2D, GLConsts.GL_TEXTURE_MAG_FILTER, GLConsts.GL_LINEAR);
            Opengl32.glTexParameteri(GLConsts.GL_TEXTURE_2D, GLConsts.GL_TEXTURE_MIN_FILTER, GLConsts.GL_LINEAR);
            return tex;
        }

        public static void LoadMatrix3x3(float[] m)
        {
            Opengl32.glMatrixMode(GLConsts.GL_MODELVIEW);
            float[] m4x4 = new float[]
            {
                m[0], m[1], 0, m[2],
                m[3], m[4], 0, m[5],
                0, 0, 1, 0,
                m[6], m[7], 0, m[8]
            };
            Opengl32.glLoadMatrixf(m4x4);
        }

        public static void Leackage(IDisposable disp)
        {
            Console.WriteLine("[Memory Leak] Object of type '{0}' is still alive.", disp.GetType().Name);
        }
    }
}
