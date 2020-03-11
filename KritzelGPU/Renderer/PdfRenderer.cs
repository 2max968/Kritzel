using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Renderer
{
    public class PdfRenderer : BaseRenderer
    {
        XGraphics g;
        XBrush cBrush;

        public PdfRenderer(XGraphics g)
        {
            this.g = g;
        }

        public override void DrawLine(Color c, float width, PointF p1, PointF p2, bool capStart = false, bool capEnd = false)
        {
            Pen p = new Pen(c, width);
            if (capStart) p.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            if (capEnd) p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            g.DrawLine(p, p1, p2);
        }

        public override void FillEllipse(PBrush c, RectangleF rect)
        {
            g.DrawEllipse(createBrush(c), rect);
        }

        public override void DrawRoundedLine(PBrush c, float width, PointF p1, PointF p2)
        {
            g.DrawLine(new Pen(c.Brush, width)
            {
                DashCap = System.Drawing.Drawing2D.DashCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                StartCap = System.Drawing.Drawing2D.LineCap.Round
            },
                    p1, p2);
        }

        public override void DrawEllipse(PBrush c, float width, RectangleF rect)
        {
            g.DrawEllipse(new Pen(c.Brush, width), rect);
        }

        public override void DrawRoundedRectangle(PBrush c, float width, RectangleF rect)
        {
            g.DrawRectangle(new Pen(c.Brush, width)
            {
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            }, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public override void DrawRect(Color c, float width, RectangleF rect)
        {
            g.DrawRectangle(new Pen(c, width),
                rect.X, rect.Y, rect.Width, rect.Height);
        }

        public override void DrawImage(Image img, RectangleF rect)
        {
            XImage bmp = XImage.FromGdiPlusImage(img.GdiBitmap);
            g.DrawImage(bmp, rect);
            bmp.Dispose();
        }

        public override void FillPolygon(PBrush b, PointF[] pts)
        {
            g.DrawPolygon(createBrush(b), pts, XFillMode.Alternate);
        }

        XBrush createBrush(PBrush pBrush)
        {
            XBrush brush = new XSolidBrush(pBrush.GetMainColor());
            return brush;
        }

        public override void DrawText(string text, PBrush brush, RectangleF rect, float size)
        {
            float x = Util.MmToPoint(rect.X);
            float y = Util.MmToPoint(rect.Y);
            float w = Util.MmToPoint(rect.Width);
            float h = 0;
            g.DrawString(text, new XFont("Calibri", Util.MmToPoint(size) * 1.3), createBrush(brush),
                new RectangleF(x, y, w, h));
        }

        public override void BeginCircles(PBrush brush)
        {
            cBrush = new XSolidBrush(brush.GetColors()[0]);
        }

        public override void Circle(float x, float y, float r)
        {
            RectangleF rect = new RectangleF(x - r, y - r, 2 * r, 2 * r);
            g.DrawEllipse(cBrush, rect);
        }

        public override void EndCircle()
        {
            
        }
    }
}
