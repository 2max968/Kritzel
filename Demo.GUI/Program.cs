using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kritzel.WebUI;

namespace Demo.GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Setup registry");
            BrowserEmulation.Emulate(BrowserEmulation.IE11_IgnoreDoctype);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine("Creating Dialog");
            WebDialog wd = new WebDialog("Interface/ColorPicker.zip");
            wd["currentColor"] = Color.AliceBlue;
            wd["colors"] = new object[] { Color.AliceBlue, Color.Aqua, Color.AntiqueWhite, Color.Bisque, Color.BlueViolet, Color.DarkSalmon };
            Console.WriteLine("Opening Window");
            Application.Run(wd);
        }
    }
}
