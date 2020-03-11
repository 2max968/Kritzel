using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineLib
{
    public class Spline
    {
        public int Length { get; private set; }
        public double[] A { get; private set; }
        public double[] B { get; private set; }
        public double[] C { get; private set; }
        public double[] D { get; private set; }

        public Spline(double[] y)
        {
            Length = y.Length - 1;
            int n = y.Length - 1;
            double h = 1;

            double[] sigma = new double[n + 1];
            for (int i = 1; i < n; i++)
            {
                double p = (y[i + 1] - 2 * y[i] + y[i - 1]) * 6.0 / (h * h);
                sigma[i] = (p - sigma[i - 1]) / 4.0;
            }

            A = new double[n];
            B = new double[n];
            C = new double[n];
            D = new double[n];

            for(int i = 0; i < n; i++)
            {
                A[i] = (sigma[i + 1] - sigma[i]) / (6 * h);
                B[i] = sigma[i] / 2.0;
                C[i] = (y[i + 1] - y[i]) / h - h / 6 * (2 * sigma[i] + sigma[i + 1]);
                D[i] = y[i];
            }
        }

        double pow(double v, int e)
        {
            switch(e)
            {
                case 0:
                    return 1;
                case 1:
                    return v;
                case 2:
                    return v * v;
                case 3:
                    return v * v * v;
                default:
                    return Math.Pow(v, e);
            }
        }

        public double GetVal(double t)
        {
            int i = (int)t;
            if (i < 0) i = 0;
            if (i >= Length) i = Length - 1;
            double v = A[i] * pow(t - i, 3) + B[i] * pow(t - i, 2) + C[i] * (t - i) + D[i];
            return v;
        }
    }
}
