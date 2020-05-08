using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kritzel.WebUI
{
    public class BrowserEmulation
    {
        public const uint IE7 = 7000;
        public const uint IE8 = 8000;
        public const uint IE8_IgnoreDoctype = 8888;
        public const uint IE9 = 9000;
        public const uint IE9_IgnoreDoctype = 9999;
        public const uint IE10 = 10000;
        public const uint IE10_IgnoreDoctype = 10001;
        public const uint IE11 = 11000;
        public const uint IE11_IgnoreDoctype = 11001;

        public static bool Emulate(uint version)
        {
            string name = new FileInfo(Application.ExecutablePath).Name;
            RegistryKeyPermissionCheck readwrite = RegistryKeyPermissionCheck.ReadWriteSubTree;
            //RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software", readwrite).CreateSubKey("Microsoft", readwrite).CreateSubKey("Internet Explorer", readwrite)
                .CreateSubKey("Main", readwrite).CreateSubKey("FeatureControl", readwrite).CreateSubKey("FEATURE_BROWSER_EMULATION", readwrite);
            if (!key.GetValueNames().Contains(name))
            {
                key.SetValue(name, version, RegistryValueKind.DWord);
                return false;
            }
            else
            {
                uint currVal = Convert.ToUInt32(key.GetValue(name));
                if (currVal == version)
                    return true;
                key.SetValue(name, version, RegistryValueKind.DWord);
                return false;
            }
        }
    }
}
