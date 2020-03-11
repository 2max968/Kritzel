using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineLib
{
    public class Spline2D
    {
        public Spline SplineX;
        public Spline SplineY;

        public Spline2D(double[] x, double[] y)
        {
            SplineX = new Spline(x);
            SplineY = new Spline(y);
        }

        public void GetPoint(double t, out double x, out double y)
        {
            x = SplineX.GetVal(t);
            y = SplineY.GetVal(t);
        }
    }
}
