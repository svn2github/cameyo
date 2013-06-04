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
    public partial class DataStorageForm : Form
    {
        public DataStorageForm()
        {
            InitializeComponent();
        }

        public bool Do(VirtPackage virtPackage, ref bool dirty)
        {
            String oldValue = virtPackage.GetProperty("BaseDirName");
            String newValue;

            // BaseDirName
            propertyLocalStorageCustomDir.Text = "";
            if (oldValue == "")
                propertyLocalStorageDefault.Checked = true;
            else if (oldValue.Equals("%ExeDir%\\%AppID%.cameyo.files", StringComparison.InvariantCultureIgnoreCase))
                propertyLocalStorageExeDir.Checked = true;
            else
            {
                propertyLocalStorageCustom.Checked = true;
                propertyLocalStorageCustomDir.Text = oldValue;
            }

            // DataDirName
            propertyDataDirName.Text = virtPackage.GetProperty("DataDirName").Trim();
            propertyDataDir.Checked = !string.IsNullOrEmpty(propertyDataDirName.Text);
            propertyDataDir_CheckedChanged(null, null);

            if (ShowDialog() == DialogResult.OK)
            {
                // BaseDirName
                if (propertyLocalStorageDefault.Checked)
                    newValue = "";
                else if (propertyLocalStorageExeDir.Checked)
                    newValue = "%ExeDir%\\%AppID%.cameyo.files";
                else
                    newValue = propertyLocalStorageCustomDir.Text;
                if (newValue != oldValue)
                {
                    virtPackage.SetProperty("BaseDirName", newValue);
                    dirty = true;
                }

                // DataDirName
                virtPackage.SetProperty("DataDirName", propertyDataDirName.Text.Trim());

                return true;
            }
            else
                return false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!propertyLocalStorageDefault.Checked && !propertyLocalStorageExeDir.Checked && propertyLocalStorageCustomDir.Text.Length < 5)
            {
                if (MessageBox.Show("It is not recommended to provide a root path such as \"d:\\\". Would you still like to proceed?", "Storage dir",
                    MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;
            }
            if (propertyDataDir.Checked && propertyDataDirName.Text.Length < 5)
            {
                if (MessageBox.Show("It is not recommended to provide a root path such as \"d:\\\". Would you still like to proceed?", "Data dir",
                    MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void propertyDataDir_CheckedChanged(object sender, EventArgs e)
        {
            propertyDataDirName.Enabled = propertyDataDir.Checked;
        }
    }
}
