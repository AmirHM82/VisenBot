using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimedBot.DatabasesInterface.Classes
{
    public static class FileManager
    {
        public static void CheckOrCreateRoot()
        {
            if (!CheckRoot()) CreateRoot();
        }

        public static bool CheckRoot()
        {
            return Directory.Exists(ApplicationInfo.Path);
        }

        public static void CreateRoot()
        {
            Directory.CreateDirectory(ApplicationInfo.Path);
        }
    }
}
