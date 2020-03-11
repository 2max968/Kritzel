using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeGL
{
    public class Opengl32
    {
        public const string LIB = "opengl32.dll";

        [DllImport(LIB)] public static extern IntPtr wglCreateContext(IntPtr hdc);
        [DllImport(LIB)] public static extern bool wglDeleteContext(IntPtr hglrc);
        [DllImport(LIB)] public static extern bool wglMakeCurrent(IntPtr hdc, IntPtr hglrc);
        [DllImport(LIB)] public static extern bool wglChoosePixelFormatARB(IntPtr hdc, ref int piAttribIList,
                         ref float pfAttribFList, uint nMaxFormats, ref int piFormats, ref uint nNumFormats);
        [DllImport(LIB)] public static extern void glClearColor(float red, float green, float blue, float alpha);
        [DllImport(LIB)] public static extern void glClear(uint mask);
        [DllImport(LIB)] public static extern void glColor3f(float red, float green, float blue);
        [DllImport(LIB)] public static extern void glColor4f(float red, float green, float blue, float alpha);
        [DllImport(LIB)] public static extern void glBegin(int primitiveType);
        [DllImport(LIB)] public static extern void glEnd();
        [DllImport(LIB)] public static extern void glVertex2f(float x, float y);
        [DllImport(LIB)] public static extern void glVertex3f(float x, float y, float z);
        [DllImport(LIB)] public static extern void glGenTextures(int n, int[] textures);
        [DllImport(LIB)] public static extern void glGenTextures(int n, ref int textures);
        [DllImport(LIB)] public static extern void glBindTexture(int target, int texture);
        [DllImport(LIB)]
        public static extern void glTexImage2D(int target, int level, int internalFormat,
            int widt, int height, int border, int format, int type, IntPtr pixels);
        [DllImport(LIB)] public static extern void glTexParameteri(int target, int pname, int param);
        [DllImport(LIB)] public static extern void glEnable(int cap);
        [DllImport(LIB)] public static extern void glDisable(int cap);
        [DllImport(LIB)] public static extern void glTexCoord2f(float s, float t);
        [DllImport(LIB)] public static extern void glViewport(int x, int y, int w, int h);
        [DllImport(LIB)] public static extern void glDeleteTextures(int n, ref int texture);
        [DllImport(LIB)] public static extern void glDeleteTextures(int n, int[] texture);
        [DllImport(LIB)] public static extern void glScalef(float x, float y, float z);
        [DllImport(LIB)] public static extern void glTranslatef(float x, float y, float z);
        [DllImport(LIB)] public static extern void glLoadIdentity();
        [DllImport(LIB)] public static extern void glMatrixMode(int mode);
        [DllImport(LIB)] public static extern void glLoadMatrixf(float[] m);
        [DllImport(LIB)] public static extern void glBlendFunc(int sfactor, int dfactor);
        [DllImport(LIB)] public static extern void glOrtho(double left, double right, double bottom, double top, double zNear, double zFar);
        [DllImport(LIB)] public static extern void glPushMatrix();
        [DllImport(LIB)] public static extern void glPopMatrix();
        [DllImport(LIB)] public static extern void glLineWidth(float width);
        [DllImport(LIB)] public static extern void glLineStipple(int factor, ushort pattern);
    }
}
