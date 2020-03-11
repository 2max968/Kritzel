using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel
{
    public class KDocument
    {
        public List<KPage> Pages { get; private set; } = new List<KPage>();

        public void SavePDF(string path, ProgressBar pb = null)
        {
            if(pb != null)
            {
                pb.Maximum = Pages.Count * 2;
                pb.Value = 0;
            }
            MemoryStream mstream = new MemoryStream();
            PdfDocument doc = new PdfDocument();

            for (int i = 0; i < Pages.Count; i++)
            {
                if (Pages[i].OriginalPage != null)
                {
                    PdfPage page = Pages[i].OriginalPage;
                    doc.AddPage(page);
                }
                else
                {
                    PdfPage page = doc.AddPage();
                    page.Width = new XUnit(Pages[i].Format.Width, XGraphicsUnit.Millimeter);
                    page.Height = new XUnit(Pages[i].Format.Height, XGraphicsUnit.Millimeter);
                }
                if (pb != null) pb.Value++;
            }
            doc.Save(mstream);

            PdfDocument doc2 = PdfReader.Open(mstream, PdfDocumentOpenMode.Modify);
            for (int i = 0; i < doc2.Pages.Count; i++)
            {
                XGraphics gfx = XGraphics.FromPdfPage(doc2.Pages[i]);
                SizeF s = Pages[i].Format.GetPixelSize();
                float sX = (float)doc2.Pages[i].Width.Point / s.Width;
                float sY = (float)doc2.Pages[i].Height.Point / s.Height;
                gfx.ScaleTransform(sX, sY);
                Renderer.PdfRenderer r = new Renderer.PdfRenderer(gfx);
                r.RenderSpecial = false;
                Pages[i].Background?.Draw(r, Pages[i].Format, Pages[i].Border,
                    Pages[i].BackgroundColor1, Pages[i].BackgroundColor2);
                foreach (Line l in Pages[i].Lines)
                    l.Render(r);
                if (pb != null) pb.Value++;
            }
            doc2.Save(path);

            mstream.Close();
        }

        /*public void SavePDF(string path, ProgressBar pb = null)
        {
            FileStream stream = File.OpenWrite(path);
            SavePDF(stream, pb);
            stream.Close();
        }*/
    }
}
