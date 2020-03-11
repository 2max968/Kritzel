using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel
{
    public partial class SplineView : Form
    {
        LPoint[] pts = new LPoint[]
        {
            new LPoint(32,32,1),
            new LPoint(256,128,1),
            new LPoint(64,128,1),
            new LPoint(256,48,1)
        };
        float ang = 0;

        public SplineView()
        {
            InitializeComponent();

            this.Paint += SplineView_Paint;
        }

        private void SplineView_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(bmp);
            for(int i = 0; i < pts.Length;i++)
            {
                g.DrawEllipse(Pens.Lime, new RectangleF(pts[i].X - 4, pts[i].Y - 4, 8, 8));
                if (i > 0)
                    g.DrawLine(Pens.Lime, pts[i - 1].ToPointF(), pts[i].ToPointF());
            }

            double a1 = Math.PI / 2, a2 = ang;
            pts[1].dX = (float)Math.Cos(a1);
            pts[1].dY = (float)Math.Sin(a1);
            pts[2].dX = (float)Math.Cos(a2);
            pts[2].dY = (float)Math.Sin(a2);
            QubicSpline[] splines = new QubicSpline[]{
                new QubicSpline(pts[0],pts[1]),
                new QubicSpline(pts[1], pts[2]),
                new QubicSpline(pts[2],pts[3]) };
            float d = .01f;
            foreach (QubicSpline spline in splines)
            {
                for (float t = 0; t < 1; t += d)
                {
                    g.DrawLine(Pens.Red, spline.GetPoint(t), spline.GetPoint(t + d));
                }
            }

            PointF slope = splines[2].GetSlope(0);
            g.DrawString(""+(Math.Atan2(slope.Y,slope.X)-ang), new Font("Arial", 12), Brushes.Black, new Point(10, 10));
            PointF p = splines[1].GetPoint(1);
            g.DrawEllipse(Pens.Black, new RectangleF(p.X - 2, p.Y - 2, 4, 4));


            e.Graphics.DrawImage(bmp, new PointF(0,0));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ang += (float)Math.PI / 20;
            if (ang > Math.PI) ang -= 2 * (float)Math.PI;
            this.Refresh();
        }
    }
}
