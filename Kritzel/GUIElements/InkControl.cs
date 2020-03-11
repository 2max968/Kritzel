using PointerInputLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Kritzel
{
    public enum InkMode { Pen, Line, Rect, Arc };

    public class InkControl : PictureBox
    {
        KPage page = new KPage();
        public KPage Page { get { return page; } }
        Bitmap buffer;
        Graphics g;
        Line line = null;
        PointF pos = new PointF(0, 0);
        PointerManager pm;
        System.Windows.Forms.Timer timer;
        bool active = true;
        public float Thicknes { get; set; } = 5;
        public PBrush Brush { get; set; } = PBrush.CreateSolid(Color.Black);
        int staticI = 0;
        public InkMode InkMode { get; set; } = InkMode.Pen;
        public PageFormat PageFormat
        {
            get
            {
                return page.Format;
            }
            set
            {
                page.Format = value;
                var s = page.Format.GetPixelSize();
            }
        }
        Matrix transform = new Matrix();
        bool lastMove = false;
        Point lastMovePoint = new Point(0, 0);
        bool lastTrans = false;
        FingerTransform lastTransInfo;
        Renderer.BaseRenderer renderer;
        public bool LockMove { get; set; } = false;
        public bool LockScale { get; set; } = false;
        public bool LockRotate { get; set; } = false;
        float bufferSize = 1;
        public float BufferSize
        {
            get { return bufferSize; }
            set
            {
                bufferSize = value;
                recreateBuffer();
            }
        }
        List<PointF> selections = null;
        bool lastMouseDown = false;
        Point lastMousePos = new Point(0, 0);

        public InkControl()
        {
            pm = new PointerManager(this);
            recreateBuffer();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Start();
            timer.Tick += timer_tick;
            this.Paint += this_paint;
            this.SizeChanged += InkControl_SizeChanged;
            this.VisibleChanged += InkControl_VisibleChanged;
            SizeMode = PictureBoxSizeMode.StretchImage;
            renderer = new Renderer.GdiRenderer(CreateGraphics());

            this.MouseWheel += InkControl_MouseWheel;
            this.MouseMove += InkControl_MouseMove;

            if (Process.GetCurrentProcess().ProcessName == "devenv")
                active = false;
        }

        private void InkControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if(!lastMouseDown)
                {
                    lastMouseDown = true;
                }
                else
                {
                    Point delta = new Point(e.X - lastMousePos.X, e.Y - lastMousePos.Y);
                    Matrix m = new Matrix();
                    m.Translate(delta.X, delta.Y);
                    transform.Multiply(m, MatrixOrder.Append);
                    m.Dispose();
                    recreateBuffer();
                }
                lastMousePos = new Point(e.X, e.Y);
            }
            else
            {
                lastMouseDown = false;
            }
        }

        private void InkControl_MouseWheel(object sender, MouseEventArgs e)
        {
            float factor = 1000 + e.Delta;
            if (factor > 1500) factor = 2000;
            if (factor < 500) factor = 500;
            factor /= 1000f;
            Matrix m = new Matrix();
            m.Translate(e.X, e.Y);
            m.Scale(factor, factor);
            m.Translate(-e.X, -e.Y);
            transform.Multiply(m, MatrixOrder.Append);
            m.Dispose();
            recreateBuffer();
        }

        private void InkControl_VisibleChanged(object sender, EventArgs e)
        {
            this.PageFormat = this.PageFormat;
        }

        private void InkControl_SizeChanged(object sender, EventArgs e)
        {
            recreateBuffer();
        }

        protected override void WndProc(ref Message m)
        {
            if(active) pm.HandleWndProc(ref m);
            base.WndProc(ref m);
        }

        private void timer_tick(object sender, EventArgs e)
        {
            // Search for Pen / Finger
            Touch pen = null;
            List<Touch> fingers = new List<Touch>();
            foreach (Touch t in pm.Touches.Values)
            {
                if (t.TouchDevice == TouchDevice.Pen && t.Down)
                {
                    pen = t;
                    break;
                }
                if (t.TouchDevice == TouchDevice.Finger)
                    fingers.Add(t);
            }

            // Eraser
            if (pen != null && (pen.PenFlags & PenFlags.INVERTED) == PenFlags.INVERTED)
            {
                for (int i = 0; i < page.Lines.Count; i++)
                {
                    Point[] pts = new Point[1];
                    pts[0] = new Point(pen.X, pen.Y);
                    transform.GetInverseMatrix().TransformPoints(pts);
                    if (page.Lines[i].Collision(new LPoint(pts[0].X, pts[0].Y, pen.Pressure/100)))
                    {
                        page.Lines.RemoveAt(i--);
                        recreateBuffer();
                        break;
                    }
                }

                pen = null;
            }

            // Select
            if (pen != null && (pen.PenFlags & PenFlags.BARREL) == PenFlags.BARREL)
            {
                if(selections == null)
                {
                    selections = new List<PointF>();
                    selections.Add(new PointF(pen.X, pen.Y));
                }
                else
                {
                    selections.Add(new PointF(pen.X, pen.Y));
                    recreateBuffer();
                }
                pen = null;
            }
            else if(selections != null)
            {
                PointF[] pts = selections.ToArray();
                selections = null;
                transform.TransformInverse(pts);
                Page.SelectArea(pts);
                recreateBuffer();
            }

            // Move
            if (fingers.Count == 1 && !LockMove)
            {
                Point fp = new Point(fingers[0].X, fingers[0].Y);
                if (lastMove)
                {
                    int deltaX = fp.X - lastMovePoint.X;
                    int deltaY = fp.Y - lastMovePoint.Y;
                    Matrix mat = new Matrix();
                    mat.Translate(deltaX, deltaY);
                    transform.Multiply(mat, MatrixOrder.Append);
                }
                lastMove = true;
                lastMovePoint = fp;
                recreateBuffer();
            }
            else lastMove = false;

            // Transform
            if (fingers.Count == 2
                && !(LockMove && LockScale && LockRotate))
            {
                FingerTransform t = new FingerTransform(fingers[0].X, fingers[0].Y, fingers[1].X, fingers[1].Y);
                if(lastTrans)
                {
                    float dd = 1 + (t.Distance - lastTransInfo.Distance) / lastTransInfo.Distance;
                    float dr = t.Rotation - lastTransInfo.Rotation;
                    Point dp = new Point(t.Position.X - lastTransInfo.Position.X,
                        t.Position.Y - lastTransInfo.Position.Y);

                    if (LockMove) dp = new Point(0, 0);
                    if (LockScale) dd = 1;
                    if (LockRotate) dr = 0;

                    Matrix mat = new Matrix();
                    mat.Translate(dp.X, dp.Y);
                    mat.Translate(t.Position.X, t.Position.Y);
                    mat.Scale(dd, dd);
                    mat.Translate(-t.Position.X, -t.Position.Y);
                    mat.RotateAt(dr * 180f / (float)Math.PI, t.Position);
                    transform.Multiply(mat, MatrixOrder.Append);
                    recreateBuffer();
                }
                lastTrans = true;
                lastTransInfo = t;
            }
            else lastTrans = false;

            // Draw
            if (line != null)
            {
                if (pen != null)
                {
                    float rad = line.CalcRad(pen.Pressure, Thicknes);
                    line.AddPoint(pen.X, pen.Y, rad);
                    line.Render(CreateGraphics().GetRenderer(), 1, line.Points.Count - 2, false);
                    if (line.RefreshInEditor()) recreateBuffer();
                }
                else
                {
                    Matrix inv = transform.GetInverseMatrix();
                    line.Transform(inv);
                    line.Clamp(new RectangleF(new PointF(0, 0), Page.Format.GetPixelSize()));
                    line.Render(g.GetRenderer());
                    page.Lines.Add(line);
                    page.Deselect();
                    line = null;
                    recreateBuffer();
                    this.Refresh();
                }
            }
            else if (pen != null)
            {
                pen.Trail.Clear();
                if (InkMode == InkMode.Pen)
                {
                    line = new Line();
                    line.Brush = Brush;
                    line.AddPoint(pen.X, pen.Y, line.CalcRad(pen.Pressure, Thicknes));
                }
                else if(InkMode == InkMode.Line)
                {
                    line = new Forms.LinearLine();
                    line.Brush = Brush;
                    line.AddPoint(pen.X, pen.Y, line.CalcRad(pen.Pressure, Thicknes));
                }
                else if (InkMode == InkMode.Rect)
                {
                    line = new Forms.Rect();
                    line.Brush = Brush;
                    line.AddPoint(pen.X, pen.Y, line.CalcRad(pen.Pressure, Thicknes));
                }
                else if (InkMode == InkMode.Arc)
                {
                    line = new Forms.Arc();
                    line.Brush = Brush;
                    line.AddPoint(pen.X, pen.Y, line.CalcRad(pen.Pressure, Thicknes));
                }
            }
        }

        private void this_paint(object sender, PaintEventArgs e)
        {
            if (line != null)
            {
                line.Render(e.Graphics.GetRenderer(), 1f, 0, true);
            }
            if (selections != null)
            {
                if(selections.Count > 1)
                    g.DrawPolygon(Pens.Black, selections.ToArray());
            }
        }

        void recreateBuffer()
        {
            if (this.Height <= 0 || this.Width <= 0) return;
            int vW = (int)(this.Width * BufferSize);
            int vH = (int)(this.Height * BufferSize);
            if (buffer == null || vW != buffer.Width || vH != buffer.Height)
            {
                buffer = new Bitmap(vW, vH);
                g = Graphics.FromImage(buffer);
                g.SmoothingMode = SmoothingMode.None;
            }
            g.Clear(SystemColors.ControlDark);
            g.Transform = transform;
            SizeF pSize = page.Format.GetPixelSize();
            if (page.BackgroundImage == null)
                g.FillRectangle(Brushes.White, 0, 0, pSize.Width, pSize.Height);
            else
                g.DrawImage(page.BackgroundImage, 0, 0, pSize.Width, pSize.Height);
            if (page.Background != null)
                page.Background.Draw(g.GetRenderer(), page.Format, page.Border,
                    page.BackgroundColor1, page.BackgroundColor2);
            g.DrawRectangle(Pens.Black, 0, 0, pSize.Width, pSize.Height);

            foreach (Line l in page.Lines)
                l.Render(g.GetRenderer());

            if(line != null)
            {
                line.Render(g.GetRenderer());
            }
            if(selections != null)
            {
                Pen p = new Pen(Color.Gray, 2);
                p.DashStyle = DashStyle.Dash;
                PointF[] pts = selections.ToArray();
                transform.TransformInverse(pts);
                g.DrawPolygon(p, pts);
            }

            this.Image = buffer;
        }

        public void Print(PrintDocument doc)
        {
            
        }

        public string SaveToString()
        {
            return page.SaveToString();
        }

        public void LoadFromString(string txt)
        {
            KPage p = new KPage();
            p.LoadFromString(txt);
            this.page = p;
            recreateBuffer();
            this.Refresh();
        }

        public void LoadPage(KPage page)
        {
            this.page = page;
            recreateBuffer();
            this.Refresh();
        }

        public void RefreshPage()
        {
            recreateBuffer();
        }

        public void ResetRotation()
        {
            Matrix rot = transform.GetRotationMatrix().GetInverseMatrix();
            transform.Multiply(rot, MatrixOrder.Append);
        }
    }
}
