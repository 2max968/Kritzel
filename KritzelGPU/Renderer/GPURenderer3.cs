using PInvokeGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using gdi = System.Drawing;
using gdi2d = System.Drawing.Drawing2D;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;

namespace Kritzel.Renderer
{
    public class GPURenderer3 : GPURenderer
    {
        bool stateDraw = false;
        bool tLock = false;
        Matrix3x3 Transformation = new Matrix3x3();
        System.Windows.Forms.Control cltr;
        RenderContext ctx;
        gdi.Bitmap ellipse;
        int texEllipse;
        float[] pEllipseX;
        float[] pEllipseY;
        uint ellipseShader;

        public GPURenderer3(System.Windows.Forms.Control cltr)
        {
            this.cltr = cltr;
            Console.WriteLine("Create Renderer {0}", Thread.CurrentThread.ManagedThreadId);
            ctx = new RenderContext(cltr);
            IsHardware = true;
            float scale = GetScaleFactor();
            Width = cltr.Width;
            Height = cltr.Height;

            Opengl32.glEnable(GLConsts.GL_MULTISAMPLE);
        }

        public override void Init()
        {
            ctx.InitAsync();
            ctx.MakeCurrent();
            NativeGL.nglInit();
            Opengl32.glBlendFunc(GLConsts.GL_SRC_ALPHA, GLConsts.GL_ONE_MINUS_SRC_ALPHA);
            Opengl32.glEnable(GLConsts.GL_BLEND);

            int eSize = 32;
            ellipse = new gdi.Bitmap(eSize, eSize);
            var g = gdi.Graphics.FromImage(ellipse);
            g.Clear(gdi.Color.Transparent);
            g.SmoothingMode = gdi2d.SmoothingMode.HighQuality;
            g.FillEllipse(gdi.Brushes.White, new RectangleF(1, 1, eSize-2, eSize-2));
            texEllipse = PInvokeGL.Util.LoadTexture(ellipse);

            int eDet = 16;
            pEllipseX = new float[3 * eDet];
            pEllipseY = new float[3 * eDet];
            float l_x = 1, l_y = 0;
            for(int i = 0; i < eDet; i++)
            {
                float arg = (i + 1) / (float)eDet * 2 * (float)Math.PI;
                float x = (float)Math.Cos(arg);
                float y = (float)Math.Sin(arg);
                pEllipseX[i * 3] = l_x;
                pEllipseY[i * 3] = l_y;
                pEllipseX[i * 3 + 1] = 0;
                pEllipseY[i * 3 + 1] = 0;
                l_x = x;l_y = y;
                pEllipseX[i * 3 + 2] = x;
                pEllipseY[i * 3 + 2] = y;
            }

            int success = NativeGL.nglCreateProgram(ref ellipseShader, Properties.Resources.vertex_vs, Properties.Resources.ellipse_fs);
            if (success != NativeGL.SHADER_NOERROR)
            {
                Console.WriteLine("[Error] Creating Shader: {0}", success);
                int len = 0;
                IntPtr infoLog = NativeGL.nglGetInfoLog(ref len);
                byte[] infoLogStr = new byte[len];
                Marshal.Copy(infoLog, infoLogStr, 0, len);
                string str = Encoding.ASCII.GetString(infoLogStr);
                Console.WriteLine(str.TrimEnd(' ', '\n', '\r'));
            }
        }

        public override void Dispose()
        {
            if (Disposed) return;
            ctx.Dispose();
        }

        public static void DisposeStatic()
        {

        }

        public override void Resize(gdi.Size size)
        {
            if (stateDraw) return;
            this.Width = Math.Max(size.Width, 1);
            this.Height = Math.Max(size.Height, 1);
            ctx.Resize(new gdi.Rectangle(0, 0, (int)this.Width, (int)this.Height));
        }

        public override void DrawLine(gdi.Color c, float width, PointF p1, PointF p2, bool capStart = false, bool capEnd = false)
        {
            GL.Color(c);
            GL.Begin(PrimitiveType.QUADS);
            drawLine(width, p1, p2);
            GL.End();
            if (capStart)
                FillEllipse(null, new RectangleF(p1.X - width / 2, p1.Y - width / 2,
                    width, width));
            if (capEnd)
                FillEllipse(null, new RectangleF(p2.X - width / 2, p2.Y - width / 2,
                    width, width));
        }

        void drawLine(float width, PointF p1, PointF p2)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            PointF p_1, p_2, p_3, p_4;
            if (Math.Abs(dy) > Math.Abs(dx))
            {
                float m = -dx / dy;
                float sx = width / (float)Math.Sqrt(4 * m * m + 4);
                float sy = m * sx;

                p_1 = new PointF(p1.X + sx, p1.Y + sy);
                p_2 = new PointF(p1.X - sx, p1.Y - sy);
                p_3 = new PointF(p2.X - sx, p2.Y - sy);
                p_4 = new PointF(p2.X + sx, p2.Y + sy);
            }
            else
            {
                float m = -dy / dx;
                float sy = width / (float)Math.Sqrt(4 * m * m + 4);
                float sx = m * sy;

                p_1 = new PointF(p1.X + sx, p1.Y + sy);
                p_2 = new PointF(p1.X - sx, p1.Y - sy);
                p_3 = new PointF(p2.X - sx, p2.Y - sy);
                p_4 = new PointF(p2.X + sx, p2.Y + sy);
            }
            
            GL.Vertex2(p_1);
            GL.Vertex2(p_2);
            GL.Vertex2(p_3);
            GL.Vertex2(p_4);
        }

        public override void FillEllipse(PBrush c, RectangleF rect)
        {
            float x = (rect.Left + rect.Right) / 2f;
            float y = (rect.Top + rect.Bottom) / 2f;
            float rx = (rect.Right - rect.Left) / 2f;
            float ry = (rect.Bottom - rect.Top) / 2f;
            if (c != null) GL.Color(c.GetColors()[0]);
            GL.Begin(PrimitiveType.TRIANGLES);
            NativeGL.nglVertex2fTrans(pEllipseX.Length, pEllipseX, pEllipseY, x, y, rx, ry);
            GL.End();
        }

        public override void DrawEllipse(PBrush c, float width, RectangleF rect)
        {
            //TODO: Ellipse
            GL.Begin(PrimitiveType.LINE_STRIP);
            if(c != null) GL.Color(c.GetColors()[0]);
            Opengl32.glLineWidth(width);
            GL.Vertex2(rect.Left, rect.Top);
            GL.Vertex2(rect.Right, rect.Top);
            GL.Vertex2(rect.Right, rect.Bottom);
            GL.Vertex2(rect.Left, rect.Bottom);
            GL.End();
        }

        public override void DrawRoundedLine(PBrush c, float width, PointF p1, PointF p2)
        {
            DrawLine(c.GetColors()[0], width, p1, p2, true, true);
        }

        public override void DrawRoundedRectangle(PBrush c, float width, RectangleF rect)
        {
            GL.Color(c.GetColors()[0]);
            PointF p1 = new PointF(rect.Left, rect.Top);
            PointF p2 = new PointF(rect.Right, rect.Top);
            PointF p3 = new PointF(rect.Right, rect.Bottom);
            PointF p4 = new PointF(rect.Left, rect.Bottom);
            GL.Begin(PrimitiveType.QUADS);
            drawLine(width, p1, p2);
            drawLine(width, p2, p3);
            drawLine(width, p3, p4);
            drawLine(width, p4, p1);
            GL.End();
            
            BeginCircles(null);
            Circle(p1.X, p1.Y, width / 2);
            Circle(p2.X, p2.Y, width / 2);
            Circle(p3.X, p3.Y, width / 2);
            Circle(p4.X, p4.Y, width / 2);
            EndCircle();
        }

        public override void DrawRect(gdi.Color c, float width, RectangleF rect)
        {
            GL.Color(c);
            PointF p1 = new PointF(rect.Left, rect.Top);
            PointF p2 = new PointF(rect.Right, rect.Top);
            PointF p3 = new PointF(rect.Right, rect.Bottom);
            PointF p4 = new PointF(rect.Left, rect.Bottom);
            GL.Begin(PrimitiveType.QUADS);
            drawLine(width, p1, p2);
            drawLine(width, p2, p3);
            drawLine(width, p3, p4);
            drawLine(width, p4, p1);
            GL.End();
        }

        ~GPURenderer3()
        {
            Dispose();
        }

        public override void ResetTransform()
        {
            Transformation = new Matrix3x3();
            Opengl32.glLoadIdentity();
        }

        public override void Transform(gdi2d.Matrix m)
        {
            Matrix3x3 mat33 = new Matrix3x3(m.Elements[0], m.Elements[1], m.Elements[2],
                m.Elements[3], m.Elements[4], m.Elements[5]);
            Transformation *= mat33;
            PInvokeGL.Util.LoadMatrix3x3(mat33.GetRowMatrix());
        }

        public override bool Begin()
        {
            if (stateDraw) return false;
            while (tLock) ;
            stateDraw = true;
            ctx.MakeCurrent();
            return true;
        }

        public override bool Begin(gdi.Color color)
        {
            if (stateDraw) return false;
            Begin();
            Opengl32.glClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
            Opengl32.glClear(GLConsts.GL_COLOR_BUFFER_BIT);
            ctx.Resize();
            return true;
        }

        public override void End()
        {
            if (!stateDraw) return;
            
            /*NativeGL.nglUseProgram(ellipseShader);
            float x1 = 0, y1 = 0, x2 = 100, y2 = 100;
            GL.Color(1, 1, 1, 1);
            GL.Begin(PrimitiveType.QUADS);
            GL.TexCoord(0, 0); GL.Vertex2(x1, y1);
            GL.TexCoord(1, 0); GL.Vertex2(x2, y1);
            GL.TexCoord(1, 1); GL.Vertex2(x2, y2);
            GL.TexCoord(0, 1); GL.Vertex2(x1, y2);
            GL.End();
            NativeGL.nglUseProgram(0);*/

            ctx.SwapBuffer();
            stateDraw = false;
        }

        public override void End(RectangleF rect)
        {
            if (!stateDraw) return;
            ctx.SwapBuffer();
            stateDraw = false;
        }

        public override void FillRectangle(gdi.Color c, RectangleF rect)
        {
            GL.Color(c);
            GL.Begin(PrimitiveType.QUADS);
            GL.Vertex2(rect.Left, rect.Top);
            GL.Vertex2(rect.Right, rect.Top);
            GL.Vertex2(rect.Right, rect.Bottom);
            GL.Vertex2(rect.Left, rect.Bottom);
            GL.End();
        }

        public override void EditPage()
        {
            //tLock = true;
            //while (stateDraw) ;
        }

        public override void EndEditPage()
        {
            //tLock = false;
        }

        public override void DrawImage(Image img, RectangleF rect)
        {
            if (img.GLTextureID == 0)
            {
                img.LoadGPU();
            }
            Opengl32.glBindTexture(GLConsts.GL_TEXTURE_2D, img.GLTextureID);
            Opengl32.glEnable(GLConsts.GL_TEXTURE_2D);
            GL.Color(1, 1, 1, 1);
            GL.Begin(PrimitiveType.QUADS);
            GL.TexCoord(0, 0);GL.Vertex2(rect.Left, rect.Top);
            GL.TexCoord(1, 0); GL.Vertex2(rect.Right, rect.Top);
            GL.TexCoord(1, 1); GL.Vertex2(rect.Right, rect.Bottom);
            GL.TexCoord(0, 1); GL.Vertex2(rect.Left, rect.Bottom);
            GL.End();
            Opengl32.glDisable(GLConsts.GL_TEXTURE_2D);
        }

        public override void DrawDashPolygon(PointF[] pts)
        {
            GL.Color(gdi.Color.Gray);
            GL.Begin(PrimitiveType.QUADS);
            for (int i = 0; i < pts.Length - 1; i++)
            {
                drawLine(4, pts[i], pts[i + 1]);
            }
            GL.End();
            GL.Begin(PrimitiveType.LINES);
            GL.Vertex2(pts[pts.Length - 1]);
            GL.Vertex2(pts[0]);
            GL.End();
        }

        public override void FillPolygon(PBrush b, PointF[] pts)
        {
            /*GL.Begin(PrimitiveType.Polygon);
            GL.Color3(b.GetColors()[0]);
            for (int i = 0; i < pts.Length; i++)
            {
                GL.Vertex2(pts[i].X, pts[i].Y);
            }
            GL.End();*/
        }

        public override void DrawText(string str, PBrush brush, RectangleF rect, float size)
        {
            size = Util.MmToPoint(size);
            rect = Util.MmToPoint(rect);
            gdi.Graphics dummy = gdi.Graphics.FromHwnd(IntPtr.Zero);
            gdi.Font ft = new gdi.Font("Calibri", size);
            gdi.SizeF s = dummy.MeasureString(str, ft);
            s.Height *= 2;
            gdi.Bitmap bmp = new gdi.Bitmap((int)s.Width, (int)s.Height);
            gdi.Color c1 = brush.GetColors()[0];
            gdi.Color c2 = gdi.Color.FromArgb(0, c1.R, c1.G, c1.B);
            gdi.SolidBrush b = new gdi.SolidBrush(c1);
            gdi.Graphics g = gdi.Graphics.FromImage(bmp);
            g.Clear(c2);
            g.DrawString(str, ft, b, new PointF(0, 0));

            int tex = PInvokeGL.Util.LoadTexture(bmp);
            Opengl32.glEnable(GLConsts.GL_TEXTURE_2D);
            Opengl32.glBindTexture(GLConsts.GL_TEXTURE_2D, tex);
            rect = new RectangleF(rect.X, rect.Y, s.Width, s.Height);
            GL.Color(1, 1, 0, 1);
            GL.Begin(PrimitiveType.QUADS);
            GL.TexCoord(0, 0);GL.Vertex2(rect.Left, rect.Top);
            GL.TexCoord(1, 0);GL.Vertex2(rect.Right, rect.Top);
            GL.TexCoord(1, 1);GL.Vertex2(rect.Right, rect.Bottom);
            GL.TexCoord(0, 1);GL.Vertex2(rect.Left, rect.Bottom);
            GL.End();
            Opengl32.glDisable(GLConsts.GL_TEXTURE_2D);

            Opengl32.glDeleteTextures(1, ref tex);
            b.Dispose();
            bmp.Dispose();
        }

        public override RenderBitmap CreateRenderTarget()
        {
            return new RenderBitmap3((int)Width, (int)Height);
        }

        public override void SetRenderTarget(RenderBitmap bmp)
        {
            if (bmp == null || !(bmp is RenderBitmap3))
                NativeGL.nglBindFramebuffer(0);
            else
                ((RenderBitmap3)bmp).FBO.Bind();
        }

        public override void DrawRenderBitmap(RenderBitmap bmp)
        {
            float x1 = 0, y1 = 0, x2 = Width, y2 = Height;
            if (bmp == null || !(bmp is RenderBitmap3))
                return;

            Opengl32.glEnable(GLConsts.GL_TEXTURE_2D);
            Opengl32.glBindTexture(GLConsts.GL_TEXTURE_2D, ((RenderBitmap3)bmp).FBO.Texture);

            GL.Color(1, 1, 1, 1);
            GL.Begin(PrimitiveType.QUADS);
            GL.TexCoord(0, 1); GL.Vertex2(x1, y1);
            GL.TexCoord(1, 1); GL.Vertex2(x2, y1);
            GL.TexCoord(1, 0); GL.Vertex2(x2, y2);
            GL.TexCoord(0, 0); GL.Vertex2(x1, y2);
            GL.End();
            Opengl32.glDisable(GLConsts.GL_TEXTURE_2D);
        }

        public override float GetScaleFactor()
        {
            return Util.GetScaleFactor();
        }

        public override void DrawText(string text, PointF pos, float size, gdi.Color c)
        {
            //TODO: Text
        }

        public override void BeginCircles(PBrush brush)
        {
            if (brush != null) GL.Color(brush.GetColors()[0]);
            GL.Begin(PrimitiveType.TRIANGLE_STRIP);
        }

        public override void Circle(float x, float y, float r)
        {
            NativeGL.nglVertex2fTrans(pEllipseX.Length, pEllipseX, pEllipseY,
                x, y, r, r);
        }

        public override void EndCircle()
        {
            GL.End();
        }
    }
}