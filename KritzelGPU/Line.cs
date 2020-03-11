using LineLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel
{
    public class LPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Rad { get; set; }
        public float dX { get; set; } = 0;
        public float dY { get; set; } = 0;
        public bool Outside = false;
        public int Index { get; set; } = -1;

        public LPoint(float x, float y, float rad)
        {
            this.X = x;
            this.Y = y;
            this.Rad = rad;
        }

        public float DistSquared(float x, float y)
        {
            float dx = this.X - x;
            float dy = this.Y - y;
            return (dx * dx + dy * dy);
        }

        public float Dist(float x, float y)
        {
            float dx = this.X - x;
            float dy = this.Y - y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public float DistSquared(LPoint pt)
        {
            float dx = this.X - pt.X;
            float dy = this.Y - pt.Y;
            return (dx * dx + dy * dy);
        }

        public float Dist(LPoint pt)
        {
            float dx = this.X - pt.X;
            float dy = this.Y - pt.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public float Angle(LPoint pt)
        {
            float dx = this.X - pt.X;
            float dy = this.Y - pt.Y;
            return (float)Math.Atan2(dy, dx);
        }

        public PointF ToPointF()
        {
            return new PointF(X, Y);
        }

        public LPoint Move(float x, float y)
        {
            this.X = x;
            this.Y = y;
            return this;
        }

        public LPoint SetRad(float rad)
        {
            this.Rad = rad;
            return this;
        }

        public override string ToString()
        {
            return "(" + X + "; " + Y + ")";
        }
    }

    public class Line
    {
        public PBrush Brush { get; set; }
        public List<LPoint> Points { get; private set; } = new List<LPoint>();
        RectangleF bounds = new RectangleF(0, 0, 0, 0);
        public int RenderPos { get; private set; } = 0;
        public Spline2D Spline { get; private set; } = null;
        public RectangleF Bounds
        {
            get
            {
                return bounds;
            }
        }
        protected bool checkDist = true;
        public bool Selected { get; set; } = false;

        public virtual void AddPoint(float x, float y, float pressure)
        {
            if (Points.Count == 0)
                bounds = new RectangleF(x, y, 1, 1);

            if(checkDist && Points.Count > 0 && Points.Last().DistSquared(x,y) < 4)
            {
                return;
            }

            if (x < bounds.Left) { bounds.Width += bounds.X - x; bounds.X = x; }
            if (x > bounds.Right) bounds.Width = x - bounds.X;
            if (y < bounds.Top) { bounds.Height += bounds.Y - y; bounds.Y = y; }
            if (y > bounds.Bottom) bounds.Height = y - bounds.Y;

            LPoint n = new LPoint(x, y, pressure);
            Points.Add(n);
        }

        public virtual void Render(Renderer.BaseRenderer g, float quality = 1, int start = 0, bool simple = false)
        {
            if (Points.Count == 1)
            {
                g.BeginCircles(Brush);
                LPoint pt = Points[0];
                sp(g, pt);
                g.EndCircle();
            }
            else if (Points.Count > 1 && !simple)
            {
                g.BeginCircles(Brush);
                sp(g, Points[0]);
                if (start < 1) start = 1;
                for (int i = start; i < Points.Count; i++)
                {
                    sp(g, Points[i]);
                    sc(g, Points[i - 1], Points[i], quality,
                        g.RenderSpecial && this.Selected);
                    PointF p1 = new PointF(Points[i].X - Points[i].dX * 16,
                        Points[i].Y - Points[i].dY * 16);
                    PointF p2 = new PointF(Points[i].X + Points[i].dX * 16,
                        Points[i].Y + Points[i].dY * 16);
                    //g.DrawLine(Color.Lime, 1, p1, p2, false, true);
                }
                RenderPos = Points.Count - 1;
                g.EndCircle();
            }
            else if(Points.Count > 1)
            {
                for(int i = start + 1; i < Points.Count; i++)
                {
                    g.DrawLine(Color.Black, 1, Points[i - 1].ToPointF(), Points[i].ToPointF());
                }
            }
        }

        void sp(Renderer.BaseRenderer g, LPoint pt)
        {
            //RectangleF rect = new RectangleF(pt.X - pt.Rad, pt.Y - pt.Rad, pt.Rad * 2, pt.Rad * 2);
            //g.FillEllipse(Brush, rect);
            g.Circle(pt.X, pt.Y, pt.Rad);
        }

        void sc(Renderer.BaseRenderer g, LPoint p1, LPoint p2, float quality, bool highlight, float border = 0)
        {
            /*if(highlight)
            {
                PBrush tmp = Brush;
                Brush = new PBrush("solid", new Color[] { Color.BlueViolet }, new float[0]);
                sc(g, p1, p2, quality, false, 2);
                Brush = tmp;
            }*/
            if (p1.Outside && p2.Outside) return;
            if (Spline != null)
            {
                int n = (int)(p1.Dist(p2) * quality);
                float dx = (p2.X - p1.X) / n;
                float dy = (p2.Y - p1.Y) / n;
                float dr = (p2.Rad - p1.Rad) / n;
                for (int i = 0; i < n; i++)
                {
                    double x, y;
                    Spline.GetPoint(p1.Index + i / (float)n, out x, out y);
                    LPoint p = new LPoint((float)x, (float)y, p1.Rad + dr * i);
                    if (border == 0)
                        sp(g, p);
                    else
                        sp(g, new LPoint(p.X, p.Y, p.Rad + border));
                }
            }
            else
            {
                int n = (int)(p1.Dist(p2) * quality);
                float dx = (p2.X - p1.X) / n;
                float dy = (p2.Y - p1.Y) / n;
                float dr = (p2.Rad - p1.Rad) / n;
                for (int i = 0; i < n; i++)
                {
                    LPoint p = new LPoint(p1.X + dx * i, p1.Y + dy * i, p1.Rad + dr * i);
                    if (border == 0)
                        sp(g, p);
                    else
                        sp(g, new LPoint(p.X, p.Y, p.Rad + border));
                }
            }
        }

        public virtual bool Collision(LPoint pt)
        {
            if (!Bounds.Contains(pt.X, pt.Y))
                return false;
            foreach (LPoint p in Points)
            {
                if (p.Dist(pt) < p.Rad + pt.Rad)
                    return true;
            }
            return false;
        }

        public virtual float CalcRad(float pressure, float thicknes)
        {
            return pressure / 2000f * thicknes;
        }

        public bool CalcSpline()
        {
            Console.WriteLine("Calc Spline with {0} points", Points.Count);
            if (Points.Count < 4) return false;
            double[] px = new double[Points.Count];
            double[] py = new double[Points.Count];
            for(int i = 0; i < Points.Count; i++)
            {
                px[i] = Points[i].X;
                py[i] = Points[i].Y;
                Points[i].Index = i;
            }
            Spline = new Spline2D(px, py);
            return true;
        }

        public virtual string ToParamString()
        {
            string txt = "";
            foreach(LPoint p in Points)
            {
                txt += string.Format("{0},{1},{2};", 
                    Util.FToS(p.X), Util.FToS(p.Y), Util.FToS(p.Rad));
            }
            return txt;
        }

        public virtual void FromParamString(string txt)
        {
            Points.Clear();
            string[] pts = txt.Split(';');
            foreach(string pt in pts)
            {
                if (pt == "") continue;
                string[] dats = pt.Split(',');
                float x = Util.SToF(dats[0]);
                float y = Util.SToF(dats[1]);
                float rad = Util.SToF(dats[2]);
                LPoint lp = new LPoint(x, y, rad);
                Points.Add(lp);
            }
        }

        public void Transform(Matrix mat)
        {
            foreach(LPoint p in Points)
            {
                PointF[] pl = new PointF[1];
                pl[0] = new PointF(p.X, p.Y);
                mat.TransformPoints(pl);
                p.X = pl[0].X;
                p.Y = pl[0].Y;
            }
            CalculateBounds();
        }

        public void Clamp(RectangleF rect)
        {
            for(int i = 0; i < Points.Count; i++)
            {
                var p = Points[i];
                if(p.X < rect.Left || p.X > rect.Right || p.Y < rect.Top || p.Y > rect.Bottom)
                {
                    Points[i].Outside = true;
                }
            }
            for (int i = 1; i < Points.Count - 1; i++)
            {
                if (Points[i - 1].Outside && Points[i].Outside && Points[i + 1].Outside)
                    Points.RemoveAt(i--);
            }
            CalculateBounds();
        }

        public void CalculateBounds()
        {
            if (Points.Count <= 0) return;
            RectangleF bounds = new RectangleF(Points[0].ToPointF(), new SizeF(0, 0));
            for(int i = 0; i < Points.Count; i++)
            {
                if (Points[i].Outside) continue;
                float x = Points[i].X;
                float y = Points[i].Y;
                if (x < bounds.Left) { bounds.Width += bounds.X - x; bounds.X = x; }
                if (x > bounds.Right) bounds.Width = x - bounds.X;
                if (y < bounds.Top) { bounds.Height += bounds.Y - y; bounds.Y = y; }
                if (y > bounds.Bottom) bounds.Height = y - bounds.Y;
            }
            this.bounds = bounds;
        }

        public virtual bool RefreshInEditor()
        {
            return false;
        }
    }
}
