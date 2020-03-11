using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Renderer
{
    public abstract class BaseRenderer
    {
        public bool RenderSpecial = true;
        public abstract void DrawLine(Color c, float width, PointF p1, PointF p2, bool capStart = false, bool capEnd = false);
        public abstract void FillEllipse(Brush c, RectangleF rect);
        public abstract void DrawEllipse(PBrush c, float width, RectangleF rect);
        public abstract void DrawRoundedLine(PBrush c, float width, PointF p1, PointF p2);
        public abstract void DrawRoundedRectangle(PBrush c, float width, RectangleF rect);
        public abstract void DrawRect(Color c, float width, RectangleF rect);
    }
}
