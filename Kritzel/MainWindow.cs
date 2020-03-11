using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel
{
    public partial class MainWindow : Form
    {
        float[] sizes = new float[] { 3, 5, 7, 9 };
        PBrush[] brushes = new PBrush[] {PBrush.CreateSolid(Color.Black), PBrush.CreateSolid(Color.Red), PBrush.CreateSolid(Color.Blue),
            PBrush.CreateLinear(new Point(0,0), new Point(16,16), Color.Red, Color.Yellow) };
        FormWindowState tmpWindowState = FormWindowState.Normal;
        KDocument doc = new KDocument();

        public MainWindow()
        {
            InitializeComponent();

            Bitmap[] bmpSizes = new Bitmap[4];
            for(int i = 0; i < bmpSizes.Length; i++)
            {
                int size = 32;
                bmpSizes[i] = new Bitmap(size, size);
                Graphics g = Graphics.FromImage(bmpSizes[i]);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.FillEllipse(Brushes.Black, new RectangleF(size / 2 - sizes[i] / 2, size / 2 - sizes[i] / 2, sizes[i], sizes[i]));
            }
            pickerMenu1.Fill(bmpSizes);

            Bitmap[] bmpBrushes = new Bitmap[brushes.Length];
            for(int i = 0; i < bmpBrushes.Length; i++)
            {
                int size = 48;
                bmpBrushes[i] = new Bitmap(size, size);
                Graphics g = Graphics.FromImage(bmpBrushes[i]);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.FillEllipse(brushes[i].Brush, new RectangleF(0, 0, size, size));
            }
            pickerMenu2.Fill(bmpBrushes);
            pickerMenu2.RightclickItem += PickerMenu2_RightclickItem;

            Bitmap[] bmpModes = new Bitmap[]
            {
                Properties.Resources.Pen,
                Properties.Resources.Line,
                Properties.Resources.Rect
            };
            pickerMenu3.Fill(bmpModes);

            Bitmap[] bmpConstrol = new Bitmap[]
            {
                Properties.Resources.Move,
                Properties.Resources.Resize,
                Properties.Resources.Rotate
            };
            pmControl.Fill(bmpConstrol);
            pmControl.Checked[0] = true;
            pmControl.Checked[1] = true;
            pmControl.Checked[2] = true;
            pmControl.SelectionChanged += PmControl_SelectionChanged;

            Bitmap[] pageControlItms = new Bitmap[]
            {
                Properties.Resources.arrow_up,
                Properties.Resources.arrow_down
            };
            pageSelect.Fill(pageControlItms);
            pageSelect.ClickItem += PageSelect_ClickItem;

            pickerMenu1.SelectionChanged += PickerMenu1_SelectionChanged;
            pickerMenu2.SelectionChanged += PickerMenu2_SelectionChanged;
            pickerMenu3.SelectionChanged += PickerMenu3_SelectionChanged;

            doc.Pages.Add(inkControl1.Page);
        }

        private void PageSelect_ClickItem(PickerMenu sender, int ind)
        {
            int current = doc.Pages.IndexOf(inkControl1.Page);
            if (current < 0)
            {
                inkControl1.LoadPage(doc.Pages[0]);
            }
            else
            {
                if (ind == 0)
                {
                    if (current > 0)
                        inkControl1.LoadPage(doc.Pages[current - 1]);
                }
                else if (ind == 1)
                {
                    if (current < doc.Pages.Count - 1)
                        inkControl1.LoadPage(doc.Pages[current + 1]);
                }
            }
        }

        private void PmControl_SelectionChanged(PickerMenu sender, int e)
        {
            inkControl1.LockMove = !pmControl.Checked[0];
            inkControl1.LockScale = !pmControl.Checked[1];
            inkControl1.LockRotate = !pmControl.Checked[2];
        }

        private void PickerMenu2_RightclickItem(PickerMenu sender, int e)
        {
            BrushCreator c = new BrushCreator();
            c.Brush = brushes[e];
            if(c.ShowDialog() == DialogResult.OK)
            {
                brushes[e] = c.Brush;
                int size = 48;
                pickerMenu2.contents[e] = new Bitmap(size, size);
                Graphics g = Graphics.FromImage(pickerMenu2.contents[e]);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.FillEllipse(brushes[e].Brush, new RectangleF(0, 0, size, size));
                pickerMenu2.Refresh();
            }
        }

        private void PickerMenu3_SelectionChanged(PickerMenu sender, int e)
        {
            switch (e)
            {
                case 0:
                    inkControl1.InkMode = InkMode.Pen;break;
                case 1:
                    inkControl1.InkMode = InkMode.Line;break;
                case 2:
                    inkControl1.InkMode = InkMode.Rect;break;
                case 3:
                    inkControl1.InkMode = InkMode.Arc;break;
            }
        }

        private void PickerMenu2_SelectionChanged(PickerMenu sender, int e)
        {
            inkControl1.Brush = brushes[e];
        }

        private void PickerMenu1_SelectionChanged(PickerMenu sender, int e)
        {
            inkControl1.Thicknes = sizes[e];
        }

        private void pickerMenu2_Load(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Xml|*.xml|All|*";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                string txt = inkControl1.SaveToString();
                File.WriteAllText(sfd.FileName, txt);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Xml|*.xml|All|*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string txt = File.ReadAllText(ofd.FileName);
                inkControl1.LoadFromString(txt);
            }
        }

        private void importImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.bmp;*.png";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(ofd.FileName);
                Dialogues.ImageImporter ii = new Dialogues.ImageImporter(bmp);
                if (ii.ShowDialog() == DialogResult.OK)
                {
                    KPage p = new KPage();
                    p.Format = ii.Format;
                    p.BackgroundImage = ii.EditetImage;
                    inkControl1.LoadPage(p);
                }
            }
        }

        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.FormBorderStyle == FormBorderStyle.None)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = tmpWindowState;
            }
            else
            {
                tmpWindowState = this.WindowState;
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void pageFormatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Dialogues.Sizes().Show();
        }

        private void importPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF|*.pdf";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                KPage page = new KPage();
                page.LoadPDF(ofd.FileName, 1);
                doc.Pages.Add(page);
                inkControl1.LoadPage(page);
            }
        }

        private void exportToPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF|*.pdf";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                Dialogues.ProgressWindow wnd = new Dialogues.ProgressWindow("Save to PDF");
                wnd.TopMost = true;
                wnd.Show();
                try
                {
                    doc.SavePDF(sfd.FileName, wnd.ProgressBar);
                    //inkControl1.Page.SavePDF(sfd.FileName);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error while saving File:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                wnd.Close();
            }
        }

        private void scaleDOwnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(scaleDOwnToolStripMenuItem.Checked)
            {
                inkControl1.BufferSize = 1;
                scaleDOwnToolStripMenuItem.Checked = false;
            }
            else
            {
                inkControl1.BufferSize = .5f;
                scaleDOwnToolStripMenuItem.Checked = true;
            }
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new Form();
            frm.TopMost = true;
            frm.FormBorderStyle = FormBorderStyle.None;
            GUIElements.BackgroundSelectPanel panel = new GUIElements.BackgroundSelectPanel();
            frm.Controls.Add(panel);
            frm.ClientSize = panel.Size;
            panel.Location = new Point(0, 0);
            frm.Show();
            frm.Location = Cursor.Position;
            frm.Activate();
            panel.ItemClicked += new GUIElements.BackgroundSelectPanel.ItemClickEvent(delegate (GUIElements.BackgroundSelectPanel s, Type type) {
                Backgrounds.Background bgr =
                (Backgrounds.Background)type.GetConstructor(new Type[0]).Invoke(new object[0]);
                inkControl1.Page.Background = bgr;
                inkControl1.RefreshPage();
                s.ParentForm.Close();
            });
            frm.Deactivate += new EventHandler(delegate (object s, EventArgs ea)
            {
                frm.Close();
            });
        }

        private void resetRotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inkControl1.ResetRotation();
        }

        private void layoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialogues.LayoutEditor ed = new Dialogues.LayoutEditor(inkControl1.Page, inkControl1);
            ed.ShowDialog();
        }
    }
}
