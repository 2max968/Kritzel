using LineLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel.Main
{
    static class Program
    {
        public static Icon WindowIcon;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            WebUI.BrowserEmulation.Emulate(WebUI.BrowserEmulation.IE10_IgnoreDoctype);
            WindowIcon = Icon.FromHandle(Properties.Resources.WindowIcon.GetHicon());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SplineView());
            Application.Run(new MainWindow());
        }
    }
}
