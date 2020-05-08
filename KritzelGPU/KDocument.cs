using Ionic.Zip;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Kritzel.Main
{
    public class KDocument : IDisposable
    {
        public List<KPage> Pages { get; private set; } = new List<KPage>();
        public List<KPage> Deletet { get; private set; } = new List<KPage>();
        public bool IsDisposed { get; private set; } = false;

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
                /*Pages[i].Background?.Draw(r, Pages[i].Format, Pages[i].Border,
                    Pages[i].BackgroundColor1, Pages[i].BackgroundColor2);
                foreach (Line l in Pages[i].Lines)
                    l.Render(r);*/
                Pages[i].Draw(r);
                if (pb != null) pb.Value++;
            }
            doc2.Save(path);

            mstream.Close();
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            foreach (KPage page in Pages)
                page.Dispose();
        }

        public void SaveDocument(string path)
        {
            List<FileInfo> files = new List<FileInfo>();
            DirectoryInfo dir = new DirectoryInfo("tmp\\" + Process.GetCurrentProcess().Id);
            if (dir.Exists)
                dir.RemoveAll();
            dir.Create();
            FileInfo docFile = new FileInfo(dir + "\\document.kritzel");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\n";
            files.Add(docFile);
            using (XmlWriter writer = XmlWriter.Create(docFile.FullName, settings))
            {
                PdfDocument pdfDoc = new PdfDocument();
                writer.WriteStartElement("Document");
                writer.WriteStartElement("Pages");
                for(int i = 0; i < Pages.Count; i++)
                {
                    writer.WriteStartElement("Page");
                    FileInfo pageFile = new FileInfo(dir + "\\Page" + i + ".xml");
                    string page = Pages[i].SaveToString();
                    File.WriteAllText(pageFile.FullName, page);
                    files.Add(pageFile);
                    writer.WriteAttributeString("src", pageFile.Name);
                    string pdf = "-1";
                    if(Pages[i].OriginalPage != null)
                    {
                        string pdfName = "Page" + i + ".pdf";
                        pdf = "" + pdfDoc.Pages.Count;
                        pdfDoc.AddPage(Pages[i].OriginalPage);
                    }
                    writer.WriteAttributeString("pdf", pdf);
                    writer.WriteEndElement(); // Page
                }
                writer.WriteEndElement(); // Pages
                writer.WriteEndElement(); // Document
                writer.Close();

                if (pdfDoc.Pages.Count > 0)
                {
                    files.Add(new FileInfo(dir + "\\.pdf"));
                    pdfDoc.Save(dir + "\\.pdf");
                }

                ZipFile zFile = new ZipFile();
                foreach (FileInfo file in files)
                    zFile.AddFile(file.FullName, "");
                zFile.Save(path);
            }
        }

        public void LoadDocument(string path, MessageLog log)
        {
            log?.Add(MessageType.MSG, "Loading File '{0}'", path);
            ZipFile zip = new ZipFile(path);
            DirectoryInfo dir = new DirectoryInfo("tmp\\" + Process.GetCurrentProcess().Id);
            if (dir.Exists) dir.RemoveAll();
            zip.ExtractAll(dir.FullName);
            FileInfo docFile = new FileInfo(dir + "\\document.kritzel");
            XmlReader reader = XmlReader.Create(docFile.FullName);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();
            XmlNode root = doc.LastChild;
            XmlNode pagesNode = root.GetNode("Pages");
            Bitmap[] bgrs = new Bitmap[0];
            PdfDocument pdfDoc = null;
            bool loadPdf = false;
            if(File.Exists(dir + "\\.pdf"))
            {
                bgrs = MupdfSharp.PageRenderer.Render(dir + "\\.pdf", Dialogues.PDFImporter.PAGETHEIGHTPIXEL);
                pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open(dir + "\\.pdf", PdfDocumentOpenMode.Modify | PdfDocumentOpenMode.Import);
                loadPdf = true;
            }
            int ind = 1;
            foreach(XmlNode xmlPage in pagesNode.GetNodes("Page"))
            {
                string src = xmlPage.Attributes.GetNamedItem("src").InnerText;
                string text = File.ReadAllText(dir.FullName + "\\" + src);
                KPage page = new KPage();
                log?.Add(MessageType.MSG, "Loading Page {0}", ind++);
                page.LoadFromString(text, log);
                int pdfInd;
                int.TryParse(xmlPage.Attributes["pdf"].Value, out pdfInd);
                if (pdfInd >= 0 && loadPdf)
                {
                    page.BackgroundImage = new Renderer.Image(bgrs[pdfInd]);
                    page.OriginalPage = pdfDoc.Pages[pdfInd];
                }
                Pages.Add(page);
            }
            if (loadPdf)
            {
                pdfDoc.Close();
                pdfDoc.Dispose();
            }
        }

        public void RemovePage(KPage page)
        {
            Pages.Remove(page);
            page.Dispose();
        }
    }
}
