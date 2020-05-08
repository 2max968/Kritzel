using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Kritzel.Main.GUIElements
{
    public partial class BackgroundSelectPanel : UserControl
    {
        public delegate void ItemClickEvent(BackgroundSelectPanel sender, Type bgrType);

        public event ItemClickEvent ItemClicked;

        public Type SelectedType { get; private set; } = null;

        const int PSIZE = 96;
        const int MARGIN = 8;
        const int ROW = 4;

        public BackgroundSelectPanel()
        {
            InitializeComponent();

            Assembly asm = Assembly.GetCallingAssembly();
            List<Type> bgrTypes = new List<Type>();
            List<string> names = new List<string>();
            List<Bitmap> icons = new List<Bitmap>();

            bgrTypes.Add(typeof(Backgrounds.BackgroundNull));
            names.Add("None");
            icons.Add(new Bitmap(PSIZE, PSIZE));

            foreach (Type t in asm.GetTypes())
            {
                if(t.IsSubclassOf(typeof(Backgrounds.Background))
                    && t.GetCustomAttribute<Backgrounds.BName>() != null)
                {
                    bgrTypes.Add(t);
                    names.Add(t.GetCustomAttribute<Backgrounds.BName>().Name);
                    PageFormat format = 
                        new PageFormat(Util.PointToMm(PSIZE), Util.PointToMm(PSIZE));
                    Backgrounds.Background bgr = 
                        (Backgrounds.Background)t.GetConstructor(new Type[0]).Invoke(new object[0]);
                    Bitmap bmp = new Bitmap(PSIZE, PSIZE);
                    bgr.Draw(Graphics.FromImage(bmp).GetRenderer(), format, 
                        2, Color.LightGray, Color.Red);
                    icons.Add(bmp);
                }
            }

            int rows = bgrTypes.Count / ROW+1;
            this.Width = ROW * PSIZE + (ROW + 1) * MARGIN;
            this.Height = rows * PSIZE + (rows + 1) * MARGIN;
            for(int y = 0; y < rows; y++)
            {
                for(int x = 0; x < ROW; x++)
                {
                    int i = y * ROW + x;
                    if (i >= bgrTypes.Count) continue;
                    PictureBox panel = new PictureBox();
                    panel.Location = new Point(x * (PSIZE + MARGIN) + MARGIN, y * (PSIZE + MARGIN) + MARGIN);
                    panel.Size = new Size(PSIZE, PSIZE);
                    this.Controls.Add(panel);
                    panel.Tag = bgrTypes[i];
                    panel.Image = icons[i];
                    panel.BackColor = Color.White;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.MouseClick += Panel_MouseClick;
                }
            }
        }

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            if((sender is Control) && ((Control)sender).Tag is Type)
            {
                Control panel = (Control)sender;
                Type t = (Type)panel.Tag;
                SelectedType = t;
                ItemClicked?.Invoke(this, t);
            }
        }
    }
}
