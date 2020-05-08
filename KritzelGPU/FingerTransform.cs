using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Main
{
    public class FingerTransform
    {
        public float Rotation;
        public float Distance;
        public Point Position;

        public FingerTransform(int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            Rotation = (float)Math.Atan2(dy, dx);
            Distance = (float)Math.Sqrt(dx * dx + dy * dy);
            Position = new Point((x1 + x2) / 2, (y1 + y2) / 2);
        }
    }
}
