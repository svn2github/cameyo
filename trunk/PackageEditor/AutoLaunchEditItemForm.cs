using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtPackageAPI;

namespace PackageEditor
{
    public partial class AutoLaunchEditItemForm : Form
    {
        private VirtPackage virtPackage;

        public AutoLaunchEditItemForm(VirtPackage virtPackage)
        {
            InitializeComponent();
            this.virtPackage = virtPackage;
        }

        public void SetValues(String name, String target, String args, String description)
        {
            nameBox.Text = name;
            targetBox.Text = target;
            argsBox.Text = args;
            descriptionBox.Text = description;
        }

        public void GetValues(ref String name, ref String target, ref String args, ref String description)
        {
            name = nameBox.Text;
            target = targetBox.Text;
            args = argsBox.Text;
            description = descriptionBox.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (targetBox.Text == "")
            {
                MessageBox.Show("Please specify a target");
                return;
            }
            if (nameBox.Text == "")
            {
                MessageBox.Show("Please specify a name");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnVirtFilesBrowse_Click(object sender, EventArgs e)
        {
            VirtFilesBrowse virtFilesBrowse = new VirtFilesBrowse(virtPackage);
            String path = "";
            if (virtFilesBrowse.Do(ref path, false))
                targetBox.Text = path;
        }
    }
}
