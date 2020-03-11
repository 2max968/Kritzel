using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel
{
    public class QubicSpline
    {
        public float[] A = new float[4];
        public float[] B = new float[4];

        public QubicSpline(LPoint p1, LPoint p2)
        {
            float slopefact = 1;

            float x0 = p1.X;
            float x1 = p2.X;
            float y0 = p1.Y;
            float y1 = p2.Y;
            float dx0 = p1.dX * slopefact;
            float dx1 = p2.dX * slopefact;
            float dy0 = p1.dY * slopefact;
            float dy1 = p2.dY * slopefact;

            /*A[0] = -2 * x1 - dx1 - 3 * dx0 - 4 * x0;
            A[1] = 3 * x1 - dx1 - 2 * dx0 - 3 * x0;
            A[2] = dx0;
            A[3] = x0;
            B[0] = -2 * y1 - dy1 - 3 * dy0 - 4 * y0;
            B[1] = 3 * y1 - dy1 - 2 * dy0 - 3 * y0;
            B[2] = dy0;
            B[3] = y0;*/

            A[0] = dx1 - 2 * x1 + x0;
            A[1] = 3 * x1 - dx1 - 2 * x0 - dx0;
            A[2] = dx0;
            A[3] = x0;
            B[0] = dy1 - 2 * y1 + y0;
            B[1] = 3 * y1 - dy1 - 2 * y0 - dy0;
            B[2] = dy0;
            B[3] = y0;
        }

        public PointF GetPoint(float t)
        {
            float x = A[0] * t * t * t + A[1] * t * t + A[2] * t + A[3];
            float y = B[0] * t * t * t + B[1] * t * t + B[2] * t + B[3];
            return new PointF(x, y);
        }

        public PointF GetSlope(float t)
        {
            float x = 3 * A[0] * t * t + 2 * A[1] * t + A[2];
            float y = 3 * B[0] * t * t + 2 * B[1] * t + B[2];
            return new PointF(x, y);
        }
    }
}
