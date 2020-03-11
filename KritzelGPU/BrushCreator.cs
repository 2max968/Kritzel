using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel
{
    public partial class BrushCreator : Form
    {
        public PBrush Brush { get; set; } = PBrush.CreateSolid(Color.Black);

        public BrushCreator()
        {
            InitializeComponent();

            Type t = typeof(Color);
            foreach(PropertyInfo prop in t.GetProperties(BindingFlags.Static | BindingFlags.Public))
            { 
                object oc = prop.GetValue(null);
                if(oc is Color)
                {
                    Color c = (Color)oc;
                    string name = prop.Name;
                    Bitmap bmp = new Bitmap(16,16);
                    Graphics.FromImage(bmp).Clear(c);
                    lvNamedColor.Items.Add(name, namedColorImages.Images.Count).Tag = c;
                    namedColorImages.Images.Add(bmp);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch(tabControl1.TabIndex)
            {
                case 0:
                    if(lvNamedColor.FocusedItem.Tag is Color)
                        Brush = PBrush.CreateSolid((Color)lvNamedColor.FocusedItem.Tag);
                    break;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
