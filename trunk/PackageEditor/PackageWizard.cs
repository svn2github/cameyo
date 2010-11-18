using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PackageEditor
{
    public partial class PackageWizard : Form
    {
        public PackageWizard()
        {
            InitializeComponent();
        }

        private void wpIntro_ShowFromNext(object sender, EventArgs e)
        {
            infoPage1.PageText =
                "This wizard will allow you to quickly configure the main aspects of your virtual application:\n" +
                "\n" +
                "- Application ID\n" +
                "- Auto-launch executable\n" +
                "- Isolation mode\n" +
                "- Mobility mode";
        }
    }
}
