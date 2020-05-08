#if SLIMDX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct2D;
using gdi = System.Drawing;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;
using SlimDX;
using gdi2d = System.Drawing.Drawing2D;
using text = SlimDX.DirectWrite;

namespace Kritzel.Main.Renderer
{
    public class GPURenderer1 : GPURenderer
    {
        public static RenderTarget RenderTarget { get; private set; } = null;
        static List<GPURenderer> refTrack = new List<GPURenderer>();
        static SolidColorBrush brush_gray = null;

        Dictionary<int, Brush> brushes = new Dictionary<int, Brush>();
        Factory factory;
        text.Factory writeFactory;
        RenderTarget renderTarget;
        WindowRenderTarget mainRenderTarget;
        System.Windows.Forms.Control cltr;
        bool stateDraw = false;
        bool tLock = false;

        public GPURenderer1(System.Windows.Forms.Control cltr)
        {
            IsHardware = true;
            refTrack.Add(this);
            float scale = GetScaleFactor();
            this.cltr = cltr;
            factory = new Factory(FactoryType.Multithreaded, DebugLevel.Error);
            mainRenderTarget = new WindowRenderTarget(factory, new WindowRenderTargetProperties()
            {
                Handle = cltr.Handle,
                PixelSize = cltr.Size,
                PresentOptions = PresentOptions.Immediately
            });
            RenderTarget = mainRenderTarget;
            Width = cltr.Width;
            Height = cltr.Height;
            mainRenderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            mainRenderTarget.DotsPerInch = new gdi.SizeF(
                mainRenderTarget.DotsPerInch.Width / scale, mainRenderTarget.DotsPerInch.Height / scale);

            writeFactory = new text.Factory();

            if (brush_gray == null)
                brush_gray = new SolidColorBrush(mainRenderTarget, new Color4(gdi.Color.Gray));

            renderTarget = mainRenderTarget;
        }

        public override void Dispose()
        {
            if (Disposed) return;
            refTrack.Remove(this);
            Disposed = true;
            mainRenderTarget.Dispose();
            writeFactory.Dispose();
            factory.Dispose();
            foreach (var b in brushes)
                b.Value.Dispose();
            if (refTrack.Count == 0)
                DisposeStatic();
        }

        public static void DisposeStatic()
        {
            brush_gray.Dispose();
        }

        public override void Resize(gdi.Size size)
        {
            if (stateDraw) return;
            mainRenderTarget.Resize(size);
            this.Width = size.Width;
            this.Height = size.Height;
        }

        public override void DrawLine(gdi.Color c, float width, PointF p1, PointF p2, bool capStart = false, bool capEnd = false)
        {
            using (Brush b = new SolidColorBrush(renderTarget, new SlimDX.Color4(c)))
            {
                CapStyle start = capStart ? CapStyle.Triangle : CapStyle.Flat;
                CapStyle end = capEnd ? CapStyle.Triangle : CapStyle.Flat;
                StrokeStyle style = new StrokeStyle(factory, new StrokeStyleProperties()
                {
                    StartCap = start,
                    EndCap = end
                });
                renderTarget.DrawLine(b, p1, p2, width, style);
                style.Dispose();
            }
        }

        public override void FillEllipse(PBrush c, RectangleF rect)
        {
            // TODO: Brush
            Brush b = CreateBrush(c);
            Ellipse ell = new Ellipse();
            ell.Center = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            ell.RadiusX = rect.Width / 2;
            ell.RadiusY = rect.Height / 2;
            renderTarget.FillEllipse(b, ell);
        }

        public override void DrawEllipse(PBrush c, float width, RectangleF rect)
        {
            using (Brush b = CreateBrush(c))
            {
                Ellipse ell = new Ellipse();
                ell.Center = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                ell.RadiusX = rect.Width / 2;
                ell.RadiusY = rect.Height / 2;
                renderTarget.DrawEllipse(b, ell, width);
            }
        }

        public override void DrawRoundedLine(PBrush c, float width, PointF p1, PointF p2)
        {
            using (Brush b = CreateBrush(c))
            {
                StrokeStyle style = new StrokeStyle(factory, new StrokeStyleProperties()
                {
                    StartCap = CapStyle.Round,
                    EndCap = CapStyle.Round
                });
                renderTarget.DrawLine(b, p1, p2, width, style);
                style.Dispose();
            }
        }

        public override void DrawRoundedRectangle(PBrush c, float width, RectangleF rect)
        {
            using (Brush b = CreateBrush(c))
            {
                RoundedRectangle rr = new RoundedRectangle();
                rr.Left = rect.Left;
                rr.Right = rect.Right;
                rr.Top = rect.Top;
                rr.Bottom = rect.Bottom;
                rr.RadiusX = width / 2;
                rr.RadiusY = width / 2;
                renderTarget.DrawRoundedRectangle(b, rr, width);
            }
        }

        public override void DrawRect(gdi.Color c, float width, RectangleF rect)
        {
            using (Brush b = new SolidColorBrush(renderTarget, new SlimDX.Color4(c)))
            {
                renderTarget.DrawRectangle(b, rect, width);
            }
        }

        ~GPURenderer1()
        {
            Dispose();
        }

        public Brush CreateBrush(PBrush pBrush)
        {
            int hash = pBrush.GetHashCode();
            if (brushes.ContainsKey(hash) && !brushes[hash].Disposed)
                return brushes[hash];
            if (pBrush.SType() == PBrush.B_SOLID)
            {
                gdi.Color c = pBrush.GetColors()[0];
                brushes[hash] = new SolidColorBrush(mainRenderTarget, new Color4(c));
            }
            else if (pBrush.SType() == PBrush.B_LINEAR)
            {
                gdi.Color[] colors = pBrush.GetColors();
                float[] nums = pBrush.GetNums();
                if (colors.Length >= 2 && nums.Length >= 4)
                {
                    GradientStop p1 = new GradientStop()
                    {
                        Position = 0,
                        Color = new Color4(colors[0])
                    };
                    GradientStop p2 = new GradientStop()
                    {
                        Position = 1,
                        Color = new Color4(colors[1])
                    };
                    using (GradientStopCollection stops = new GradientStopCollection(mainRenderTarget,
                        new GradientStop[] { p1, p2 }, Gamma.Linear, ExtendMode.Mirror))
                    {
                        LinearGradientBrushProperties props = new LinearGradientBrushProperties()
                        {
                            StartPoint = new PointF(nums[0], nums[1]),
                            EndPoint = new PointF(nums[2], nums[3])
                        };
                        brushes[hash] = new LinearGradientBrush(mainRenderTarget, stops, props);
                    }
                }
            }

            if (!brushes.ContainsKey(hash))
                brushes[hash] = new SolidColorBrush(mainRenderTarget, new Color4(1, 0, 1));
            return brushes[hash];
        }

        public override void ResetTransform()
        {
            renderTarget.Transform = Matrix3x2.Identity;
        }

        public override void Transform(gdi2d.Matrix m)
        {
            Matrix3x2 mat = new Matrix3x2();
            mat.M11 = m.Elements[0];
            mat.M12 = m.Elements[1];
            mat.M21 = m.Elements[2];
            mat.M22 = m.Elements[3];
            mat.M31 = m.Elements[4];
            mat.M32 = m.Elements[5];
            renderTarget.Transform *= mat;
        }

        public override bool Begin()
        {
            if (stateDraw) return false;
            while (tLock) ;
            stateDraw = true;
            mainRenderTarget.BeginDraw();
            return true;
        }

        public override bool Begin(gdi.Color color)
        {
            if (stateDraw) return false;
            while (tLock) ;
            stateDraw = true;
            mainRenderTarget.BeginDraw();
            mainRenderTarget.Clear(new Color4(color));
            return true;
        }

        public override void End()
        {
            if (!stateDraw) return;
            mainRenderTarget.EndDraw();
            stateDraw = false;
        }

        public override void End(RectangleF rect)
        {
            if (!stateDraw) return;
            mainRenderTarget.EndDraw();
            stateDraw = false;
        }

        public override void FillRectangle(gdi.Color c, RectangleF rect)
        {
            using (Brush b = new SolidColorBrush(renderTarget, c))
            {
                renderTarget.FillRectangle(b, rect);
            }
        }

        public override void EditPage()
        {
            //tLock = true;
            //while (stateDraw) ;
        }

        public override void EndEditPage()
        {
            //tLock = false;
        }

        public override void DrawImage(Image img, RectangleF rect)
        {
            if (img.GPUBitmap == null)
            {
                img.LoadGPU(RenderTarget);
            }
            renderTarget.DrawBitmap(img.GPUBitmap, rect);
        }

        public override void DrawDashPolygon(PointF[] pts)
        {
            if (pts.Length == 0) return;
            PathGeometry geo = new PathGeometry(factory);
            var sink = geo.Open();
            sink.BeginFigure(pts[0], FigureBegin.Filled);
            sink.AddLines(pts);
            sink.EndFigure(FigureEnd.Open);
            sink.Close();
            StrokeStyle style = new StrokeStyle(factory, new StrokeStyleProperties()
            {
                DashStyle = DashStyle.Dash
            });
            renderTarget.DrawGeometry(geo, brush_gray, 4, style);
            renderTarget.DrawLine(brush_gray, pts[0], pts[pts.Length - 1], 2, style);
            style.Dispose();
            sink.Dispose();
            geo.Dispose();
        }

        public override void FillPolygon(PBrush b, PointF[] pts)
        {
            if (pts.Length == 0) return;
            using (Brush brush = CreateBrush(b))
            {
                PathGeometry geo = new PathGeometry(factory);
                var sink = geo.Open();
                sink.BeginFigure(pts[0], FigureBegin.Filled);
                sink.AddLines(pts);
                sink.EndFigure(FigureEnd.Closed);
                sink.Close();
                renderTarget.FillGeometry(geo, brush);
                sink.Dispose();
                geo.Dispose();
            }
        }

        public override void DrawText(string str, PBrush brush, RectangleF rect, float size)
        {
            float sizept = Util.MmToPoint(size);
            float px = Util.MmToPoint(rect.X);
            float py = Util.MmToPoint(rect.Y);
            float pw = Util.MmToPoint(rect.Width);
            float ph = Util.MmToPoint(rect.Height);
            RectangleF rectpt = new RectangleF(px, py, pw, ph);

            text.TextFormat format = new text.TextFormat(writeFactory, "Arial", text.FontWeight.Normal,
                text.FontStyle.Normal, text.FontStretch.Normal, sizept, "Arial" + sizept + "pt");
            renderTarget.DrawText(str, format, rectpt, CreateBrush(brush));
            format.Dispose();
        }

        public static explicit operator RenderTarget(GPURenderer1 r)
        {
            return r.mainRenderTarget;
        }

        public override RenderBitmap CreateRenderTarget()
        {
            return new RenderBitmap1(renderTarget);
        }

        public override void SetRenderTarget(RenderBitmap bmp)
        {
            if (bmp == null || !(bmp is RenderBitmap1))
                renderTarget = mainRenderTarget;
            else
                renderTarget = ((RenderBitmap1)bmp).GetRenderTarget();
        }

        public override void DrawRenderBitmap(RenderBitmap bmp)
        {
            if (!(bmp is RenderBitmap1))
                return;
            renderTarget.DrawBitmap(((RenderBitmap1)bmp).GetBitmap());
        }

        public override float GetScaleFactor()
        {
            return Util.GetScaleFactor();
        }
    }
}
#endif