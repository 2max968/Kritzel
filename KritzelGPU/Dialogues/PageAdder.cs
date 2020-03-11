using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel.Dialogues
{
    public partial class PageAdder : Form
    {
        InkControl control;
        KDocument document;
        Dictionary<string, PageFormat> formats;

        public PageAdder(InkControl control, KDocument document)
        {
            InitializeComponent();

            this.control = control;
            this.document = document;

            formats = PageFormat.GetFormats();
            foreach (var f in formats)
            {
                comboBox1.Items.Add(f.Key);
            }

            comboBox1.Text = comboBox1.Items[0].ToString();

            this.Shown += PageAdder_Shown;
        }

        private void PageAdder_Shown(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int pindex = document.Pages.IndexOf(control.Page);
            int index = pindex + 1;
            if (radioBefore.Checked)
                index = pindex;
            else if (radioEnd.Checked)
                index = document.Pages.Count;

            KPage page = new KPage();
            page.Format = formats[comboBox1.Text];
            document.Pages.Insert(index, page);
            control.LoadPage(page);
            this.Close();
        }
    }
}
