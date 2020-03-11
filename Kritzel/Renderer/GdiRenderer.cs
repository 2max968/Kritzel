using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Renderer
{
    public class GdiRenderer : BaseRenderer
    {
        Graphics g;

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

        public override void FillEllipse(Brush c, RectangleF rect)
        {
            g.FillEllipse(c, rect);
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
    }
}
