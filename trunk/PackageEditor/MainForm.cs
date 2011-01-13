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
        private bool dragging;

        // creation of delegate for PackageOpen
        private delegate bool DelegatePackageOpen(String path);
        DelegatePackageOpen Del_Open;

        public MainForm(string packageExeFile, bool notifyPackageBuilt)
        {
            InitializeComponent();
            dragging = false;

            // delegate for PackageOpen init
            Del_Open = new DelegatePackageOpen(this.PackageOpen);

            tabControl.Visible = false;
            regLoaded = false;
            dirty = false;
            virtPackage = new VirtPackage();
            mru = new MRU("Software\\Cameyo\\Packager\\MRU");

            fsEditor = new FileSystemEditor(virtPackage, fsFolderTree, fsFilesList,
                fsFolderInfoFullName, fsFolderInfoIsolationCombo, fsAddBtn, fsRemoveBtn, fsAddEmptyDirBtn, fsSaveFileAsBtn, fsAddDirBtn);
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

            #if DropBox
            if (!System.IO.File.Exists(Application.StartupPath + "\\AppLimit.CloudComputing.oAuth.dll")
                || !System.IO.File.Exists(Application.StartupPath + "\\AppLimit.CloudComputing.SharpBox.dll")
                || !System.IO.File.Exists(Application.StartupPath + "\\Newtonsoft.Json.Net20.dll"))
            {
                dropboxLabel.Hide();
                dropboxButton.Hide();
                resetCredLink.Hide();
                MessageBox.Show("This version is compiled with DropBox funtionality, but one or more of the dlls needed are missing:\nAppLimit.CloudComputing.oAuth.dll\nAppLimit.CloudComputing.SharpBox.dll\nNewtonsoft.Json.Net20.dll\n\nThe button will be hidden", "Missing DLL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            #else
            dropboxLabel.Hide();
            dropboxButton.Hide();
            resetCredLink.Hide();
            #endif
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
                if (PackageSave(saveFileDialog.FileName))
                    virtPackage.openedFile = saveFileDialog.FileName;
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

            // StopInheritance
            propertyStopInheritance.Text = virtPackage.GetProperty("StopInheritance");

            // CleanupOnExit
            propertyCleanupOnExit.Checked = virtPackage.GetProperty("OnStopUnvirtualized").
                Contains("%MyExe%>-Quiet -Remove");

            this.Text = "Package Editor" + " - " + virtPackage.openedFile;
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
            Ret &= virtPackage.SetProperty("StopInheritance", propertyStopInheritance.Text);
            if (propertyCleanupOnExit.Checked)
            {
                String str = virtPackage.GetProperty("OnStopUnvirtualized");
                if (!str.Contains("%MyExe%>-Quiet -Remove"))
                {
                    if (str != "")
                        str += ";";
                    str += "%MyExe%>-Quiet -Remove";
                    Ret &= virtPackage.SetProperty("OnStopUnvirtualized", str);
                }
            }


            // AutoLaunch (and SaveAutoLaunchCmd + SaveAutoLaunchMenu) already set by AutoLaunchForm

            // BaseDirName already set by SetProperty

            // Isolation. Note: it is allowed to have no checkbox selected at all.
            uint sandboxMode = 0;
            if (propertyIsolationIsolated.Checked || propertyIsolationDataMode.Checked)
                sandboxMode = VirtPackage.SANDBOXFLAGS_WRITE_COPY;
            else if (propertyIsolationMerge.Checked)
                sandboxMode = VirtPackage.SANDBOXFLAGS_MERGE;
            if (sandboxMode != 0)
            {
                virtPackage.SetFileSandbox("", sandboxMode);
                virtPackage.SetRegistrySandbox("", sandboxMode);
                if (propertyIsolationDataMode.Checked)
                    sandboxMode = VirtPackage.SANDBOXFLAGS_MERGE;
                virtPackage.SetFileSandbox("%Personal%", sandboxMode);
                virtPackage.SetFileSandbox("%Desktop%", sandboxMode);
                virtPackage.SetFileSandbox("%Network%", sandboxMode);
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
            propertyStopInheritance.Text = "";
            propertyCleanupOnExit.Checked = false;
            this.Text = "Package Editor";
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
            else if (baseDirName == "%ExeDir%")
                propertyDataStorage.Text = "Under the executable's directory";
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
            propertyIsolationDataMode.Checked = false;
            propertyIsolationIsolated.Checked = false;
            propertyIsolationMerge.Checked = false;
            if (virtPackage.GetFileSandbox("") == VirtPackage.SANDBOXFLAGS_WRITE_COPY &&
                virtPackage.GetRegistrySandbox("") == VirtPackage.SANDBOXFLAGS_WRITE_COPY &&
                virtPackage.GetFileSandbox("%Personal%") == VirtPackage.SANDBOXFLAGS_MERGE &&
                virtPackage.GetFileSandbox("%Desktop%") == VirtPackage.SANDBOXFLAGS_MERGE &&
                virtPackage.GetFileSandbox("%Network%") == VirtPackage.SANDBOXFLAGS_MERGE)
                propertyIsolationDataMode.Checked = true;
            else if (virtPackage.GetFileSandbox("") == VirtPackage.SANDBOXFLAGS_WRITE_COPY &&
                virtPackage.GetRegistrySandbox("") == VirtPackage.SANDBOXFLAGS_WRITE_COPY)
                propertyIsolationIsolated.Checked = true;
            else if (virtPackage.GetFileSandbox("") == VirtPackage.SANDBOXFLAGS_MERGE &&
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

        // dragdrop function (DragEnter) to open a new file dropping it in the main form
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                dragging = true;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // dragdrop function (DragDrop) to open a new file dropping it in the main form
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            dragging = false;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length > 1)
            {
                MessageBox.Show("You can drop only one file");
                return;
            }
            //if (System.IO.Path.GetExtension(System.IO.Path.GetFileNameWithoutExtension(files[0]))
            //         + System.IO.Path.GetExtension(files[0]) != ".virtual.exe")
            if (System.IO.Path.GetExtension(files[0]).ToLower() != ".exe")
            {
                MessageBox.Show("You can only open files with .exe extension");
                return;
            }
            // open in a new thread to avoid blocking of explorer in case of big files
            this.BeginInvoke(Del_Open, new Object[] { files[0] });
            this.Activate();
        }

        // dragdrop function (DragEnter) to add file to the tree list and file list
        private void Vfs_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                ((Control)sender).Focus();
                e.Effect = DragDropEffects.Copy;
                dragging = false;
                itemHoverTimer.Interval = 2000;
                itemHoverTimer.Start();
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // dragdrop function (DragDrop) to add file to the tree list and file list
        private void Vfs_DragDrop(object sender, DragEventArgs e)
        {
            dragging = false;
            if (itemHoverTimer.Enabled)
                itemHoverTimer.Stop();

            FolderTreeNode parentNode = (FolderTreeNode)fsFolderTree.SelectedNode;
            if (parentNode == null)
            {
                MessageBox.Show("Please select a folder to add to");
                return;
            }
            if (parentNode.deleted)
            {
                MessageBox.Show("Folder was deleted");
                return;
            }

            String[] paths = (String[])e.Data.GetData(DataFormats.FileDrop);
            foreach (String path in paths)
            {
                this.BeginInvoke(fsEditor.Del_AddFOrFR, new object[]{ parentNode, path });
            }
        }

        // dragdrop function (DragOver) to add file to the tree list and file list
        private void fsFolderTree_DragOver(object sender, DragEventArgs e)
        {
            Point pt = fsFolderTree.PointToClient(new Point(e.X, e.Y));
            TreeNode nodeUnderCursor = fsFolderTree.GetNodeAt(pt);
            if (nodeUnderCursor != null)
            {
                fsFolderTree.SelectedNode = nodeUnderCursor;
            }
        }

        // dragdrop function to add file to the tree list and file list
        // this function allows the nodes to close while navigating in the tree to drop files and folders
        private void fsFolderTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (dragging && e.Node.Level > 0)
            {
                TreeNode oldNode = fsFolderTree.SelectedNode;
                TreeNode newNode = e.Node;
                if (oldNode.IsExpanded && newNode.Level == oldNode.Level)
                    oldNode.Collapse();
                else
                {
                    while (newNode.Level <= oldNode.Level)
                    {
                        oldNode.Collapse();
                        oldNode = oldNode.Parent;
                    }
                }
            }
        }

        // dragdrop function to add file to the tree list and file list
        // this function allows the nodes to open while navigating in the tree to drop files and folders
        private void fsFolderTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (dragging)
            {
                if (itemHoverTimer.Enabled)
                    itemHoverTimer.Stop();
                itemHoverTimer.Interval = 900;
                itemHoverTimer.Start();
            }
        }

        // timer to open the nodes
        private void OnItemHover(object sender, EventArgs e)
        {
            itemHoverTimer.Stop();
            if (fsFolderTree.SelectedNode != null)
            {
                if (!dragging)
                    dragging = true;
                fsFolderTree.SelectedNode.Expand();
            }
        }

        private void dropboxButton_Click(object sender, EventArgs e)
        {
            #if DropBox
            if (this.dirty || fsEditor.dirty || regEditor.dirty)
            {
                MessageBox.Show("You have to save the package first");
                return;
            }
            String opened = virtPackage.openedFile;
            try
            {
                virtPackage.Close();
                DropboxLogin dropLogin = new DropboxLogin();
                dropLogin.Publish(opened);
            }
            finally
            {
                if (!virtPackage.opened)
                    virtPackage.Open(opened);
            }
            #endif
        }

        private void resetCredLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegistryKey cameyoKey = Registry.CurrentUser.CreateSubKey(@"Software\Cameyo");
            try
            {
                cameyoKey.DeleteValue("DropBoxTokenKey");
                cameyoKey.DeleteValue("DropBoxTokenSecret");
            }
            catch
            {
                MessageBox.Show("Cannot delete login tokens, did you save them?");
            }
        }

        private void lnkCustomEvents_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CustomEventsForm customEventsForm = new CustomEventsForm(virtPackage);
            customEventsForm.ShowDialog();
            dirty |= customEventsForm.dirty;
            customEventsForm.Dispose();
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
