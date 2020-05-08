using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdi = System.Drawing;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;
using gdi2d = System.Drawing.Drawing2D;

namespace Kritzel.Main.Renderer
{
    public abstract class GPURenderer : BaseRenderer, IDisposable
    {
        public bool Disposed { get; protected set; } = false;
        public float Width { get; protected set; }
        public float Height { get; protected set; }
        public bool IsHardware { get; protected set; }
        public bool Drawing { get; protected set; } = false;

        public abstract void Resize(gdi.Size size);
        public abstract void ResetTransform();
        public abstract void Transform(gdi2d.Matrix m);
        public abstract bool Begin();
        public abstract bool Begin(gdi.Color c);
        public abstract void End();
        public abstract void End(RectangleF rect);
        public abstract void FillRectangle(gdi.Color c, gdi.RectangleF rect);
        public abstract void EditPage();
        public abstract void EndEditPage();
        public abstract void DrawDashPolygon(PointF[] pts);
        public abstract RenderBitmap CreateRenderTarget();
        public abstract void SetRenderTarget(RenderBitmap bmp);
        public abstract void DrawRenderBitmap(RenderBitmap bmp);
        public abstract float GetScaleFactor();
        public abstract void DrawText(string text, PointF pos, float size, gdi.Color c);
        public virtual void Init() { }

        public abstract void Dispose();

        public static GPURenderer Create(System.Windows.Forms.Control cltr)
        {
            return Create(cltr, true);
        }

        public static GPURenderer Create(System.Windows.Forms.Control cltr, bool tryHardware)
        {
#if SLIMDX
            GPURenderer r = null;
            try
            {
                if (!tryHardware)
                    r = new GPURenderer2(cltr);
                else
                    r = new GPURenderer1(cltr);
            }
            catch(Exception e)
            {
                Console.WriteLine("Cant create Renderer\n" + e.Message);
                r?.Dispose();
                r = new GPURenderer2(cltr);
            }
            return r;
#else
            if (tryHardware)
                return new GPURenderer3(cltr);
            return new GPURenderer2(cltr);
#endif
        }
    }
}
