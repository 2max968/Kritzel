using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kritzel
{
    public class KPage : IDisposable
    {
        public PageFormat Format;
        public List<Line> Lines;
        public Renderer.Image BackgroundImage = null;
        public PdfPage OriginalPage = null;
        public Backgrounds.Background Background = new Backgrounds.BackgroundQuadPaper5mm();
        public Color BackgroundColor1 = Color.LightSteelBlue;
        public Color BackgroundColor2 = Color.OrangeRed;
        public float Border { get; set; } = 15f;
        public bool IsDisposed { get; private set; } = false;
        public DateTime CreationTime { get; private set; }
        public bool ShowDate { get; set; } = true;

        public KPage()
        {
            Format = PageFormat.A4;
            Lines = new List<Line>();
            CreationTime = DateTime.Now;
        }

        public string SaveToString()
        {
            StringBuilder output = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = " ",
                NewLineChars = "\n"
            };
            using (XmlWriter xml = XmlWriter.Create(output, settings))
            {
                xml.WriteStartElement("Ink");
                xml.WriteStartElement("Format");
                string bgrName = "null";
                if (Background != null)
                    bgrName = Background.GetType().FullName;
                xml.WriteAttributeString("w", "" + Format.Width);
                xml.WriteAttributeString("h", "" + Format.Height);
                xml.WriteAttributeString("background", bgrName);
                xml.WriteAttributeString("border", Util.FToS(Border));
                xml.WriteEndElement();
                xml.WriteStartElement("CreationTime");
                xml.WriteAttributeString("show", ShowDate ? "true" : "false");
                xml.WriteAttributeString("date", CreationTime.ToFileTime().ToString());
                xml.WriteEndElement();
                foreach (Line l in Lines)
                {
                    xml.WriteStartElement("Line");
                    xml.WriteAttributeString("type", l.GetType().FullName);
                    xml.WriteAttributeString("params", l.ToParamString());
                    xml.WriteStartElement("Brush");
                    xml.WriteAttributeString("type", l.Brush.SType());
                    xml.WriteAttributeString("color", l.Brush.SColors());
                    xml.WriteAttributeString("nums", l.Brush.SFloats());
                    xml.WriteEndElement();
                    xml.WriteEndElement();
                }
                xml.WriteEndElement();
            }
            return output.ToString();
        }

        public void LoadFromString(string txt)
        {
            StringReader input = new StringReader(txt);
            Line line = null;
            using (XmlReader xml = XmlReader.Create(input))
            {
                while (xml.Read())
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        if (xml.Name == "Line")
                        {
                            string typeN = xml.GetAttribute("type");
                            string param = xml.GetAttribute("params");
                            Type t = Assembly.GetCallingAssembly().GetType(typeN);
                            line = t.GetConstructor(new Type[0]).Invoke(new object[0]) as Line;
                            if (line != null)
                            {
                                line.FromParamString(param);
                                line.CalcSpline();
                            }
                            line.Brush = PBrush.CreateSolid(Color.Black);
                            Lines.Add(line);
                        }
                        else if (xml.Name == "Brush")
                        {
                            PBrush brush = PBrush.FromStrings(
                                xml.GetAttribute("type"),
                                xml.GetAttribute("color"),
                                xml.GetAttribute("nums"));
                            if (line != null) line.Brush = brush;
                        }
                        else if(xml.Name == "Format")
                        {
                            float w = 1;
                            float h = 1;
                            float border = 15;
                            float.TryParse(xml.GetAttribute("w"), out w);
                            float.TryParse(xml.GetAttribute("h"), out h);
                            Util.TrySToF(xml.GetAttribute("border"), out border);
                            Border = border;
                            Format = new PageFormat(w, h);
                            string backgroundName = xml.GetAttribute("background");
                            Type backgroundType = Type.GetType(backgroundName);
                            Backgrounds.Background bgr = (Backgrounds.Background)backgroundType
                                .GetConstructor(new Type[0])
                                .Invoke(new object[0]);
                            Background = bgr;
                        }
                        else if(xml.Name == "CreationTime")
                        {
                            bool show = xml.GetAttribute("show") == "true";
                            long time = 0;
                            long.TryParse(xml.GetAttribute("date"), out time);
                            ShowDate = show;
                            CreationTime = DateTime.FromFileTime(time);
                        }
                    }
                    else if (xml.NodeType == XmlNodeType.EndElement)
                    {
                        if (xml.Name == "Line")
                        {
                            line = null;
                        }
                    }
                }
            }
        }

        public void LoadPDF(string path, int page)
        {
            // Load Image
            Directory.CreateDirectory("tmp");
            if (!File.Exists("mutool.exe"))
                File.WriteAllBytes("mutool.exe", Properties.Resources.mutool);
            Util.Run("mutool.exe", "draw -o \"tmp\\thumb%d.png\" -r 300 \"" + path + "\" " + page);
            FileStream stream = File.OpenRead("tmp\\thumb" + page + ".png");
            Bitmap image = new Bitmap(stream);
            stream.Close();
            File.Delete("tmp\\thumb" + page + ".png");

            // Load Page
            PdfDocument pdfdoc = PdfReader.Open(path, PdfDocumentOpenMode.Modify | PdfDocumentOpenMode.Import);
            PdfPage pPage = pdfdoc.Pages[page - 1];
            this.OriginalPage = pPage;

            this.BackgroundImage = new Renderer.Image(image);
            this.Format = PageFormat.A4;
            this.Background = null;
        }

        public void SelectArea(PointF[] points)
        {
            Deselect();
            if (points.Length <= 1)
                return;
            RectangleF fbounds = Util.GetBounds(points);
            Rectangle bounds = new Rectangle((int)fbounds.X, (int)fbounds.Y, (int)fbounds.Width + 1, (int)fbounds.Height + 1);

            SizeF s = Format.GetPixelSize();
            Bitmap sBuffer = new Bitmap(bounds.Width, bounds.Height);
            Graphics g = Graphics.FromImage(sBuffer);
            g.Clear(Color.Black);
            g.TranslateTransform(-bounds.X, -bounds.Y);
            g.FillPolygon(Brushes.White, points);

            int divider = 1;
            for(int x = bounds.X; x < bounds.Right; x+=divider)
            {
                for(int y = bounds.Y; y < bounds.Bottom; y+=divider)
                {
                    bool v = sBuffer.GetPixel(x - bounds.X, y - bounds.Y).R > 127;
                    if (!v) continue;
                    foreach(Line l in Lines)
                    {
                        if(l.Collision(new LPoint(x, y, divider*2)))
                        {
                            l.Selected = true;
                        }
                    }
                }
            }
            sBuffer.Dispose();
        }

        public void Deselect()
        {
            foreach (Line l in Lines)
                l.Selected = false;
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            if (BackgroundImage != null)
                BackgroundImage.Dispose();
        }

        public void Draw(Renderer.BaseRenderer r)
        {
            SizeF pSize = Format.GetPixelSize();
            if (Background != null)
                Background.Draw(r, Format, Border,
                    BackgroundColor1, BackgroundColor2);

            if(ShowDate)
                r.DrawText(CreationTime.ToLongDateString() + " " + CreationTime.ToShortTimeString(), 
                    PBrush.CreateSolid(BackgroundColor1),
                    new RectangleF(Border + 1, Border - 5, 300, 50), 2);

            for (int i = 0; i < Lines.Count; i++)
                Lines[i].Render(r);
        }
    }
}
