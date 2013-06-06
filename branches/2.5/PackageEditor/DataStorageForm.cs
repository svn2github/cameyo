﻿using System;
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
        private const string DefaultBaseDir = @"%AppData%\VOS\%AppID%";
        private const string DefaultDataDir = DefaultBaseDir + @"\CHANGES";

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
            {
                propertyLocalStorageDefault.Checked = true;
                propertyLocalStorageCustomDir.Text = DefaultBaseDir;   // Show user how to build this path
            }
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
            if (propertyDataDirName.Text == "") 
                propertyDataDirName.Text = DefaultDataDir;   // Show user how to build this path
            propertyDataDir_CheckedChanged(null, null);
            propertyLocalStorageCustom_CheckedChanged(null, null);

retry:
            if (ShowDialog() == DialogResult.OK)
            {
                propertyLocalStorageCustomDir.Text = propertyLocalStorageCustomDir.Text.Trim();
                propertyDataDirName.Text = propertyDataDirName.Text.Trim();

                // Validate
                if (propertyLocalStorageCustom.Checked && propertyLocalStorageCustomDir.Text.Trim('\\').IndexOf('\\') == -1 &&
                    MessageBox.Show(PackageEditor.Messages.Messages.storageDirSubdirWarning + "\n" + propertyLocalStorageCustomDir.Text, 
                    "", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    goto retry;
                if (propertyDataDir.Checked && propertyDataDirName.Text.Trim('\\').IndexOf('\\') == -1 &&
                    MessageBox.Show(PackageEditor.Messages.Messages.storageDirSubdirWarning + "\n" + propertyDataDirName.Text,
                    "", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    goto retry;

                // BaseDirName
                if (propertyLocalStorageCustomDir.Text.Equals(DefaultBaseDir, StringComparison.InvariantCultureIgnoreCase))
                    propertyLocalStorageCustomDir.Text = "";
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
                if (propertyDataDirName.Text.Equals(DefaultDataDir, StringComparison.InvariantCultureIgnoreCase) || !propertyDataDir.Checked)
                    propertyDataDirName.Text = "";
                virtPackage.SetProperty("DataDirName", propertyDataDirName.Text);

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

        private void propertyLocalStorageCustom_CheckedChanged(object sender, EventArgs e)
        {
            propertyLocalStorageCustomDir.Enabled = propertyLocalStorageCustom.Checked;
        }
    }
}
