using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kritzel.Main.Renderer;

namespace Kritzel.Main.Backgrounds
{
    [BName("Ruled Title 15mm")]
    public class BackgroundRuledTitle15mm : Background
    {
        public override void Draw(BaseRenderer r, PageFormat format, float border, Color mainColor, Color secondaryColor)
        {
            float borderpx = Util.MmToPoint(border);

            float titleY = Util.MmToPoint(border + 15);
            float titleX = Util.MmToPoint(format.Width / 2);
            float right = Util.MmToPoint(format.Width - border);
            float height = Util.MmToPoint(format.Height);
            float width = Util.MmToPoint(format.Width);

            r.DrawLine(mainColor, 1, new PointF(borderpx*2, titleY), new PointF(titleX, titleY));
            
            for(float y = border + 35; y < format.Height-border; y+=15)
            {
                float yPx = Util.MmToPoint(y);
                r.DrawLine(mainColor, 1, new PointF(0, yPx), new PointF(width, yPx));
            }

            r.DrawLine(secondaryColor, 1, new PointF(borderpx*2, 0), new PointF(borderpx*2, height));
        }
    }
}
