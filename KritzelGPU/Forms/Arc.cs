using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Forms
{
    public class Arc : LinearLine
    {
        public override void Render(Renderer.BaseRenderer g, float quality = 1, int start = 0, bool simple = false)
        {
            if(Points.Count > 1)
            {
                LPoint center = Points[0];
                float rad = center.Dist(Points[1]);
                RectangleF r = new RectangleF(center.X - rad, center.Y - rad, rad * 2, rad * 2);
                g.DrawEllipse(Brush, Points[0].Rad * 2, r);
            }
        }

        public override bool Collision(LPoint pt)
        {
            float dist = pt.Dist(Points[0]);
            float rad = Points[0].Dist(Points[1]);
            return (dist > rad - Points[0].Rad
                && dist < rad + Points[0].Rad) ;
        }
    }
}
