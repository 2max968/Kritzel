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
    public partial class Measurement : Form
    {
        public Measurement()
        {
            InitializeComponent();
            float inch = 0.0393701f;
            float dpi = this.CreateGraphics().DpiX;

            Bitmap bmp = new Bitmap(200, 30);
            Graphics g = Graphics.FromImage(bmp);
            float dpmm = dpi * inch * 1.0f;
            for (float i = 0; i < bmp.Width; i += dpmm * 10)
                g.DrawLine(Pens.Black, i, 0, i, 30);
            this.BackgroundImage = bmp;
        }
    }
}
