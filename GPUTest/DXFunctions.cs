using SlimDX.Direct2D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPUTest
{
    public class DXFunctions : IDisposable
    {
        Factory factory;
        WindowRenderTarget renderTarget;
        public bool Disposed { get; private set; } = false;

        public DXFunctions(Control cltr)
        {
            try
            {
                factory = new Factory(FactoryType.Multithreaded, DebugLevel.None);
                renderTarget = new WindowRenderTarget(factory, new WindowRenderTargetProperties()
                {
                    Handle = cltr.Handle,
                    PixelSize = cltr.Size,
                    PresentOptions = PresentOptions.Immediately
                });
                renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + ":\n" + e.StackTrace.ToString());
                throw e;
            }
        }

        ~DXFunctions()
        {
            if (!Disposed)
                Dispose();
        }

        public void Begin()
        {
            try
            {
                renderTarget.BeginDraw();
                renderTarget.Clear(new SlimDX.Color4(1, 1, 1));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ":\n" + e.StackTrace.ToString());
                throw e;
            }
        }

        public void End()
        {
            try
            {
                renderTarget.EndDraw();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ":\n" + e.StackTrace.ToString());
                throw e;
            }
        }

        public void DrawRect(System.Drawing.Color c, float width, System.Drawing.RectangleF rect)
        {
            try
            {
                Brush b = new SolidColorBrush(renderTarget, new SlimDX.Color4(c));
                renderTarget.DrawRectangle(b, rect);
                b.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ":\n" + e.StackTrace.ToString());
                throw e;
            }
        }

        public void Dispose()
        {
            try
            {
                Disposed = true;
                renderTarget.Dispose();
                factory.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ":\n" + e.StackTrace.ToString());
                throw e;
            }
        }
    }
}
