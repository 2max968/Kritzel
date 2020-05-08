using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel.Main.Dialogues
{
    public partial class Sizes : Form
    {
        public Sizes()
        {
            InitializeComponent();

            foreach(var format in PageFormat.GetFormats())
            {
                ListViewItem itm = new ListViewItem(format.Key);
                itm.SubItems.Add("" + format.Value.Width + "mm");
                itm.SubItems.Add("" + format.Value.Height + "mm");
                listView1.Items.Add(itm);
            }
        }
    }
}
