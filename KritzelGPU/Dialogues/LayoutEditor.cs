using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel.Dialogues
{
    public partial class LayoutEditor : Form
    {
        KPage page;
        InkControl inkControl;

        public LayoutEditor(KPage page, InkControl inkControl = null)
        {
            InitializeComponent();
            this.page = page;
            this.inkControl = inkControl;
            backgroundSelectPanel1.ItemClicked += BackgroundSelectPanel1_ItemClicked;
            label1.Text = (page.Border / 10f) + "cm";
            this.StartPosition = FormStartPosition.CenterParent;
            cbShowDate.Checked = page.ShowDate;
        }

        private void BackgroundSelectPanel1_ItemClicked(GUIElements.BackgroundSelectPanel sender, Type bgrType)
        {
            Backgrounds.Background bgr = (Backgrounds.Background)bgrType.GetConstructor(new Type[0])
                .Invoke(new object[0]);
            page.Background = bgr;
            inkControl?.RefreshPage();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (page.Border < 30)
                page.Border += 5f;
            label1.Text = (page.Border / 10f) + "cm";
            inkControl?.RefreshPage();
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (page.Border > 0)
                page.Border -= 5f;
            label1.Text = (page.Border / 10f) + "cm";
            inkControl?.RefreshPage();
        }

        void bColor()
        {
            Color c1 = page.BackgroundColor1, c2 = page.BackgroundColor2;
            Bitmap bmp1 = new Bitmap(16, 16), bmp2 = new Bitmap(16, 16);
            Graphics g1 = Graphics.FromImage(bmp1), g2 = Graphics.FromImage(bmp2);
            g1.Clear(c1);
            g2.Clear(c2);
            btnC1.Image = bmp1;
            btnC2.Image = bmp2;
        }

        private void btnC1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = page.BackgroundColor1;
            if(cd.ShowDialog() == DialogResult.OK)
            {
                page.BackgroundColor1 = cd.Color;
            }
            bColor();
            inkControl?.RefreshPage();
        }

        private void btnC2_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = page.BackgroundColor2;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                page.BackgroundColor2 = cd.Color;
            }
            bColor();
            inkControl?.RefreshPage();
        }

        private void cbShowDate_CheckedChanged(object sender, EventArgs e)
        {
            page.ShowDate = cbShowDate.Checked;
            inkControl?.RefreshPage();
        }
    }
}
