using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Forms
{
    public class LinearLine : Line
    {
        public LinearLine()
        {
            base.checkDist = false;
        }

        public override void AddPoint(float x, float y, float pressure)
        {
            if(Points.Count < 2)
                base.AddPoint(x, y, pressure);
            else
            {
                Points.Last().Move(x, y);
            }
        }

        public override float CalcRad(float pressure, float thicknes)
        {
            return .5f * thicknes;
        }

        public override void Render(Renderer.BaseRenderer g, float quality = 1, int start = 0, bool simple = false)
        {
            if (Points.Count < 2)
                base.Render(g, quality, start);
            else
                g.DrawRoundedLine(Brush, Points[0].Rad * 2, Points[0].ToPointF(), Points[1].ToPointF());
        }

        public override bool RefreshInEditor()
        {
            return true;
        }
    }
}
