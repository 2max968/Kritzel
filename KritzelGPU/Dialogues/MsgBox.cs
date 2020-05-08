using Kritzel.WebUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.Main.Dialogues
{
    public class MsgBox
    {
        public static void ShowOk(string text)
        {
            Show(text, "Ok", "");
        }

        public static bool ShowYesNo(string text)
        {
            return Show(text, "Yes", "No");
        }

        public static bool Show(string text, string btnOkText, string btnCancelText)
        {
            WebDialog diag = new WebDialog("Interface/MsgBox.zip");
            diag["msg"] = text;
            diag["btnOkText"] = btnOkText;
            diag["btnCancelText"] = btnCancelText;
            return diag.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }
    }
}
