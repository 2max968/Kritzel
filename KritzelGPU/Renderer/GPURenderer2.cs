using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Kritzel.Renderer
{
    public class GPURenderer2 : GPURenderer
    {
        Bitmap buffer;
        Graphics g = null;
        Graphics mainG = null;
        GdiRenderer r;
        System.Windows.Forms.Control cltr;
        float scaleF;
        Matrix scaleI, scaleT;

        public GPURenderer2(System.Windows.Forms.Control cltr)
        {
            IsHardware = false;
            this.cltr = cltr;
            buffer = null;

            scaleF = GetScaleFactor();
            scaleT = new Matrix();
            scaleT.Scale(scaleF, scaleF);
            scaleI = scaleT.GetInverseMatrix();
        }

        public override bool Begin()
        {
            if(buffer == null)
            {
                float scale = GetScaleFactor();
                buffer = new Bitmap((int)(cltr.Width * scale),
                    (int)(cltr.Height * scale));
                mainG = Graphics.FromImage(buffer);
                mainG.SmoothingMode = SmoothingMode.None;
                g = mainG;
                r = mainG.GetRenderer();
            }
            Drawing = true;
            return true;
        }

        public override bool Begin(Color c)
        {
            if (!Begin()) return false;
            g.Clear(c);
            return true;
        }

        public override void End()
        {
            Graphics g = cltr.CreateGraphics();
            g.DrawImage(buffer,
                new Rectangle(new Point(0, 0), cltr.Size));
            g.Dispose();
            Drawing = false;
        }

        public override void End(RectangleF rect)
        {
            Graphics g = cltr.CreateGraphics();
            g.DrawImage(buffer,
                rect, rect, GraphicsUnit.Pixel);
            g.Dispose();
            Drawing = false;
        }

        public override void Dispose()
        {
            buffer?.Dispose();
            mainG?.Dispose();
            scaleT?.Dispose();
            scaleI?.Dispose();
        }

        public override void Resize(Size size)
        {
            this.Width = Math.Min(size.Width, 1);
            this.Height = Math.Min(size.Height, 1);
            buffer?.Dispose();
            mainG?.Dispose();
            float scale = GetScaleFactor();
            buffer = new Bitmap((int)(cltr.Width * scale),
                (int)(cltr.Height * scale));
            mainG = Graphics.FromImage(buffer);
            g = mainG;
            r = mainG.GetRenderer();
        }

        public override void EditPage()
        {
            
        }

        public override void EndEditPage()
        {
            
        }

        public override void Transform(Matrix m)
        {
            g.MultiplyTransform(scaleI, MatrixOrder.Append);
            g.MultiplyTransform(m, MatrixOrder.Append);
            g.MultiplyTransform(scaleT, MatrixOrder.Append);
        }

        public override void ResetTransform()
        {
            Matrix tmp = g.Transform;
            g.Transform = new Matrix();
            g.MultiplyTransform(scaleT, MatrixOrder.Prepend);
            tmp.Dispose();
        }

        public override void DrawDashPolygon(PointF[] pts)
        {
            Pen p = new Pen(Color.Gray, 4);
            p.DashStyle = DashStyle.Dash;
            g.DrawCurve(p, pts);
            p.Width = 2;
            g.DrawLine(p, pts[pts.Length - 1], pts[0]);
            p.Dispose();
        }

        public override void DrawEllipse(PBrush c, float width, RectangleF rect)
        {
            r.DrawEllipse(c, width, rect);
        }

        public override void DrawImage(Image img, RectangleF rect)
        {
            r.DrawImage(img, rect);
        }

        public override void DrawLine(Color c, float width, PointF p1, PointF p2, bool capStart = false, bool capEnd = false)
        {
            r.DrawLine(c, width, p1, p2, capStart, capEnd);
        }

        public override void DrawRect(Color c, float width, RectangleF rect)
        {
            r.DrawRect(c, width, rect);
        }

        public override void DrawRoundedLine(PBrush c, float width, PointF p1, PointF p2)
        {
            r.DrawRoundedLine(c, width, p1, p2);
        }

        public override void DrawRoundedRectangle(PBrush c, float width, RectangleF rect)
        {
            r.DrawRoundedRectangle(c, width, rect);
        }

        public override void DrawText(string text, PBrush brush, RectangleF rect, float size)
        {
            r.DrawText(text, brush, rect, size);
        }

        public override void FillEllipse(PBrush c, RectangleF rect)
        {
            r.FillEllipse(c, rect);
        }

        public override void FillPolygon(PBrush b, PointF[] pts)
        {
            r.FillPolygon(b, pts);
        }

        public override void FillRectangle(Color c, RectangleF rect)
        {
            Brush b = new SolidBrush(c);
            g.FillRectangle(b, rect);
            b.Dispose();
        }

        public override RenderBitmap CreateRenderTarget()
        {
            return new RenderBitmap2(buffer.Size);
        }

        public override void DrawRenderBitmap(RenderBitmap bmp)
        {
            if (!(bmp is RenderBitmap2)) return;
            g.DrawImage(((RenderBitmap2)bmp).GetBuffer(), new PointF(0, 0));
        }

        public override void SetRenderTarget(RenderBitmap bmp)
        {
            if(bmp != null && bmp is RenderBitmap2)
            {
                RenderBitmap2 bmp2 = (RenderBitmap2)bmp;
                g = bmp2.GetGraphics();
                r = g.GetRenderer();
            }
            else
            {
                g = mainG;
                r = g.GetRenderer();
            }
        }

        public override float GetScaleFactor()
        {
            return 1;
        }

        public override void DrawText(string text, PointF pos, float size, Color c)
        {
            Font ft = new Font("Calibri", size);
            SolidBrush b = new SolidBrush(c);
            g.DrawString(text, ft, b, pos);
            b.Dispose();
        }

        public override void BeginCircles(PBrush brush)
        {
            r.BeginCircles(brush);
        }

        public override void Circle(float x, float y, float r)
        {
            this.r.Circle(x, y, r);
        }

        public override void EndCircle()
        {
            r.EndCircle();
        }
    }
}
