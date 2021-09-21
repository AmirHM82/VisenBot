using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimedBot.DatabasesInterface.Classes
{
    public static class ApplicationInfo
    {
        public static string Path { get => $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\DatabasesInterface"; }
    }
}
