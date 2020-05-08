using Kritzel.PointerInputLibrary;
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

namespace Kritzel.Main
{
    public enum InkMode { Pen, Line, Rect, Arc };

    public class InkControl : PictureBox
    {
        KPage page = new KPage();
        public KPage Page { get { return page; } }
        Line line = null;
        List<PointF> linePoints = null;
        PointF pos = new PointF(0, 0);
        PointerManager pm;
        System.Windows.Forms.Timer timer;
        bool active = true;
        public float Thicknes { get; set; } = 5;
        public PBrush Brush { get; set; } = PBrush.CreateSolid(Color.Black);
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
        volatile Matrix3x3 transform = new Matrix3x3();
        volatile Matrix3x3 stableTransform = new Matrix3x3();
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
        public Renderer.GPURenderer gpuRenderer;
        Thread renderThread;
        Thread renderThread2;
        volatile bool running = true;
        volatile bool redraw = false;
        volatile bool fullredraw = false;
        volatile bool lockDraw = false;
        public bool LockDraw
        {
            get
            {
                return lockDraw;
            }
            set
            {
                bool tmp = lockDraw;
                lockDraw = value;
                redraw = true;
                if(value && !tmp)
                {
                    while (!waiting) ;
                }
            }
        }
        volatile bool waiting = false;
        Renderer.RenderBitmap rbmp = null;
        RectangleF rBounds = RectangleF.Empty;
        Line tmpLine = null;

        public InkControl()
        {
            pm = new PointerManager(this);
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Start();
            timer.Tick += timer_tick;
            this.Paint += this_paint;
            this.SizeChanged += InkControl_SizeChanged;
            this.VisibleChanged += InkControl_VisibleChanged;
            //SizeMode = PictureBoxSizeMode.StretchImage;
            renderer = new Renderer.GdiRenderer(CreateGraphics());
            gpuRenderer = Renderer.GPURenderer.Create(this, false);

            this.MouseWheel += InkControl_MouseWheel;
            this.MouseMove += InkControl_MouseMove;
            
            recreateBufferFull();
            renderThread = new Thread(renderLoop);
            renderThread2 = new Thread(renderLoop2);

            /*MenuItem[] contextMenu = new MenuItem[]
            {
                new MenuItem("Copy"),
                new MenuItem("Paste")
            };
            this.ContextMenu = new ContextMenu(contextMenu);*/

            if (Process.GetCurrentProcess().ProcessName == "devenv")
                active = false;
        }

        public void InitRenderer()
        {
            if (active)
            {
                gpuRenderer = Renderer.GPURenderer.Create(this, true);
                renderThread.Start();
                renderThread2.Start();
                recreateBufferFull();
            }
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
                    Matrix3x3 m = Matrix3x3.Translation(delta.X, delta.Y);
                    transform *= m;
                    recreateBufferFull();
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
            Matrix3x3 m = new Matrix3x3();
            m *= Matrix3x3.Translation(-e.X, -e.Y);
            m *= Matrix3x3.Scale(factor);
            m *= Matrix3x3.Translation(e.X, e.Y);
            transform *= m;
            recreateBufferFull();
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
            if (Native.PreFilterMessage(ref m)) return;
            if(m.Msg == Native.WM_TABLET_QUERYSYSTEMGESTURESTATUS)
            {
                m.Result = (IntPtr)Native.TABLET_DISABLE_FLICKS;
                return;
            }
            base.WndProc(ref m);
        }

        private void timer_tick(object sender, EventArgs e)
        {
            // Search for Pen / Finger
            Touch pen = null;
            Touch mouse = null;
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
                if (t.TouchDevice == TouchDevice.Mouse && t.Down)
                    mouse = t;
            }

            // Eraser
            if (pen != null && (pen.PenFlags & PenFlags.INVERTED) == PenFlags.INVERTED)
            {
                for (int i = 0; i < page.Lines.Count; i++)
                {
                    PointF[] pts = new PointF[1];
                    pts[0] = new PointF(pen.X, pen.Y);
                    transform.GetInverse().Transform(pts);
                    if (page.Lines[i].Collision(new LPoint(pts[0].X, pts[0].Y, pen.Pressure/100)))
                    {
                        gpuRenderer.EditPage();
                        page.Lines.RemoveAt(i--);
                        gpuRenderer.EndEditPage();
                        recreateBufferFull();
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
                transform.GetInverse().Transform(pts);
                Page.SelectArea(pts);
                recreateBufferFull();
            }

            // Move
            if (fingers.Count == 1 && !LockMove)
            {
                Point fp = new Point(fingers[0].X, fingers[0].Y);
                if (lastMove)
                {
                    int deltaX = fp.X - lastMovePoint.X;
                    int deltaY = fp.Y - lastMovePoint.Y;
                    Matrix3x3 mat = Matrix3x3.Translation(deltaX, deltaY);
                    transform *= mat;
                }
                lastMove = true;
                lastMovePoint = fp;
                pen = null;
                recreateBuffer();
            }
            else if(lastMove)
            {
                lastMove = false;
                recreateBufferFull();
            }

            // Transform
            if (fingers.Count == 2
                && !(LockMove && LockScale && LockRotate))
            {
                FingerTransform t = new FingerTransform(fingers[0].X, fingers[0].Y, fingers[1].X, fingers[1].Y);
                if (lastTrans)
                {
                    float dd = 1 + (t.Distance - lastTransInfo.Distance) / lastTransInfo.Distance;
                    float dr = t.Rotation - lastTransInfo.Rotation;
                    Point dp = new Point(t.Position.X - lastTransInfo.Position.X,
                        t.Position.Y - lastTransInfo.Position.Y);

                    if (LockMove) dp = new Point(0, 0);
                    if (LockScale) dd = 1;
                    if (LockRotate) dr = 0;

                    Matrix3x3 mat = new Matrix3x3();
                    mat.TransformTranslate(dp.X, dp.Y);
                    mat.TransformTranslate(-t.Position.X, -t.Position.Y);
                    mat.TransformScale(dd);
                    mat.TransformTranslate(t.Position.X, t.Position.Y);
                    mat.TransformRotateAt(-dr, t.Position.X, t.Position.Y);
                    transform *= mat;
                    recreateBuffer();
                }
                lastTrans = true;
                lastTransInfo = t;
            }
            else if(lastTrans)
            {
                lastTrans = false;
                recreateBufferFull();
            }

            // Draw
            if (line != null)
            {
                if (pen == null && mouse != null)
                    pen = mouse;
                if (pen != null)
                {
                    float rad = line.CalcRad(pen.Pressure, Thicknes);
                    PointF[] p = new PointF[1];
                    p[0] = new PointF(pen.X, pen.Y);
                    linePoints.Add(new PointF(pen.X, pen.Y));
                    transform.GetInverse().Transform(p);
                    line.AddPoint(p[0].X, p[0].Y, rad);
                    RecreateBuffer(Util.GetBounds(linePoints.ToArray()).Expand(8));
                    
                }
                else
                {
                    line.CalcSpline();
                    line.Clamp(new RectangleF(new PointF(0, 0), Page.Format.GetPixelSize()));
                    gpuRenderer.EditPage();
                    page.Lines.Add(line);
                    page.Deselect();
                    gpuRenderer.EndEditPage();
                    //while (gpuRenderer.Drawing) ;
                    //Util.WaitTimeout(gpuRenderer, gpuRenderer.GetType().GetProperty("Drawing"), 500);
                    tmpLine = line;
                    line = null;
                    linePoints = null;
                    recreateBufferFull();
                }
            }
            else if (pen != null)
            {
                pen.Trail.Clear();
                PointF[] p = new PointF[1];
                p[0] = new PointF(pen.X, pen.Y);
                transform.GetInverse().Transform(p);
                linePoints = new List<PointF>();
                if (InkMode == InkMode.Pen)
                {
                    line = new Line();
                    line.Brush = Brush;
                    line.AddPoint(p[0].X, p[0].Y, line.CalcRad(pen.Pressure, Thicknes));
                }
                else if(InkMode == InkMode.Line)
                {
                    line = new Forms.LinearLine();
                    line.Brush = Brush;
                    line.AddPoint(p[0].X, p[0].Y, line.CalcRad(pen.Pressure, Thicknes));
                }
                else if (InkMode == InkMode.Rect)
                {
                    line = new Forms.Rect();
                    line.Brush = Brush;
                    line.AddPoint(p[0].X, p[0].Y, line.CalcRad(pen.Pressure, Thicknes));
                }
                else if (InkMode == InkMode.Arc)
                {
                    line = new Forms.Arc();
                    line.Brush = Brush;
                    line.AddPoint(p[0].X, p[0].Y, line.CalcRad(pen.Pressure, Thicknes));
                }
            }
        }

        private void this_paint(object sender, PaintEventArgs e)
        {
            recreateBufferFull();
        }

        void recreateBuffer()
        {
            this.rBounds = RectangleF.Empty;
            redraw = true;
        }

        void RecreateBuffer(RectangleF bounds)
        {
            this.rBounds = bounds;
            redraw = true;
        }

        int index_num = 0;
        void recreateBufferFull()
        {
            this.rBounds = RectangleF.Empty;
            redraw = true;
            fullredraw = true;
            Console.WriteLine("{0} Redraw", index_num++);
        }

        void RecreateBufferFull(RectangleF bounds)
        {
            this.rBounds = bounds;
            redraw = true;
            fullredraw = true;
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
            p.LoadFromString(txt, null);
            this.page = p;
            recreateBuffer();
            this.Refresh();
        }

        public void LoadPage(KPage page)
        {
            this.page?.BackgroundImage?.UnloadGPU();
            this.page = page;
            Matrix3x3 tmp = transform;
            transform = new Matrix3x3();
            transform.TransformScale(Util.GetScaleFactor());
            recreateBuffer();
            this.Refresh();
        }

        public void RefreshPage()
        {
            recreateBufferFull();
        }

        public void ResetRotation()
        {
            SizeF size = Page.Format.GetPixelSize();
            PointF center = new PointF(size.Width / 2, size.Height / 2);
            transform.Transform(ref center);
            transform.TransformRotateAt(-transform.GetRotation(), center.X, center.Y);
            Refresh();
        }

        public void ResetScale()
        {
            SizeF size = Page.Format.GetPixelSize();
            PointF center = new PointF(size.Width / 2, size.Height / 2);
            transform.Transform(ref center);
            float scale = 1f / transform.GetScale();
            transform.TransformScaleAt(scale, scale, center.X, center.Y);
            Refresh();
        }
        
        void renderLoop()
        {
            gpuRenderer.Init();
            while (running)
            {
                if (redraw || fullredraw)
                {
                    if(lockDraw)
                    {
                        waiting = true;
                        continue;
                    }
                    try
                    {
                        Matrix tMat = transform.CreateGdiMatrix();
                        Matrix dMat = (stableTransform.GetInverse() * transform).CreateGdiMatrix();

                        waiting = false;
                        redraw = false;
                        //if (this.Height <= 0 || this.Width <= 0) return;
                        int vW = (int)(this.Width * BufferSize);
                        int vH = (int)(this.Height * BufferSize);
                        if (rbmp == null || vW != gpuRenderer.Width || vH != gpuRenderer.Height)
                        {
                            gpuRenderer.Resize(new Size(vW, vH));
                            rbmp?.Dispose();
                            rbmp = gpuRenderer.CreateRenderTarget();
                        }
                        if (!gpuRenderer.Begin(SystemColors.ControlDark)) return;
                        SizeF pSize = page.Format.GetPixelSize();
                        if (fullredraw)
                        {
                            stableTransform = transform;
                            dMat?.Dispose();
                            dMat = new Matrix();
                            rbmp.Begin();
                            fullredraw = false;
                            gpuRenderer.SetRenderTarget(rbmp);
                            gpuRenderer.Begin(SystemColors.ControlDark);
                            gpuRenderer.ResetTransform();
                            gpuRenderer.Transform(tMat);
                            
                            if (page.BackgroundImage == null)
                                gpuRenderer.FillRectangle(Color.White, new RectangleF(
                                    0, 0, pSize.Width, pSize.Height));
                            else
                                gpuRenderer.DrawImage(page.BackgroundImage,
                                    new RectangleF(0, 0, pSize.Width, pSize.Height));

                            page.Draw(gpuRenderer);
                            rbmp.End();
                            gpuRenderer.SetRenderTarget(null);
                            tmpLine = null;
                        }

                        gpuRenderer.ResetTransform();
                        gpuRenderer.Transform(dMat);
                        gpuRenderer.DrawRenderBitmap(rbmp);
                        gpuRenderer.ResetTransform();
                        gpuRenderer.Transform(tMat);
                        gpuRenderer.DrawRect(Color.Black, 1, new RectangleF(0, 0, pSize.Width, pSize.Height));
                        if(tmpLine != null)
                        {
                            tmpLine.Render(gpuRenderer);
                        }
                        if (line != null)
                        {
                            line.Render(gpuRenderer);
                        }
                        if (selections != null)
                        {
                            PointF[] pts = selections.ToArray();
                            transform.GetInverse().Transform(pts);
                            gpuRenderer.DrawDashPolygon(pts);
                        }

                        if (rBounds.Equals(RectangleF.Empty))
                            gpuRenderer.End();
                        else
                            gpuRenderer.End(rBounds);

                        tMat.Dispose();
                        dMat.Dispose();
                    }
                    catch(InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (FieldAccessException)
                    {
                        gpuRenderer.End();
                    }
                }
                Thread.Sleep(10);
            }
            gpuRenderer.Dispose();
        }

        void renderLoop2()
        {
            Graphics g = this.CreateGraphics();
            Renderer.BaseRenderer r = g.GetRenderer();
            while (running)
            {
                try
                {
                    //g.Transform = transform.CreateGdiMatrix();
                    //line?.Render(r);
                    //g.Dispose();
                }
                catch (Exception) { }
                Thread.Sleep(10);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!this.IsDisposed)
            {
                Console.WriteLine("Stopping renderer");
                running = false;
                renderThread.Join();
                renderThread2.Join();
                rbmp?.Dispose();
                gpuRenderer?.Dispose();
            }
            try
            {
                base.Dispose(disposing);
            }
            catch(InvalidOperationException)
            {

            }
        }
    }
}
