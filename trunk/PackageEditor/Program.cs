using System;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;

namespace PackageEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            String param = "";
            bool notifyPackageBuilt = false;
            if (args.Length >= 1)
            {
                if (args.Length == 2 && args[0].ToUpper() == "/PACKAGEBUILT")
                {
                    notifyPackageBuilt = true;
                    param = args[1];
                }
                else
                    param = args[0];
            }
            Application.Run(new MainForm(param, notifyPackageBuilt));
        }
    }
}
