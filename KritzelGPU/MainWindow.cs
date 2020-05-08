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

namespace Kritzel.Main
{
    public partial class MainWindow : Form
    {
        float[] sizes = new float[] { 3, 5, 7, 9 };
        PBrush[] brushes = new PBrush[] {PBrush.CreateSolid(Color.Black),
            PBrush.CreateSolid(Color.FromArgb(255,106,0)),
            PBrush.CreateSolid(Color.FromArgb(0,148,255)),
            PBrush.CreateSolid(Color.FromArgb(38,127,0)) };
        FormWindowState tmpWindowState = FormWindowState.Normal;
        KDocument doc = new KDocument();

        public MainWindow()
        {
            InitializeComponent();
            this.Icon = Program.WindowIcon;

            Bitmap[] bmpSizes = new Bitmap[4];
            for(int i = 0; i < bmpSizes.Length; i++)
            {
                int size = 32;
                bmpSizes[i] = new Bitmap(size, size);
                Graphics g = Graphics.FromImage(bmpSizes[i]);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.FillEllipse(Brushes.Black, new RectangleF(size / 2 - sizes[i] / 2, size / 2 - sizes[i] / 2, sizes[i], sizes[i]));
            }
            pickerMenu1.Fill(bmpSizes);

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
                new Bitmap(64,64),
                Properties.Resources.arrow_down
            };
            pageSelect.Fill(pageControlItms);
            pageSelect.ClickItem += PageSelect_ClickItem;

            pickerMenu1.SelectionChanged += PickerMenu1_SelectionChanged;

            doc.Pages.Add(new KPage());
            inkControl1.LoadPage(doc.Pages[0]);
            inkControl1.Thicknes = sizes[0];

            this.Shown += MainWindow_Shown;

            colorPicker1.SetColor += ColorPicker1_SetColor;
        }

        private void ColorPicker1_SetColor(Color c)
        {
            inkControl1.Brush = PBrush.CreateSolid(c);
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            Kritzel.WebUI.WebDialog.ScaleFactor = Util.GetScaleFactor();
            inkControl1.InitRenderer();
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
                else if (ind == 2)
                {
                    if (current < doc.Pages.Count - 1)
                        inkControl1.LoadPage(doc.Pages[current + 1]);
                }
                else if(ind == 1)
                {
                    if(doc.Pages.Count <= 1)
                    {
                        Dialogues.MsgBox.ShowOk("Cant delete last page");
                    }
                    else if(Dialogues.MsgBox.ShowYesNo("Do you want to remove this Page?"))
                    {
                        KPage todel = doc.Pages[current];
                        int newP = current - 1;
                        if (current == 0) newP = 1;
                        inkControl1.LoadPage(doc.Pages[newP]);
                        if (current > 0) current--;
                        doc.Pages.Remove(todel);
                        todel.Dispose();
                    }
                }
            }
        }

        private void PmControl_SelectionChanged(PickerMenu sender, int e)
        {
            inkControl1.LockMove = !pmControl.Checked[0];
            inkControl1.LockScale = !pmControl.Checked[1];
            inkControl1.LockRotate = !pmControl.Checked[2];
        }

        private void PickerMenu1_SelectionChanged(PickerMenu sender, int e)
        {
            inkControl1.Thicknes = sizes[e];
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            doc.Dispose();
        }

        private void btnFullscreen_Click(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == FormBorderStyle.None)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = tmpWindowState;
                btnFullscreen.BackgroundImage = Properties.Resources.ArrowsExpand;
            }
            else
            {
                tmpWindowState = this.WindowState;
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                btnFullscreen.BackgroundImage = Properties.Resources.arrowShrink;
            }
        }

        private void btnLayout_Click(object sender, EventArgs e)
        {
            Dialogues.LayoutEditor ed = new Dialogues.LayoutEditor(inkControl1.Page, inkControl1);
            ed.ShowDialog();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            GUIElements.FileMenu menu = new GUIElements.FileMenu(inkControl1, doc, this, this);
            this.Controls.Add(menu);
            menu.Location = this.PointToClient(btnFile.PointToScreen(btnFile.Location));
            menu.BringToFront();
            menu.CloseMenu += new GUIElements.FileMenu.CloseMenuEvent(delegate ()
            {
                this.Controls.Remove(menu);
                menu.Dispose();
            });
        }

        private void btnFormType_Click(object sender, EventArgs e)
        {
            menuFormType.Show(btnFormType.PointToScreen(new Point(0, btnFormType.Height)));
        }

        private void strokeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFormType.BackgroundImage = Properties.Resources.Pen;
            inkControl1.InkMode = InkMode.Pen;
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFormType.BackgroundImage = Properties.Resources.Line;
            inkControl1.InkMode = InkMode.Line;
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFormType.BackgroundImage = Properties.Resources.Rect;
            inkControl1.InkMode = InkMode.Rect;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(diagOpenDoc.ShowDialog() == DialogResult.OK)
            {
                FileInfo info = new FileInfo(diagOpenDoc.FileName);
                string ext = info.Extension.ToLower();
                if(ext == ".zip")
                {
                    KDocument doc = new KDocument();
                    doc.LoadDocument(diagOpenDoc.FileName, null);
                    this.doc = doc;
                    inkControl1.LoadPage(doc.Pages[0]);
                }
                else if(ext == ".pdf")
                {
                    Dialogues.PDFImporter imp = new Dialogues.PDFImporter(diagOpenDoc.FileName);
                    if (imp.ShowDialog() == DialogResult.OK)
                    {
                        inkControl1.LockDraw = true;
                        doc.Pages.AddRange(imp.Pages);
                    }
                    inkControl1.LockDraw = false;
                }
                else if(ext == ".jpg" || ext == ".jpeg" || ext == ".bmp"|| ext == ".png")
                {
                    Bitmap bmp = new Bitmap(diagOpenDoc.FileName);
                    Dialogues.ImageImporter ii = new Dialogues.ImageImporter(bmp);
                    if (ii.ShowDialog() == DialogResult.OK)
                    {
                        KPage p = new KPage();
                        p.Format = ii.Format;
                        p.BackgroundImage = new Renderer.Image(ii.EditetImage);
                        p.Background = null;
                        p.ShowDate = false;
                        inkControl1.LoadPage(p);
                    }
                }
                else
                {
                    MessageBox.Show("Error importing File");
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doc = new KDocument();

        }

        public void SetDocument(KDocument document)
        {
            doc = document;
            inkControl1.LoadPage(doc.Pages[0]);
        }
    }
}
