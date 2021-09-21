using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrimedBot.DatabasesInterface.Classes;
using TrimedBot.DatabasesInterface.Views.Forms;

namespace TrimedBot.DatabasesInterface
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FileManager.CheckOrCreateRoot();
            await Connections.LoadFromFile();

            Application.Run(new frmMain());

            await Connections.SaveToFile();
        }
    }
}
