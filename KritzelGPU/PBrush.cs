using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Main
{
    public class PBrush
    {
        public const string B_SOLID = "solid";
        public const string B_LINEAR = "linear";

        string type;
        Color[] colors;
        float[] nums;
        Brush b = null;

        public Brush Brush
        {
            get
            {
                if (b == null)
                    b = new SolidBrush(Color.Fuchsia);
                return b;
            }
        }

        public static PBrush CreateSolid(Color c)
        {
            return new PBrush(B_SOLID, new Color[] { c }, new float[0]);
        }

        public static PBrush CreateLinear(PointF p1, PointF p2, Color c1, Color c2)
        {
            return new PBrush(B_LINEAR, new Color[] { c1, c2 },
                        new float[] { p1.X, p1.Y, p2.X, p2.Y });
        }

        public PBrush(string type, Color[] colors, float[] nums)
        {
            this.type = type;
            this.colors = colors;
            this.nums = nums;

            switch(type)
            {
                case B_SOLID:
                    b = new SolidBrush(colors[0]);
                    break;
                case B_LINEAR:
                    PointF p1 = new PointF(nums[0], nums[1]);
                    PointF p2 = new PointF(nums[2], nums[3]);
                    LinearGradientBrush lb = new LinearGradientBrush(p1, p2, colors[0], colors[1]);
                    b = lb;
                    lb.WrapMode = WrapMode.TileFlipXY;
                    break;
            }
        }

        public float[] GetNums()
        {
            return nums;
        }

        public Color[] GetColors()
        {
            return colors;
        }

        public string SType()
        {
            return type;
        }

        public string SColors()
        {
            string txt = "";
            foreach(Color c in colors)
            {
                txt += string.Format("{0},{1},{2},{3};", 
                    Util.FToS(c.R), Util.FToS(c.G), Util.FToS(c.B), Util.FToS(c.A));
            }
            return txt;
        }

        public string SFloats()
        {
            string txt = "";
            foreach(float f in nums)
            {
                txt += Util.FToS(f) + ";";
            }
            return txt;
        }

        public static PBrush FromStrings(string type, string colors, string floats)
        {
            List<Color> cs = new List<Color>();
            List<float> fs = new List<float>();

            foreach(string cN in colors.Split(';'))
            {
                if (cN == "") continue;
                string[] dats = cN.Split(',');
                int r = int.Parse(dats[0]);
                int g = int.Parse(dats[1]);
                int b = int.Parse(dats[2]);
                int a = int.Parse(dats[3]);
                Color c = Color.FromArgb(a, r, g, b);
                cs.Add(c);
            }
            foreach(string fN in floats.Split(';'))
            {
                if (fN == "") continue;
                float val = Util.SToF(fN);
                fs.Add(val);
            }

            return new PBrush(type, cs.ToArray(), fs.ToArray());
        }

        public Color GetMainColor()
        {
            if (colors.Length == 0)
                return Color.Fuchsia;
            return colors[0];
        }
    }
}
