using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using Kritzel.WebUI;
using System.Globalization;

namespace Kritzel.Main.GUIElements
{
    public partial class ColorPicker : UserControl
    {
        public delegate void SetColorEvent(Color c);
        public event SetColorEvent SetColor;

        List<Color> colors = new List<Color>();
        List<Button> buttons = new List<Button>();
        List<Bitmap> icons = new List<Bitmap>();
        int ocap = 0;

        public ColorPicker()
        {
            InitializeComponent();

            this.Resize += ColorPicker_Resize;

            string path = "colors.cfg";
            if (File.Exists(path))
            {
                string[] entries = File.ReadAllLines(path);
                for (int i = 0; i < entries.Length; i++)
                {
                    if (entries[i].Length >= 6)
                    {
                        try
                        {
                            string sr = entries[i].Substring(0, 2).Trim();
                            string sg = entries[i].Substring(2, 2).Trim();
                            string sb = entries[i].Substring(4, 2).Trim();
                            int r = Convert.ToInt16(sr, 16);
                            int g = Convert.ToInt16(sg, 16);
                            int b = Convert.ToInt16(sb, 16);
                            Add(Color.FromArgb((byte)r, (byte)g, (byte)b));
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            refresh();
        }

        private void ColorPicker_Resize(object sender, EventArgs e)
        {
            refresh();
        }

        public void Add(Color color)
        {
            Bitmap icon = createIcon(color);
            colors.Add(color);
            icons.Add(icon);
            refresh(true);
        }

        public int GetCapacity()
        {
            int width = this.Width - 128;
            return width / 64;
        }

        void save()
        {
            string path = "colors.cfg";
            Stream s = File.OpenWrite(path);
            StreamWriter sw = new StreamWriter(s);
            foreach(Color c in colors)
            {
                sw.WriteLine(string.Format("{0,2:X}{1,2:X}{2,2:X}", c.R, c.G, c.B));
            }
            sw.Flush();
            s.Flush();
            s.Close();
        }

        Bitmap createIcon(Color c)
        {
            float factor = 1f;
            Bitmap bmp = new Bitmap((int)(64 * factor), (int)(64 * factor));
            Graphics g = Graphics.FromImage(bmp);
            ImageAttributes ia = new ImageAttributes();
            var cm = new ColorMatrix(new float[][]
            {
              new float[] {c.R / 255f, 0, 0, 0, 0},
              new float[] {0, c.G / 255f, 0, 0, 0},
              new float[] {0, 0, c.B / 255f, 0, 0},
              new float[] {0, 0, 0, 1, 0},
              new float[] {0, 0, 0, 0, 1}
            });
            ia.SetColorMatrix(cm);
            g.DrawImage(Properties.Resources.feltpen, new Rectangle(0, 0, (int)(64 * factor), (int)(64 * factor)),
                0, 0, 64, 64, GraphicsUnit.Pixel, ia);
            ia.Dispose();
            g.Dispose();
            return bmp;
        }

        void refresh(bool full = false)
        {
            if (!full && GetCapacity() == ocap) return;
            ocap = GetCapacity();
            if (!full && ocap > colors.Count) return;

            while(buttons.Count > 0)
            {
                this.Controls.Remove(buttons[0]);
                buttons[0].Dispose();
                buttons.RemoveAt(0);
            }

            int cap = GetCapacity();
            for(int i = 0; i < cap && i < colors.Count; i++)
            {
                Button btn = new Button();
                btn.Size = new Size(64, 64);
                btn.Tag = colors[i];
                btn.Image = icons[i];
                btn.Text = "";
                buttons.Add(btn);
                btn.Dock = DockStyle.Left;
                Controls.Add(btn);
                btn.MouseClick += Btn_MouseClick;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].BringToFront();
            }
            btnAdd.BringToFront();

            btnExpand.Enabled = colors.Count > buttons.Count;
        }

        private void Btn_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                if (e.Button == MouseButtons.Left)
                {
                    if (btn.Tag is Color)
                    {
                        Color c = (Color)btn.Tag;
                        foreach (Button b in buttons)
                        {
                            b.BackColor = default(Color);
                            b.UseVisualStyleBackColor = true;
                        }
                        btn.BackColor = Color.Tomato;
                        SetColor?.Invoke(c);
                    }
                }
                else if(e.Button == MouseButtons.Right)
                {
                    penCtx.Tag = buttons.IndexOf(btn);
                    penCtx.Show(Cursor.Position);
                }
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            WebDialog cp = new WebDialog("Interface/ColorPicker.zip");
            //cp["colors"] = new Color[] { Color.Red, Color.Orange, Color.Blue, Color.Black };
            //cp["currentColor"] = Color.Black;
            if(cp.ShowDialog() == DialogResult.OK)
            {
                if (!cp.Vars.ContainsKey("more"))
                {
                    string colorStr = cp["color", "000000"];
                    if (colorStr.Length == 6)
                    {
                        string sr = "" + colorStr.Substring(0, 2);
                        string sg = "" + colorStr.Substring(2, 2);
                        string sb = "" + colorStr.Substring(4, 2);

                        int r, g, b;
                        int.TryParse(sr, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out r);
                        int.TryParse(sg, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out g);
                        int.TryParse(sb, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out b);
                        Color c = Color.FromArgb(r, g, b);
                        Add(c);
                        save();
                    }
                }
                else
                {
                    ColorDialog cd = new ColorDialog();
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        Add(cd.Color);
                        save();
                    }
                }
            }
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
