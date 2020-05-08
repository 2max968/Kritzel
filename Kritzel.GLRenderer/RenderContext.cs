using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel.GLRenderer
{
    public class RenderContext : IDisposable
    {
        public IntPtr HWND { get; private set; }
        public IntPtr HDC { get; private set; }
        public IntPtr HGLRC { get; private set; }
        public bool Disposed { get; private set; } = false;
        Rectangle clientArea;

        public RenderContext(Control control)
        {
            this.HWND = control.Handle;
            this.HDC = User32.GetDC(this.HWND);
            Structs.PIXELFORMATDESCRIPTOR pfd = Structs.PIXELFORMATDESCRIPTOR.Create();
            int iPixelFormat = Gdi32.ChoosePixelFormat(this.HDC, ref pfd);
            Gdi32.SetPixelFormat(this.HDC, iPixelFormat, ref pfd);

            Resize(control.ClientRectangle);
        }

        public void Resize(Rectangle clientArea)
        {
            this.clientArea = clientArea;
            Opengl32.glViewport(clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);
            Opengl32.glMatrixMode(GLConsts.GL_PROJECTION);
            Opengl32.glLoadIdentity();
            Opengl32.glOrtho(clientArea.Left, clientArea.Right, clientArea.Bottom, clientArea.Top, -1, 1);
            Opengl32.glMatrixMode(GLConsts.GL_MODELVIEW);
        }

        public void Resize()
        {
            Opengl32.glViewport(clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);
            Opengl32.glMatrixMode(GLConsts.GL_PROJECTION);
            Opengl32.glLoadIdentity();
            Opengl32.glOrtho(clientArea.Left, clientArea.Right, clientArea.Bottom, clientArea.Top, -1, 1);
            Opengl32.glMatrixMode(GLConsts.GL_MODELVIEW);
        }

        public void InitAsync()
        {
            this.HGLRC = Opengl32.wglCreateContext(this.HDC);
        }

        public void MakeCurrent()
        {
            Opengl32.wglMakeCurrent(this.HDC, this.HGLRC);
        }

        public void SwapBuffer()
        {
            Gdi32.SwapBuffers(this.HDC);
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Opengl32.wglDeleteContext(this.HGLRC);
        }

        ~RenderContext()
        {
            if (!Disposed)
                Util.Leackage(this);
        }
    }
}
