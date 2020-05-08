using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kritzel.Main.Renderer;

namespace Kritzel.Main.Backgrounds
{
    [BName("Ruled 15mm")]
    public class BackgroundRiledPaper15mm : Background
    {
        public override void Draw(BaseRenderer r, PageFormat format, float border, Color mainColor, Color secondaryColor)
        {
            float borderpx = Util.MmToPoint(border);
            float x2 = format.GetPixelSize().Width - borderpx;
            for (float y = border; y < format.Height - border; y+=15)
            {
                float pt = Util.MmToPoint(y);
                r.DrawLine(mainColor, 1, new PointF(borderpx, pt), new PointF(x2, pt));
            }
        }
    }
}
