using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;
using VirtPackageAPI;

namespace PackageEditor
{
    public partial class MainForm : Form
    {
        private VirtPackage virtPackage;
        private FileSystemEditor fsEditor;
        private RegistryEditor regEditor;
        private bool regLoaded;
        private Thread regLoadThread;
        private MRU mru;
        public bool dirty;

        public MainForm(string packageExeFile, bool notifyPackageBuilt)
        {
            InitializeComponent();
            tabControl.Visible = false;
            regLoaded = false;
            dirty = false;
            virtPackage = new VirtPackage();
            mru = new MRU("Software\\Cameyo\\Packager\\MRU");
            fsEditor = new FileSystemEditor(virtPackage, fsFolderTree, fsFilesList,
                fsFolderInfoFullName, fsFolderInfoIsolationCombo, fsAddBtn, fsRemoveBtn, fsAddEmptyDirBtn, fsSaveFileAsBtn);
            regEditor = new RegistryEditor(virtPackage, regFolderTree, regFilesList,
                regFolderInfoFullName, regFolderInfoIsolationCombo, regRemoveBtn, regEditBtn);

            EnableDisablePackageControls(false);       // No package opened yet; disable Save menu etc
            if (packageExeFile != "")
            {
                if (PackageOpen(packageExeFile) && notifyPackageBuilt)
                {
                    PackageBuiltNotify packageBuiltNotify = new PackageBuiltNotify();
                    packageBuiltNotify.Do("Package successfully created in:",
                        packageExeFile, "PackageBuiltNotify");
                }
            }
            if (!virtPackage.opened)
                openToolStripMenuItem_Click(this, null);
        }

        private void ThreadedRegLoad()
        {
            regEditor.OnPackageOpenBeforeUI();
            regLoaded = true;
        }

        private void regProgressTimer_Tick(object sender, EventArgs e)
        {
            if (regLoaded)
            {
                regEditor.OnPackageOpenUI();
                regProgressBar.Visible = false;
                regToolStrip.Visible = true;
                regSplitContainer.Visible = true;
                regProgressTimer.Enabled = false;
                return;
            }
            regProgressBar.Value += 5;
            if (regProgressBar.Value >= 100)
                regProgressBar.Value = 0;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabGeneral)
                this.OnTabActivate();
            else if (tabControl.SelectedTab == tabFileSystem)
                fsEditor.OnTabActivate();
            else if (tabControl.SelectedTab == tabRegistry)
                regEditor.OnTabActivate();
        }

        private void EnableDisablePackageControls(bool enable)
        {
            tabControl.Visible = enable;
            saveToolStripMenuItem.Enabled = enable;
            saveasToolStripMenuItem.Enabled = enable;
            closeToolStripMenuItem.Enabled = enable;
        }

        private bool PackageOpen(String packageExeFile)
        {
            if (virtPackage.opened && !PackageClose())      // User doesn't want to discard changes
                return false;
            if (virtPackage.Open(packageExeFile))
            {
                regLoaded = false;
                dirty = false;
                this.OnPackageOpen();
                fsEditor.OnPackageOpen();

                // regEditor (threaded)
                regProgressBar.Visible = true;
                regToolStrip.Visible = false;
                regSplitContainer.Visible = false;
                regProgressTimer.Enabled = true;
                if (regLoadThread != null)
                    regLoadThread.Abort();
                regLoadThread = new Thread(ThreadedRegLoad);
                regLoadThread.Start();

                tabControl.SelectedIndex = 0;
                EnableDisablePackageControls(true);

                mru.AddFile(packageExeFile);

                return true;
            }
            else
                return false;
        }

        private bool PackageClose()
        {
            if (virtPackage.opened == false)
                return true;
            if (this.dirty || fsEditor.dirty || regEditor.dirty)
            {
                if (MessageBox.Show("Your changes will be lost. Discard changes?", "Confirm", 
                    MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                {
                    return false;
                }            
            }

            // If regLoadThread is working, wait for it to finish
            if (regLoadThread != null)
                regLoadThread.Abort();

            this.OnPackageClose();
            fsEditor.OnPackageClose();
            regEditor.OnPackageClose();
            virtPackage.Close();
            EnableDisablePackageControls(false);
            return true;
        }

        private bool PackageSave(String fileName)
        {
            this.OnPackageSave();
            fsEditor.OnPackageSave();
            regEditor.OnPackageSave();
            if (virtPackage.Save(fileName))
            {
                this.dirty = false;
                fsEditor.dirty = false;
                regEditor.dirty = false;
                MessageBox.Show("Package saved.");
                return true;
            }
            else
            {
                MessageBox.Show("Cannot save file.");
                return false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory =
                System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Cameyo Packages";
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Virtual packages (*.virtual.exe)|*.virtual.exe|All files (*.*)|*.*";
            //openFileDialog.DefaultExt = "virtual.exe";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PackageOpen(openFileDialog.FileName);
            }
        }

        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "virtual.exe";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                PackageSave(saveFileDialog.FileName);
            }
        }

        private void DisplayAutoLaunch()
        {
            if (virtPackage.GetProperty("AutoLaunch").Contains(';'))
            {
                String[] autoLaunches = virtPackage.GetProperty("AutoLaunch").Split(';');
                propertyAutoLaunch.Text = "";
                propertyAutoLaunch.AutoEllipsis = true;
                for (int i = 0; i < autoLaunches.Count(); i++)
                {
                    String[] items = autoLaunches[i].Split('>');
                    if (items.Count() < 3) continue;     // No Name
                    if (propertyAutoLaunch.Text != "")
                        propertyAutoLaunch.Text += ", ";
                    propertyAutoLaunch.Text += virtPackage.FriendlyShortcutName(items[2]);
                }
                propertyAutoLaunch.Text = "Display menu: " + propertyAutoLaunch.Text;
            }
            else
            {
                String[] items = virtPackage.GetProperty("AutoLaunch").Split('>');
                if (items.Count() >= 3)
                    propertyAutoLaunch.Text = items[0] + " (" + virtPackage.FriendlyShortcutName(items[2]) + ")";
                else
                    propertyAutoLaunch.Text = items[0];
            }
        }

        private void OnPackageOpen()
        {
            // AppID
            propertyAppID.Text = virtPackage.GetProperty("AppID");
            //propertyAppID.TextChanged += PropertyChange;

            // FriendlyName
            propertyFriendlyName.Text = virtPackage.GetProperty("FriendlyName");
            //propertyFriendlyName.TextChanged += PropertyChange;

            // AutoLaunch
            //FillAutoLaunchCombo(propertyAutoLaunch);
            DisplayAutoLaunch();

            // BaseDirName
            DisplayBaseDirName();

            // Isolation
            DisplayIsolation();

            // Icon
            Icon ico = Icon.ExtractAssociatedIcon(virtPackage.openedFile);
            propertyIcon.Image = ico.ToBitmap();

            dirty = false;
        }

        public void OnTabActivate()
        {
            //no needed anymore: DisplayIsolation();
        }

        public bool OnPackageSave()
        {
            bool Ret = true;

            // AppID + AutoLaunch
            Ret &= virtPackage.SetProperty("AppID", propertyAppID.Text);
            Ret &= virtPackage.SetProperty("FriendlyName", propertyFriendlyName.Text);

            // AutoLaunch (and SaveAutoLaunchCmd + SaveAutoLaunchMenu) already set by AutoLaunchForm

            // BaseDirName already set by SetProperty

            // Isolation. Note: it is allowed to have no checkbox selected at all.
            uint sandboxMode = 0;
            if (propertyIsolationIsolated.Checked)
                sandboxMode = VirtPackage.SANDBOXFLAGS_WRITE_COPY;
            if (propertyIsolationMerge.Checked)
                sandboxMode = VirtPackage.SANDBOXFLAGS_MERGE;
            if (sandboxMode != 0)
            {
                virtPackage.SetFileSandbox("", sandboxMode);
                virtPackage.SetRegistrySandbox("", sandboxMode);
            }

            // SetIcon already set when icon button is pressed

            return (Ret);
        }

        private void OnPackageClose()
        {
            propertyAppID.Text = "";
            propertyFriendlyName.Text = "";
            propertyAutoLaunch.Text = "";
            propertyIcon.Image = null;
        }

        private bool DeleteFile(String FileName)
        {
            bool ret = true;
            try
            {
                System.IO.File.Delete(FileName);
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        private bool MoveFile(String srcFileName, String destFileName)
        {
            bool ret = true;
            try
            {
                System.IO.File.Move(srcFileName, destFileName);
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String tmpFileName = virtPackage.openedFile + ".new";
            DeleteFile(tmpFileName);
            //DeleteFile(virtPackage.openedFile + ".bak");
            if (PackageSave(tmpFileName))
            {
                // Release (close) original file, and delete it (otherwise it won't be erasable)
                String packageExeFile = virtPackage.openedFile;
                if (regLoadThread != null)
                    regLoadThread.Abort();
                virtPackage.Close();

                DeleteFile(packageExeFile);
                if (!MoveFile(tmpFileName, packageExeFile))
                    MessageBox.Show("Cannot rename: " + tmpFileName + " to: " + packageExeFile);
                virtPackage.Open(packageExeFile);
            }
            else
            {
                // Save failed. Delete .new file.
                System.IO.File.Delete(virtPackage.openedFile + ".new");
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PackageClose();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PackageClose())
                this.Close();
        }

        private void DisplayBaseDirName()
        {
            String baseDirName = virtPackage.GetProperty("BaseDirName");
            if (baseDirName == "")
                propertyDataStorage.Text = "Use hard disk or USB drive (wherever application is launched from)";
            else
                propertyDataStorage.Text = "Custom: " + baseDirName;
        }

        private void PropertyChange(object sender, EventArgs e)
        {
            dirty = true;
        }

        private void IsolationChanged(object sender, EventArgs e)
        {
            dirty = true;
        }

        private void lnkChangeDataStorage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DataStorageForm dataStorageForm = new DataStorageForm();
            if (dataStorageForm.Do(virtPackage, ref dirty))
                DisplayBaseDirName();
        }

        private void lnkChangeIcon_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Icon ico = Icon.ExtractAssociatedIcon(openFileDialog.FileName);
                if (virtPackage.SetIcon(openFileDialog.FileName))
                {
                    propertyIcon.Image = ico.ToBitmap();
                    //propertyNewIconFileName.Text = openFileDialog.FileName;
                    dirty = true;
                }
                else
                    MessageBox.Show("Error: file not found");
            }
        }

        private void DisplayIsolation()
        {
            // Isolation. Note: it is allowed to have no checkbox selected at all.
            propertyIsolationIsolated.Checked = false;
            propertyIsolationMerge.Checked = false;
            if (virtPackage.GetFileSandbox("") == VirtPackage.SANDBOXFLAGS_WRITE_COPY &&
                virtPackage.GetRegistrySandbox("") == VirtPackage.SANDBOXFLAGS_WRITE_COPY)
                propertyIsolationIsolated.Checked = true;
            if (virtPackage.GetFileSandbox("") == VirtPackage.SANDBOXFLAGS_MERGE &&
                virtPackage.GetRegistrySandbox("") == VirtPackage.SANDBOXFLAGS_MERGE)
                propertyIsolationMerge.Checked = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (regLoadThread != null)
                regLoadThread.Abort();
        }

        private void lnkAutoLaunch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AutoLaunchForm autoLaunchForm = new AutoLaunchForm(virtPackage);
            String oldValue = virtPackage.GetProperty("AutoLaunch");
            if (autoLaunchForm.ShowDialog() == DialogResult.OK)
            {
                DisplayAutoLaunch();
                if (virtPackage.GetProperty("AutoLaunch") != oldValue)
                    dirty = true;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }

    public class MRU
    {
        public int maxItems;
        private RegistryKey regKey;

        public MRU(String regKeyName)
        {
            maxItems = 8;
            try
            {
                regKey = Registry.CurrentUser.OpenSubKey(regKeyName, true);
                if (regKey == null)
                    regKey = Registry.CurrentUser.CreateSubKey(regKeyName);
                if (regKey == null)
                {
                    MessageBox.Show("Cannot write to registry. Quitting.", "Fatal");
                    Application.Exit();
                }
            }
            catch { }
        }

        ~MRU()
        {
            regKey.Close();
        }

        private void InsertTopItem(String fileName)
        {
            // Delete maximum entry, if it exists
            try { regKey.DeleteValue(Convert.ToString(maxItems - 1)); }
            catch { }

            // Look for empty holes (or last element) to move all items up to
            int moveTo = -1;
            for (int i = 0; i < maxItems; i++)
            {
                String fileNameValue = (String)regKey.GetValue(Convert.ToString(i));
                if (fileNameValue == null)
                {
                    moveTo = i;
                    break;
                }
            }

            // Rename (increment) all items up to moveTo
            if (moveTo > 0)
            {
                for (int i = moveTo - 1; i >= 0; i--)
                {
                    // Rename #1:filename -> #2:filename
                    String fileNameValue = (String)regKey.GetValue(Convert.ToString(i));
                    if (i < maxItems)
                        regKey.SetValue(Convert.ToString(i + 1), fileNameValue);
                }
            }

            // Add "0" item
            try { regKey.SetValue("0", fileName); }
            catch { }
        }

        public void AddFile(String fileName)
        {
            // First browse through MRU items, deleting those that already contain this fileName
            String[] items = regKey.GetValueNames();
            for (int i = 0; i < items.Count(); i++)
            {
                String fileNameValue = (String)regKey.GetValue(items[i]);
                if (fileNameValue == null)
                    continue;
                if (fileNameValue.ToUpper() == fileName.ToUpper())
                    regKey.DeleteValue(items[i]);
            }

            // Add to top ("0")
            InsertTopItem(fileName);
        }
    }
}
