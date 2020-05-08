using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kritzel.HTTPServer
{
    public class BlockedDirs
    {
        public static List<DirectoryInfo> Directories { get; private set; } = new List<DirectoryInfo>();

        public static bool IsBlocked(FileInfo file)
        {
            string name = file.FullName.ToLower();
            foreach(DirectoryInfo di in Directories)
            {
                if (name.StartsWith(di.FullName.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
