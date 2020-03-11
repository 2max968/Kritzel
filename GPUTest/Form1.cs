using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPUTest
{
    public partial class Form1 : Form
    {
        DXFunctions f1;
        DXFunctions f2;
        Stopwatch stp = new Stopwatch();
        int frames = 0;

        public Form1()
        {
            InitializeComponent();

            f1 = new DXFunctions(pictureBox1);
            f2 = new DXFunctions(pictureBox2);

            stp.Start();

            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Dispose();
            f2.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            f1.Begin();
            f2.Begin();
            float dx = (float)Math.Sin(stp.Elapsed.TotalSeconds * 4) * 10;
            float dy = (float)Math.Cos(stp.Elapsed.TotalSeconds * 4) * 10;
            f1.DrawRect(Color.Red, 2, new RectangleF(16 + dx, 16, 32, 32));
            f2.DrawRect(Color.Green, 4, new RectangleF(32, 32 + dy, 64, 64));
            f2.End();
            f1.End();
            frames++;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            float fps = frames / 10f;
            frames = 0;
            this.Text = "FPS: " + fps;
        }
    }
}
