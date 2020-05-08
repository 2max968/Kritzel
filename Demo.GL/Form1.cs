using Kritzel.GLRenderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using pgl = Kritzel.GLRenderer;

namespace Demo.GL
{
    public partial class Form1 : Form
    {
        RenderContext ctx = null;
        bool timerLog = true;
        Thread thread = null;
        bool running = true;

        float animationTime = 0;

        int tex = 0;
        uint fbo = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void startSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startSyncToolStripMenuItem.Enabled = false;
            startAsyncToolStripMenuItem.Enabled = false;
            initGlewToolStripMenuItem.Enabled = true;

            Console.WriteLine("Creating Sync Renderer");
            ctx = new RenderContext(panel1);
            Console.WriteLine("Initializing Sync Renderer");
            ctx.InitAsync();
            Console.WriteLine("Make Renderer Current");
            ctx.MakeCurrent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            render(timer1.Interval / 1000f);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(thread != null && thread.IsAlive)
            {
                running = false;
                thread.Join();
            }
            ctx?.Dispose();
        }

        void render(float dt)
        {
            animationTime += dt;

            if (timerLog) Console.WriteLine("Set clear Color");
            Opengl32.glClearColor(0, 0, 0, 1);
            if (timerLog) Console.WriteLine("Clear Screen");
            Opengl32.glClear(GLConsts.GL_COLOR_BUFFER_BIT);

            pgl.Opengl32.glLoadIdentity();
            float cx = (float)Math.Cos(animationTime);
            float sx = (float)Math.Sin(animationTime);
            pgl.Opengl32.glLoadMatrixf(new float[] {
                cx, sx, 0, 0,
                -sx,cx,0,0,
                0,0,1,0,
                0,0,0,1
            });
            if (timerLog) Console.WriteLine("Begin triangle");
            pgl.GL.Begin(PrimitiveType.TRIANGLES);
            pgl.GL.Color(Color.Yellow);
            pgl.GL.Vertex2(0, -.75f);
            pgl.GL.Color(Color.Green);
            pgl.GL.Vertex2(.75f, .75f);
            pgl.GL.Color(Color.Red);
            pgl.GL.Vertex2(-.75f, .75f);
            pgl.GL.End();
            if (timerLog) Console.WriteLine("Ended Triangle");

            if (timerLog) Console.WriteLine("Swap Buffer");
            ctx.SwapBuffer();
            timerLog = false;
        }

        private void startAsyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startSyncToolStripMenuItem.Enabled = false;
            startAsyncToolStripMenuItem.Enabled = false;

            Console.WriteLine("Create Async renederer");
            ctx = new RenderContext(panel1);
            thread = new Thread(delegate ()
            {
                Console.WriteLine("Initialize Async renderer");
                ctx.InitAsync();
                Console.WriteLine("Make Current");
                ctx.MakeCurrent();

                while(running)
                {
                    render(.06f);
                    Thread.Sleep(60);
                }
            });
            thread.Start();
        }

        private void initGlewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.Write("Initialize ");
            int ret = pgl.NativeGL.nglInit();
            Console.WriteLine(ret);
            Console.WriteLine("Create Texture");
            tex = pgl.Util.LoadTexture(IntPtr.Zero, 100, 100);
            Console.WriteLine("Texture ID: " + tex);
            Console.WriteLine("Create FBO");
            fbo = pgl.NativeGL.nglCreateFBO(tex, 100, 100);
            Console.WriteLine("Bind FBO");
            pgl.NativeGL.nglBindFramebuffer(fbo);
            Console.WriteLine("Draw in FBO");
            pgl.Opengl32.glClear(GLConsts.GL_COLOR_BUFFER_BIT);
            pgl.GL.Begin(PrimitiveType.LINES);
            pgl.GL.Color(Color.Lime);
            pgl.GL.Vertex2(-.5f, -.5f);
            pgl.GL.Vertex2(.5f, .5f);
            pgl.GL.End();
            Console.WriteLine("Unbind FBO");
            pgl.NativeGL.nglBindFramebuffer(0);
        }
    }
}
