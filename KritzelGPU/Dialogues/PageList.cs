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
    public partial class PageList : Form
    {
        public PageList(KDocument doc)
        {
            InitializeComponent();

            TreeNode nodePages = new TreeNode("Pages");
            TreeNode nodeTrash = new TreeNode("Trash Bin");
        }
    }
}
