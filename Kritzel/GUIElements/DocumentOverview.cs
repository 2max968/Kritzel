using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel.GUIElements
{
    public partial class DocumentOverview : UserControl
    {
        KDocument doc;

        public DocumentOverview()
        {
            InitializeComponent();
        }

        public void SetDocument(KDocument doc)
        {
            list.Items.Clear();
            for(int i = 0; i < doc.Pages.Count; i++)
            {
                ListViewItem itm = new ListViewItem("Page " + (i+1));
                list.Items.Add(itm);
            }
        }
    }
}
