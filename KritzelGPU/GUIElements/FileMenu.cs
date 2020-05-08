using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Kritzel.WebUI;

namespace Kritzel.Main.GUIElements
{
    public partial class FileMenu : UserControl
    {
        public delegate void CloseMenuEvent();
        public event CloseMenuEvent CloseMenu;

        InkControl control;
        KDocument document;
        Form parent;
        MainWindow window;

        public FileMenu(InkControl control, KDocument document, Form parent, MainWindow window)
        {
            InitializeComponent();

            this.control = control;
            this.document = document;
            this.parent = parent;
            this.window = window;

            int bottom = 0;
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].Bottom > bottom)
                    bottom = Controls[i].Bottom;
            }
            this.Height = bottom;

            foreach(Control c in this.Controls)
            {
                c.MouseUp += C_MouseUp;
            }
        }

        private void C_MouseUp(object sender, MouseEventArgs e)
        {
            CloseMenu?.Invoke();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseMenu?.Invoke();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            KDocument doc = new KDocument();
            doc.Pages.Add(new KPage());
            window.SetDocument(doc);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog diagOpenDoc = new OpenFileDialog();
            diagOpenDoc.Filter = "Supportet Files|*.zip;*.pdf;*.jpg;*.jpeg;*.png;*.bmp|Kritzel Documents|*.zip|PDF Files|*.pdf|Images|*.jpg;*.jpeg;*.bmp;*.png";
            if (diagOpenDoc.ShowDialog(window) == DialogResult.OK)
            {
                FileInfo info = new FileInfo(diagOpenDoc.FileName);
                string ext = info.Extension.ToLower();
                if (ext == ".zip")
                {
                    KDocument doc = new KDocument();
                    MessageLog log = new MessageLog();
                    doc.LoadDocument(diagOpenDoc.FileName, log);
                    Console.WriteLine(log);
                    document = doc;
                    window.SetDocument(document);
                }
                else if (ext == ".pdf")
                {
                    Dialogues.PDFImporter imp = new Dialogues.PDFImporter(diagOpenDoc.FileName);
                    if (imp.ShowDialog(window) == DialogResult.OK)
                    {
                        //control.LockDraw = true;
                        document.Pages.AddRange(imp.Pages);
                    }
                    //control.LockDraw = false;
                }
                else if (ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".png")
                {
                    Bitmap bmp = new Bitmap(diagOpenDoc.FileName);
                    Dialogues.ImageImporter ii = new Dialogues.ImageImporter(bmp);
                    if (ii.ShowDialog(window) == DialogResult.OK)
                    {
                        KPage p = new KPage();
                        p.Format = ii.Format;
                        p.BackgroundImage = new Renderer.Image(ii.EditetImage);
                        p.Background = null;
                        p.ShowDate = false;
                        document.Pages.Add(p);
                        control.LoadPage(p);
                    }
                }
                else
                {
                    MessageBox.Show("Error importing File");
                }
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog diagSaveDoc = new SaveFileDialog();
            diagSaveDoc.Filter = "Kritzel Documents|*.zip|PDF Files|*.pdf|JPEG (Current Page)|*.jpg|Windows Bitmap (Current Page)|*.bmp|Portable Network Graphic (Current Page)|*.png";
            if (diagSaveDoc.ShowDialog(window) == DialogResult.OK)
            {
                FileInfo info = new FileInfo(diagSaveDoc.FileName);
                string ext = info.Extension.ToLower();
                if (ext == ".zip")
                {
                    document.SaveDocument(diagSaveDoc.FileName);
                }
                else if (ext == ".pdf")
                {
                    Dialogues.ProgressWindow wnd = new Dialogues.ProgressWindow("Save to PDF");
                    wnd.TopMost = true;
                    wnd.Show();
                    try
                    {
                        document.SavePDF(diagSaveDoc.FileName, wnd.ProgressBar);
                        //inkControl1.Page.SavePDF(sfd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while saving File:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    wnd.Close();
                }
                else if (ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".png")
                {
                    float factor = 5;
                    KPage page = control.Page;
                    SizeF size = page.Format.GetPixelSize();
                    Bitmap bmp = new Bitmap((int)(size.Width * factor), (int)(size.Height * factor));
                    Graphics g = Graphics.FromImage(bmp);
                    g.Clear(Color.White);
                    Renderer.GdiRenderer r = g.GetRenderer();
                    g.ScaleTransform(factor, factor);
                    page.Draw(r);
                    bmp.Save(diagSaveDoc.FileName);
                }
                else
                {
                    MessageBox.Show("Error export File");
                }
            }
        }

        private void btnAddPage_Click(object sender, EventArgs e)
        {
            //Dialogues.PageAdder pa = new Dialogues.PageAdder(control, document);
            //pa.ShowDialog(window);
            WebDialog wd = new WebDialog("Interface/AddPage.zip");
            wd["formats"] = PageFormat.GetFormats().Keys.ToArray();
            wd["currentFormat"] = "A4";
            if(wd.ShowDialog() == DialogResult.OK)
            {
                int pindex = document.Pages.IndexOf(control.Page);
                int index = pindex;
                if (wd["POS", "1"] == "2")
                    index = pindex + 1;
                else if (wd["POS", "1"] == "3")
                    index = document.Pages.Count;

                KPage page = new KPage();
                string formatStr = wd["FORMAT", "A4"].Replace("+", " ");
                if (PageFormat.GetFormats().ContainsKey(formatStr)) page.Format = PageFormat.GetFormats()[formatStr];
                document.Pages.Insert(index, page);
                control.LoadPage(page);
            }
        }
    }
}
