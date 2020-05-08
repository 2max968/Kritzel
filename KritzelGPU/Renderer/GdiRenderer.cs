using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Main.Renderer
{
    public class GdiRenderer : BaseRenderer
    {
        Graphics g;
        Brush cColor = new SolidBrush(Color.Fuchsia);

        public GdiRenderer(Graphics g)
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
            g.FillEllipse(c.Brush, rect);
        }

        public override void DrawRoundedLine(PBrush c, float width, PointF p1, PointF p2)
        {
            g.DrawLine(new Pen(c.Brush, width)
            {
                DashCap = System.Drawing.Drawing2D.DashCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                StartCap = System.Drawing.Drawing2D.LineCap.Round
            },
                    p1,p2);
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
            g.DrawImage(img.GdiBitmap, rect);
        }

        public override void FillPolygon(PBrush b, PointF[] pts)
        {
            g.FillPolygon(b.Brush, pts);
        }

        public override void DrawText(string text, PBrush brush, RectangleF rect, float size)
        {
            float x = Util.MmToPoint(rect.X);
            float y = Util.MmToPoint(rect.Y);
            float w = Util.MmToPoint(rect.Width);
            float h = 0;
            g.DrawString(text, new Font("Calibri", Util.MmToPoint(size)), brush.Brush,
                new RectangleF(x, y, w, h));
        }

        public override void BeginCircles(PBrush brush)
        {
            cColor?.Dispose();
            cColor = new SolidBrush(brush.GetColors()[0]);
        }

        public override void Circle(float x, float y, float r)
        {
            RectangleF rect = new RectangleF(x - r, y - r, 2 * r, 2 * r);
            g.FillEllipse(cColor, rect);
        }

        public override void EndCircle()
        {
            
        }
    }
}
