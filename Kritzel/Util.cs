using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel
{
    public static class Util
    {
        public static RectangleF CreateRect(PointF p1, PointF p2)
        {
            RectangleF rect = new RectangleF(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
            if(rect.Width < 0)
            {
                rect.Width *= -1;
                rect.X -= rect.Width;
            }
            if(rect.Height < 0)
            {
                rect.Height *= -1;
                rect.Y -= rect.Height;
            }
            return rect;
        }

        public static string FToS(float f)
        {
            return f.ToString(CultureInfo.InvariantCulture);
        }

        public static float SToF(string s)
        {
            return float.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool TrySToF(string s, out float f)
        {
            return float.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out f);
        }

        public static Matrix GetInverseMatrix(this Matrix mat)
        {
            // [d/(a* d - b* c),           -b/(a* d - b* c), 0]
            // [          -c/(a* d - b* c), a/(a* d - b* c), 0]
            // [ (c * f - d * e)/(a* d - b* c), -(a* f - b* e)/(a* d - b* c), 1]
            float a = mat.Elements[0];
            float b = mat.Elements[1];
            float c = mat.Elements[2];
            float d = mat.Elements[3];
            float e = mat.Elements[4];
            float f = mat.Elements[5];
            return new Matrix(d / (a * d - b * c), -b / (a * d - b * c),
                -c / (a * d - b * c), a / (a * d - b * c),
                (c * f - d * e) / (a * d - b * c), -(a * f - b * e) / (a * d - b * c));
        }

        public static void Transform(this Matrix mat, ref float x, ref float y)
        {
            // [e + a*x + c*y, f + b*x + d*y, 1]
            float a = mat.Elements[0];
            float b = mat.Elements[1];
            float c = mat.Elements[2];
            float d = mat.Elements[3];
            float e = mat.Elements[4];
            float f = mat.Elements[5];
            x = e + a * x + c * y;
            y = f + b * x + d * y;
        }

        public static void Transform(this Matrix mat, LPoint p)
        {
            // [a*x + c*y, b*x + d*y, 0]
            float a = mat.Elements[0];
            float b = mat.Elements[1];
            float c = mat.Elements[2];
            float d = mat.Elements[3];
            float e = mat.Elements[4];
            float f = mat.Elements[5];
            float x = p.X;
            float y = p.Y;
            x = e + a * x + c * y;
            y = f + b * x + d * y;
            p.X = x;
            p.Y = y;
        }

        public static string Run(string cmd, string args)
        {
            ProcessStartInfo psi = new ProcessStartInfo(cmd, args);
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            Process p = Process.Start(psi);
            p.WaitForExit();
            string txt = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
            return txt;
        }

        public static Renderer.GdiRenderer GetRenderer(this Graphics g)
        {
            return new Renderer.GdiRenderer(g);
        }

        public static float MmToPoint(float mm)
        {
            float inch = 0.0393701f;
            float dpi = 96;
            return mm * dpi * inch;
        }

        public static float PointToMm(float point)
        {
            float inch = 0.0393701f;
            float dpi = 96;
            return point / (dpi * inch);
        }

        public static PointF GetTranslation(this Matrix m)
        {
            return new PointF(m.Elements[4], m.Elements[5]);
        }

        public static PointF GetScale(this Matrix m)
        {
            float lx = (float)Math.Sqrt(m.Elements[0] * m.Elements[0] 
                + m.Elements[1] * m.Elements[1]);
            float ly = (float)Math.Sqrt(m.Elements[2] * m.Elements[2]
                + m.Elements[3] * m.Elements[3]);
            return new PointF(lx, ly);
        }

        public static Matrix GetRotationMatrix(this Matrix m)
        {
            PointF scale = GetScale(m);
            float a = m.Elements[0] / scale.X;
            float b = m.Elements[1] / scale.X;
            float c = m.Elements[2] / scale.Y;
            float d = m.Elements[3] / scale.Y;
            return new Matrix(a, b, c, d, 0, 0);
        }

        public static void TransformInverse(this Matrix m, PointF[] pts)
        {
            using (Matrix inv = m.GetInverseMatrix())
            {
                inv.TransformPoints(pts);
            }
        }

        public static RectangleF GetBounds(PointF[] pts)
        {
            if (pts.Length == 0) return new RectangleF(0, 0, 0, 0);
            RectangleF bounds = new RectangleF(pts[0].X, pts[0].Y, 0, 0);
            foreach(PointF pt in pts)
            {
                float x = pt.X;
                float y = pt.Y;
                if (x < bounds.Left) { bounds.Width += bounds.X - x; bounds.X = x; }
                if (x > bounds.Right) bounds.Width = x - bounds.X;
                if (y < bounds.Top) { bounds.Height += bounds.Y - y; bounds.Y = y; }
                if (y > bounds.Bottom) bounds.Height = y - bounds.Y;
            }
            return bounds;
        }
    }
}
