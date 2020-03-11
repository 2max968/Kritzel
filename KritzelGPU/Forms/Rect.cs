using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Forms
{
    public class Rect : LinearLine
    {
        public override void Render(Renderer.BaseRenderer g, float quality = 1, int start = 0, bool simple = false)
        {
            if(Points.Count < 2)
                base.Render(g, quality, start);
            else
            {
                RectangleF r = Util.CreateRect(Points[0].ToPointF(), Points[1].ToPointF());
                g.DrawRoundedRectangle(Brush, Points[0].Rad * 2, r);
            }
        }

        public override bool Collision(LPoint pt)
        {
            if (Points.Count <= 1) return false;
            float rad = Points[0].Rad;
            RectangleF r1 = Util.CreateRect(Points[0].ToPointF(),
                Points[1].ToPointF());
            RectangleF r2 = r1;
            r1.X -= rad;
            r1.Y -= rad;
            r1.Width += 2 * rad;
            r1.Height += 2 * rad;
            r2.X += rad;
            r2.Y += rad;
            r2.Width -= 2 * rad;
            r2.Height -= 2 * rad;

            return r1.Contains(pt.ToPointF()) && !r2.Contains(pt.ToPointF());
        }
    }
}
