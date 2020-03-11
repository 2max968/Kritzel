using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MupdfSharp;
using PdfSharp.Pdf;
using pdf = PdfSharp.Pdf;
using pdf_io = PdfSharp.Pdf.IO;

namespace Kritzel.Dialogues
{
    public partial class PDFImporter : Form
    {
        string path;
        public KPage[] Pages { get; private set; }

        public PDFImporter(string path)
        {
            InitializeComponent();
            this.path = path;

            Bitmap[] pages = PageRenderer.Render(path, imgPages.ImageSize.Height);
            imgPages.Images.AddRange(pages);
            for (int i = 0; i < pages.Length; i++)
            {
                ListViewItem itm = new ListViewItem("Page " + (i + 1));
                itm.ImageIndex = i;
                itm.Checked = true;
                lvPages.Items.Add(itm);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itm in lvPages.Items)
                itm.Checked = true;
        }

        private void btnDeselect_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itm in lvPages.Items)
                itm.Checked = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            List<int> pageIndexes = new List<int>();
            for (int i = 0; i < lvPages.Items.Count; i++)
            {
                if (lvPages.Items[i].Checked)
                {
                    pageIndexes.Add(i);
                }
            }
            Bitmap[] bmps = MupdfSharp.PageRenderer.Render(path, 3450, pageIndexes.ToArray());

            Pages = new KPage[pageIndexes.Count];
            pdf.PdfDocument pdfdoc = pdf_io.PdfReader.Open(path, pdf_io.PdfDocumentOpenMode.Modify | pdf_io.PdfDocumentOpenMode.Import);
            for (int i = 0; i < pageIndexes.Count; i++)
            {
                int p = pageIndexes[i];
                KPage page = new KPage();
                pdf.PdfPage pPage = pdfdoc.Pages[p];
                float w = (float)pPage.Width.Millimeter;
                float h = (float)pPage.Height.Millimeter;
                page.Format = new PageFormat(w, h);
                page.Background = null;
                page.OriginalPage = pPage;
                page.BackgroundImage 
                    = new Renderer.Image(bmps[i]);
                Pages[i] = page;
            }
            pdfdoc.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
