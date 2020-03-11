using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel
{
    public class PageFormat
    {
        public const float InchInMM = 25.4f;

        public float Width { get; private set; }
        public float Height { get; private set; }

        public PageFormat(float w, float h)
        {
            this.Width = w;
            this.Height = h;
        }

        public PageFormat(float w, float h, bool inch)
        {
            if(inch)
            {
                w *= InchInMM;
                h *= InchInMM;
            }
            this.Width = w;
            this.Height = h;
        }

        /*public SizeF GetPixelSize(Graphics g)
        {
            float inch = 0.0393701f;
            float w = this.Width * g.DpiX * inch;
            float h = this.Height * g.DpiY * inch;
            return new SizeF(w, h);
        }*/

        public SizeF GetPixelSize()
        {
            return new SizeF(Util.MmToPoint(this.Width),
                Util.MmToPoint(this.Height));
        }

        public PageFormat GetLandscape()
        {
            return new PageFormat(this.Height, this.Width);
        }

        public PageFormat Split(int times)
        {
            if (times <= 0)
                return this;
            if(times == 1)
            {
                if(Width > Height)
                {
                    return new PageFormat(Height, (int)(Width / 2));
                }
                else
                {
                    return new PageFormat((int)(Height / 2), Width);
                }
            }
            return this.Split(times - 1).Split(1);
        }

        public PageFormat Expand(int times)
        {
            if (times <= 0)
                return this;
            if (times == 1)
            {
                if (Width < Height)
                {
                    return new PageFormat(Height, Width * 2);
                }
                else
                {
                    return new PageFormat(Height * 2, Width);
                }
            }
            return this.Expand(times - 1).Expand(1);
        }

        public static PageFormat A4 = new PageFormat(210, 297);
        public static PageFormat Letter = new PageFormat(8.5f, 11, true);
        public static PageFormat Legal = new PageFormat(8.5f, 14, true);

        public static Dictionary<string, PageFormat> GetFormats()
        {
            Dictionary<string, PageFormat> f = new Dictionary<string, PageFormat>();
            f.Add("A4", A4);
            f.Add("A5", A4.Split(1));
            f.Add("A3", A4.Expand(1));
            f.Add("Letter", Letter);
            f.Add("Legal", Legal);

            int cl = f.Count;
            var list = f.ToList();
            for(int i = 0; i < cl; i++)
            {
                f.Add(list[i].Key + " Landscape", list[i].Value.GetLandscape());
            }

            return f;
        }
    }
}
