using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using PointerInputLibrary;

namespace Kritzel
{
    public partial class ViewControl : Form
    {
        Bitmap buffer;
        Graphics g;
        Matrix transform;
        PointerManager pm;

        bool lastDown = false;
        FingerTransform lastTransform;

        float rotation = 0;
        float zoom = 1;

        public ViewControl()
        {
            InitializeComponent();

            buffer = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(buffer);
            transform = new Matrix();
            pm = new PointerManager(this);

            timer.Start();

            this.Paint += PbView_Paint;
        }

        private void PbView_Paint(object sender, PaintEventArgs e)
        {
            g.Clear(Color.White);
            g.ResetTransform();
            if(lastDown)
                g.FillRectangle(Brushes.Black, new RectangleF(lastTransform.Position.X - 2, lastTransform.Position.Y - 2, 4, 4));
            g.Transform = transform;
            g.FillRectangle(Brushes.Black, new RectangleF(16, 16, 32, 32));
            e.Graphics.DrawImage(buffer, new PointF(0, 0));
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            List<Touch> fingers = new List<Touch>();
            foreach (KeyValuePair<uint, Touch> t in pm.Touches)
                if (t.Value.TouchDevice == TouchDevice.Finger)
                    fingers.Add(t.Value);

            this.Text = "" + fingers.Count;
            if(fingers.Count >= 2)
            {
                Touch t1 = fingers[0];
                Touch t2 = fingers[1];

                FingerTransform t = new FingerTransform(t1.X, t1.Y, t2.X, t2.Y);
                if(lastDown)
                {
                    float dd = 1 + (t.Distance - lastTransform.Distance) / t.Distance;
                    float dr = t.Rotation - lastTransform.Rotation;
                    Point dp = new Point(t.Position.X - lastTransform.Position.X,
                        t.Position.Y - lastTransform.Position.Y);
                    
                    Matrix mat = new Matrix();
                    mat.Translate(dp.X, dp.Y);
                    mat.Translate(t.Position.X, t.Position.Y);
                    mat.Scale(dd, dd);
                    mat.Translate(-t.Position.X, -t.Position.Y);
                    mat.RotateAt(dr * 180f / (float)Math.PI, t.Position);
                    transform.Multiply(mat, MatrixOrder.Append);

                    this.Text = "" + rotation;
                }
                lastTransform = t;

                lastDown = true;
            }
            else
            {
                lastDown = false;
            }

            this.Refresh();
        }

        protected override void WndProc(ref Message m)
        {
            pm.HandleWndProc(ref m);
            base.WndProc(ref m);
        }
    }
}
