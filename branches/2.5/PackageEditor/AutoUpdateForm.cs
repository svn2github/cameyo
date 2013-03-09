using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PackageEditor
{
    public partial class AutoUpdateForm : Form
    {
        public AutoUpdateForm()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            btnBack.Enabled = true;
            tabWizard.SelectedIndex++;
            if (tabWizard.SelectedIndex == tabWizard.TabPages.Count - 1)
                btnNext.Enabled = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = true;
            tabWizard.SelectedIndex--;
            if (tabWizard.SelectedIndex == 0)
                btnBack.Enabled = false;
        }
    }
}
