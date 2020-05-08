using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kritzel.Main.Renderer;

namespace Kritzel.Main.Backgrounds
{
    [BName("Border")]
    public class BackgroundBorder : Background
    {
        public override void Draw(BaseRenderer r, PageFormat format, float border, Color mainColor, Color secondaryColor)
        {
            float borderpx = Util.MmToPoint(border);
            SizeF bsize = format.GetPixelSize();
            RectangleF bounds = new RectangleF(borderpx, borderpx, bsize.Width-2*borderpx, bsize.Height-2*borderpx);
            r.DrawRect(secondaryColor, 1, bounds);
        }
    }
}
