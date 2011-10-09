using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;
using VirtPackageAPI;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using PackageEditor.FilesEditing;

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
        private string CleanupOnExitExe = "%MyExe%";
        private string CleanupOnExitOptionQuiet = "-Quiet";
        private string CleanupOnExitOptionConfirm = "-Confirm";
        private string CleanupOnExitOptionRemove = "-Remove";
        private string CleanupOnExitOptionRemoveReg = "-Remove:Reg";

        private Control[] Editors;

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
            panelWelcome.Visible = !tabControl.Visible;
            regLoaded = false;
            dirty = false;
            virtPackage = new VirtPackage();
            mru = new MRU("Software\\Cameyo\\Packager\\MRU");

            fsEditor = new FileSystemEditor(virtPackage, fsFolderTree, fsFilesList,
                fsFolderInfoFullName, fsFolderInfoIsolationCombo, fsAddBtn, fsRemoveBtn, fsAddEmptyDirBtn, fsSaveFileAsBtn, fsAddDirBtn);
            regEditor = new RegistryEditor(virtPackage, regFolderTree, regFilesList,
                regFolderInfoFullName, regFolderInfoIsolationCombo, regRemoveBtn, regEditBtn);

            regFilesList.DoubleClickActivation = true;
            Editors = new Control[] { tbFile, tbValue, tbType, tbSize };

            EnableDisablePackageControls(false);       // No package opened yet; disable Save menu etc
            if (packageExeFile != "" && !packageExeFile.Equals("/OPEN", StringComparison.InvariantCultureIgnoreCase))
            {
                if (PackageOpen(packageExeFile) && notifyPackageBuilt)
                {
                    PackageBuiltNotify packageBuiltNotify = new PackageBuiltNotify();
                    String friendlyPath = packageExeFile;
                    int pos = friendlyPath.ToUpper().IndexOf("\\DOCUMENTS\\");
                    if (pos == -1) pos = friendlyPath.ToUpper().IndexOf("\\MY DOCUMENTS\\");
                    if (pos != -1) friendlyPath = friendlyPath.Substring(pos + 1);
                    packageBuiltNotify.Do("Package successfully created in:",
                        packageExeFile, friendlyPath, "PackageBuiltNotify");
                }
            }

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
            regLoadThread = null;
        }

        private void ThreadedRegLoadStop()
        {
          regEditor.threadedRegLoadStop();
          if (regLoadThread != null)
          {
            regLoadThread.Join();
            regLoadThread = null;
          }
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
            panelWelcome.Visible = !tabControl.Visible;
            saveToolStripMenuItem.Enabled = enable;
            saveasToolStripMenuItem.Enabled = enable;
            closeToolStripMenuItem.Enabled = enable;

        }

        #region PleaseWaitDialog
        private class PleaseWaitMsg
        {
            public String iconFileName;
            public String title;
            public String msg;
            public PleaseWaitMsg(String title, String msg, String iconFileName)
            {
                this.title = title;
                this.msg = msg;
                this.iconFileName = iconFileName;
            }
        }

        //System.Threading.AutoResetEvent pleaseWaitDialogEvent;
        private class PleaseWaitDialog
        {
            private PictureBox icon;
            private Label msg;
            private Form dialog;
            Icon iconFile;
            //PleaseWaitMsg pleaseWaitMsg;

            private void InitializeComponent()
            {
                icon = new PictureBox();
                icon.Location = new Point(12, 12);
                icon.Size = new Size(48, 48);

                msg = new Label();
                msg.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                msg.Location = new Point(70, 12);
                msg.AutoSize = true;

                dialog = new Form();
                dialog.ClientSize = new Size(400, 70);
                dialog.Controls.Add(this.msg);
                dialog.Controls.Add(this.icon);
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MinimizeBox = false;
                dialog.ShowInTaskbar = false;
                dialog.ControlBox = false;
                dialog.StartPosition = FormStartPosition.CenterScreen;
                dialog.TopMost = true;
            }

            public PleaseWaitDialog()
            {
                InitializeComponent();
            }

            public void Display(PleaseWaitMsg pleaseWaitMsg)
            {
                try
                {
                  if (!String.IsNullOrEmpty(pleaseWaitMsg.iconFileName))
                  {
                    iconFile = Win32Function.getIconFromFile(pleaseWaitMsg.iconFileName);
                    icon.Image = iconFile.ToBitmap();
                  }
                }
                catch { }
                dialog.Text = pleaseWaitMsg.title;
                msg.Text = pleaseWaitMsg.msg;
                dialog.ClientSize = new Size(Math.Max(msg.Width + 100, 250), 70);
                dialog.Show(null);
                EventWaitHandle pleaseWaitDialogEvent = AutoResetEvent.OpenExisting("pleaseWaitDialogEvent");
                while (!pleaseWaitDialogEvent.WaitOne(10, false))
                    Application.DoEvents();
            }
        }
        #endregion

        static void PleaseWaitJob(object data)
        {
            PleaseWaitMsg pleaseWaitMsg = (PleaseWaitMsg)data;
            PleaseWaitDialog pleaseWaitDialog = new PleaseWaitDialog();
            pleaseWaitDialog.Display(pleaseWaitMsg);
        }

        EventWaitHandle PleaseWaitBegin(String title, String msg, String iconFileName)
        {
            EventWaitHandle pleaseWaitDialogEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "pleaseWaitDialogEvent");
            Thread thread = new Thread(new ParameterizedThreadStart(PleaseWaitJob));
            PleaseWaitMsg pleaseWaitMsg = new PleaseWaitMsg(title, msg, iconFileName);
            thread.Start(pleaseWaitMsg);
            Thread.Sleep(500);
            return pleaseWaitDialogEvent;
        }

        void PleaseWaitEnd()
        {
            EventWaitHandle pleaseWaitDialogEvent = EventWaitHandle.OpenExisting("pleaseWaitDialogEvent");
            pleaseWaitDialogEvent.Set();
        }

        private bool PackageOpen(String packageExeFile)
        {
            VirtPackage.APIRET apiRet;
            return PackageOpen(packageExeFile, out apiRet);
        }

        private bool PackageOpen(String packageExeFile, out VirtPackage.APIRET apiRet)
        {
            bool ret;
            apiRet = 0;
            if (virtPackage.opened && !PackageClose())      // User doesn't want to discard changes
                return false;
            PleaseWaitBegin("Opening package", "Opening " + System.IO.Path.GetFileName(packageExeFile) + "...", packageExeFile);
            {
                if (virtPackage.Open(packageExeFile, out apiRet))
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

                    ThreadedRegLoadStop();
                    regLoadThread = new Thread(ThreadedRegLoad);
                    regLoadThread.Start();

                    tabControl.SelectedIndex = 0;
                    EnableDisablePackageControls(true);
                    mru.AddFile(packageExeFile);

                    ret = true;
                }
                else
                    ret = false;
            }
            PleaseWaitEnd();

            return ret;
        }

        private bool PackageClose()
        {
            if (virtPackage.opened == false)
                return true;
            if (this.dirty || fsEditor.dirty || regEditor.dirty)
            {
                if (MessageBox.Show("Your changes will be lost. Discard changes?", "Confirm",
                    MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }

            // If regLoadThread is working, wait for it to finish
            ThreadedRegLoadStop();

            this.OnPackageClose();
            fsEditor.OnPackageClose();
            regEditor.OnPackageClose();
            virtPackage.Close();
            EnableDisablePackageControls(false);
            return true;
        }

        private bool PackageCanSave(out String message)
        {
          message = "";
          if (String.IsNullOrEmpty(propertyAppID.Text))
            message += "- AppID is a required field to save a package.\r\n";
          if (String.IsNullOrEmpty(virtPackage.GetProperty("AutoLaunch")))
            message += "- The package does not have any program(s) selected to launch.\r\nPlease select a program to launch on the tab:General > Panel:Basics > Item:Startup.";

          return message == "";
        }

        private bool PackageSave(String fileName)
        {
            String CantSaveBecause = "";
            if (!PackageCanSave(out CantSaveBecause))
            {
              MessageBox.Show(this, CantSaveBecause, "Cannot save package.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              return false;
            }

            int ret = 0;
            VirtPackage.APIRET apiRet = 0;
            PleaseWaitBegin("Saving package", "Saving " + System.IO.Path.GetFileName(fileName) + "...", virtPackage.openedFile);
            {
              ret = ret == 0 && !this.OnPackageSave() ? 1 : ret;
              ret = ret == 0 && !fsEditor.OnPackageSave() ? 2 : ret;
              ret = ret == 0 && !regEditor.OnPackageSave() ? 3 : ret;
              ret = ret == 0 && !virtPackage.SaveEx(fileName, out apiRet) ? 4 : ret;
            }
            PleaseWaitEnd();

            if (ret == 0)
            {
              this.dirty = false;
              fsEditor.dirty = false;
              regEditor.dirty = false;
              MessageBox.Show("Package saved.");
              return true;
            }
            else
            {
              MessageBox.Show(String.Format("Cannot save file. Error:{0} ApiRet:{1}", ret, apiRet));
              return false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Cameyo Packages";
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Virtual packages (*.virtual.exe;*.cameyo.exe)|*.virtual.exe;*.cameyo.exe|All files (*.*)|*.*";
            //openFileDialog.DefaultExt = "virtual.exe";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                VirtPackage.APIRET apiRet;
                if (!PackageOpen(openFileDialog.FileName, out apiRet))
                {
                    MessageBox.Show(String.Format("Failed to open package. API error:{0}", apiRet));
                }
            }
        }

        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String message;
            if(!PackageCanSave(out message))
            {
              MessageBox.Show(this, message, "Cant save the package.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "Virtual packages (*.virtual.exe)|*.virtual.exe";
            saveFileDialog.DefaultExt = "virtual.exe";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (PackageSave(saveFileDialog.FileName))
                {
                    virtPackage.openedFile = saveFileDialog.FileName;
                    this.Text = "Package Editor" + " - " + saveFileDialog.FileName;
                }
            }
        }

        private void exportAsZeroInstallerXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.AddExtension = true;
          saveFileDialog.Filter = "ZeroInstaller configuration file (*.xml)|*.xml";
          saveFileDialog.DefaultExt = "xml";
          if (saveFileDialog.ShowDialog() == DialogResult.OK)
          {
            XmlTextWriter xmlOut = new XmlTextWriter(saveFileDialog.FileName, Encoding.Default);
            xmlOut.Formatting = Formatting.Indented;
            xmlOut.WriteStartDocument();
            xmlOut.WriteStartElement("ZeroInstallerXml");

              xmlOut.WriteStartElement("Properties");
                xmlOut.WriteStartElement("Property");
                xmlOut.WriteAttributeString("AppName", "TestApp");
                xmlOut.WriteEndElement();
                xmlOut.WriteStartElement("Property");
                xmlOut.WriteAttributeString("AppVersion", "1.0");
                xmlOut.WriteEndElement();
                xmlOut.WriteStartElement("Property");
                xmlOut.WriteAttributeString("IconFile", "Icon.exe");
                xmlOut.WriteEndElement();
                xmlOut.WriteStartElement("Property");
                xmlOut.WriteAttributeString("StopInheritance", "");
                xmlOut.WriteEndElement();
                xmlOut.WriteStartElement("Property");
                xmlOut.WriteAttributeString("BuildOutput", "[AppName].exe");
                xmlOut.WriteEndElement();
              xmlOut.WriteEndElement();

              xmlOut.WriteStartElement("FileSystem");
              xmlOut.WriteEndElement();

              xmlOut.WriteStartElement("Registry");
              xmlOut.WriteEndElement();

              xmlOut.WriteStartElement("Sandbox");
                xmlOut.WriteStartElement("FileSystem");
                xmlOut.WriteAttributeString("access", "Full");
                xmlOut.WriteEndElement();

                xmlOut.WriteStartElement("Registry");
                xmlOut.WriteAttributeString("access", "Full");
                xmlOut.WriteEndElement();
              xmlOut.WriteEndElement();

            xmlOut.WriteEndElement();
            xmlOut.WriteEndDocument();
            xmlOut.Flush();
            xmlOut.Close();

            xmlOut.Close();
          }
        }

        private void DisplayAutoLaunch()
        {
            if (virtPackage.GetProperty("AutoLaunch").Contains(";"))
            {
                String[] autoLaunches = virtPackage.GetProperty("AutoLaunch").Split(';');
                propertyAutoLaunch.Text = "";
                propertyAutoLaunch.AutoEllipsis = true;
                for (int i = 0; i < autoLaunches.Length; i++)
                {
                    String[] items = autoLaunches[i].Split('>');
                    if (items.Length < 3) continue;     // No Name
                    if (propertyAutoLaunch.Text != "")
                        propertyAutoLaunch.Text += ", ";
                    propertyAutoLaunch.Text += VirtPackage.FriendlyShortcutName(items[2]);
                }
                propertyAutoLaunch.Text = "Display menu: " + propertyAutoLaunch.Text;
            }
            else
            {
                String[] items = virtPackage.GetProperty("AutoLaunch").Split('>');
                if (items.Length >= 3)
                    propertyAutoLaunch.Text = items[0] + " (" + VirtPackage.FriendlyShortcutName(items[2]) + ")";
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
            int isolationType = virtPackage.GetIsolationMode();
            propertyIsolationDataMode.Checked = (isolationType == VirtPackage.ISOLATIONMODE_DATA);
            propertyIsolationIsolated.Checked = (isolationType == VirtPackage.ISOLATIONMODE_ISOLATED);
            propertyIsolationMerge.Checked = (isolationType == VirtPackage.ISOLATIONMODE_FULL_ACCESS);

            // Icon
            if (!String.IsNullOrEmpty(virtPackage.openedFile)){
              Icon icon = Win32Function.getIconFromFile(virtPackage.openedFile);
              if (icon != null)
              {
                propertyIcon.Image = icon.ToBitmap();
              }
            }

            // StopInheritance
            propertyStopInheritance.Text = virtPackage.GetProperty("StopInheritance");

            // CleanupOnExit
            String cleanupCommand = GetCleanUpStopCommand();
            if (cleanupCommand == "")
            {
                rdbCleanNone.Checked = true;
            }
            else
            {
                chkCleanAsk.Checked = cleanupCommand.Contains(CleanupOnExitOptionConfirm);
                chkCleanDoneDialog.Checked = !cleanupCommand.Contains(CleanupOnExitOptionQuiet);
                if (cleanupCommand.EndsWith(CleanupOnExitOptionRemoveReg))
                    rdbCleanRegOnly.Checked = true;
                else
                    rdbCleanAll.Checked = true;
            }


            // Expiration
            String expiration = virtPackage.GetProperty("Expiration");
            propertyExpiration.Checked = !String.IsNullOrEmpty(expiration);
            propertyExpirationDatePicker.Value = DateTime.Now;
            if (propertyExpiration.Checked)
            {
                String[] expirationItems = expiration.Split('/');
                if (expirationItems.Length == 3)
                {
                    int day = Convert.ToInt32(expirationItems[0]);
                    int month = Convert.ToInt32(expirationItems[1]);
                    int year = Convert.ToInt32(expirationItems[2]);
                    DateTime dt = new DateTime(year, month, day);
                    propertyExpirationDatePicker.Value = dt;
                }
            }

            this.Text = "Package Editor" + " - " + virtPackage.openedFile;
            dirty = false;
        }

        private void RemoveIfStartswith(ref String str, String value)
        {
            String newStr = str.TrimStart(' ');
            if (newStr.StartsWith(value))
                str = newStr.Remove(0, value.Length);
        }

        private string GetCleanUpStopCommand()
        {
            String[] OnStopUnvirtualized = virtPackage.GetProperty("OnStopUnvirtualized").Split(';');
            foreach (String stopCommand in OnStopUnvirtualized)
            {
                if (stopCommand.StartsWith(CleanupOnExitExe + '>') && (stopCommand.EndsWith(CleanupOnExitOptionRemove) || stopCommand.EndsWith(CleanupOnExitOptionRemoveReg)))
                {
                    String checkNoRemains = stopCommand;
                    RemoveIfStartswith(ref checkNoRemains, CleanupOnExitExe);
                    RemoveIfStartswith(ref checkNoRemains, ">");
                    RemoveIfStartswith(ref checkNoRemains, CleanupOnExitOptionConfirm);
                    RemoveIfStartswith(ref checkNoRemains, CleanupOnExitOptionQuiet);

                    RemoveIfStartswith(ref checkNoRemains, CleanupOnExitOptionRemoveReg);
                    RemoveIfStartswith(ref checkNoRemains, CleanupOnExitOptionRemove);
                    if (checkNoRemains == "")
                        return stopCommand;
                }
            }
            return "";
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
            if (propertyExpiration.Checked)
                Ret &= virtPackage.SetProperty("Expiration", propertyExpirationDatePicker.Value.ToString("dd/MM/yyyy"));
            else
                Ret &= virtPackage.SetProperty("Expiration", "");

            // propertyCleanupOnExit
            String str = virtPackage.GetProperty("OnStopUnvirtualized");
            String oldCleanupCommand = GetCleanUpStopCommand();
            String newCleanupCommand = "";
            if (!rdbCleanNone.Checked)
            {
                if (chkCleanAsk.Checked)
                    newCleanupCommand += ' ' + CleanupOnExitOptionConfirm;
                if (!chkCleanDoneDialog.Checked)
                    newCleanupCommand += ' ' + CleanupOnExitOptionQuiet;

                if (rdbCleanRegOnly.Checked)
                    newCleanupCommand += ' ' + CleanupOnExitOptionRemoveReg;
                if (rdbCleanAll.Checked)
                    newCleanupCommand += ' ' + CleanupOnExitOptionRemove;

                newCleanupCommand = CleanupOnExitExe + '>' + newCleanupCommand.Trim();
            }
            if (oldCleanupCommand == "")
                str += ";" + newCleanupCommand;
            else
            {
                str = ";" + str + ";";
                str = str.Replace(";" + oldCleanupCommand + ";", ";" + newCleanupCommand + ";");
                str = str.Replace(";;", ";");
                str = str.Trim(';');
            }
            Ret &= virtPackage.SetProperty("OnStopUnvirtualized", str);
            /*
            if (propertyCleanupOnExit.Checked)
            {
                if (!str.Contains(CleanupOnExitCmd))
                {
                    if (str != "")
                        str += ";";
                    str += CleanupOnExitCmd;
                    Ret &= virtPackage.SetProperty("OnStopUnvirtualized", str);
                }
            }
            else
            {
                if (str.Contains(CleanupOnExitCmd))
                {
                    str = str.Replace(CleanupOnExitCmd, "");
                    str = str.Replace(";;", ";");
                    str = str.Trim(';');
                    Ret &= virtPackage.SetProperty("OnStopUnvirtualized", str);
                }
            }*/

            // AutoLaunch (and SaveAutoLaunchCmd + SaveAutoLaunchMenu) already set by AutoLaunchForm

            // BaseDirName already set by SetProperty

            // Isolation. Note: it is allowed to have no checkbox selected at all.
            virtPackage.SetIsolationMode(
                propertyIsolationIsolated.Checked ? VirtPackage.ISOLATIONMODE_ISOLATED :
                propertyIsolationMerge.Checked ? VirtPackage.ISOLATIONMODE_FULL_ACCESS :
                propertyIsolationDataMode.Checked ? VirtPackage.ISOLATIONMODE_DATA :
                VirtPackage.ISOLATIONMODE_CUSTOM);

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
            //propertyCleanupOnExit.Checked = false;
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
            if (String.IsNullOrEmpty(virtPackage.openedFile))
            {
                // Its a new package.. so save as to get a filename.
                saveasToolStripMenuItem_Click(sender, e);
                return;
            }
            String tmpFileName = virtPackage.openedFile + ".new";
            DeleteFile(tmpFileName);
            //DeleteFile(virtPackage.openedFile + ".bak");
            if (PackageSave(tmpFileName))
            {
                // Release (close) original file, and delete it (otherwise it won't be erasable)
                String packageExeFile = virtPackage.openedFile;

                ThreadedRegLoadStop();
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
                Icon ico = Win32Function.getIconFromFile(openFileDialog.FileName);
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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
          ThreadedRegLoadStop();
        }

        private void lnkAutoLaunch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AutoLaunchForm autoLaunchForm = new AutoLaunchForm(virtPackage, fsEditor);
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

        static public bool ExecProg(String fileName, String args, bool wait, ref int exitCode)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo(fileName, args);
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                procStartInfo.CreateNoWindow = true;
                procStartInfo.UseShellExecute = false;
                proc.StartInfo = procStartInfo;
                proc.Start();
                if (wait)
                {
                    proc.WaitForExit();
                    exitCode = proc.ExitCode;
                }
                return true;
            }
            catch
            {
            }
            return false;
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
            if (System.IO.Path.GetFileName(files[0]).IndexOf("AppVirtDll.", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                String openedFile = "";
                CloseAndReopen_Before(ref openedFile);
                try
                {
                    // Syntax: myPath\Packager.exe -ChangeEngine AppName.virtual.exe AppVirtDll.dll
                    string myPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                    int exitCode = 0;
                    if (!ExecProg(openedFile, "-ChangeEngine \"" + files[0] + "\"", true, ref exitCode))
                        MessageBox.Show("Could not execute: " + Path.Combine(myPath, "Packager.exe"));
                }
                finally
                {
                    CloseAndReopn_After(openedFile);
                }
                return;
            }
            else if (System.IO.Path.GetExtension(files[0]).ToLower() != ".exe")
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
                this.BeginInvoke(fsEditor.Del_AddFOrFR, new object[] { parentNode, path });
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

        private void CloseAndReopen_Before(ref String openedFile)
        {
            if (this.dirty || fsEditor.dirty || regEditor.dirty)
            {
                MessageBox.Show("You have to save the package first");
                return;
            }
            openedFile = virtPackage.openedFile;
            virtPackage.Close();
        }

        private void CloseAndReopn_After(String openedFile)
        {
            if (!virtPackage.opened)
                virtPackage.Open(openedFile);
        }

        private void dropboxButton_Click(object sender, EventArgs e)
        {
#if DropBox
            String openedFile = "";
            CloseAndReopen_Before(ref openedFile);
            try
            {
                DropboxLogin dropLogin = new DropboxLogin();
                dropLogin.Publish(openedFile);
            }
            finally
            {
                CloseAndReopn_After(openedFile);
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

        private void regFilesList_SubItemClicked(object sender, SubItemEventArgs e)
        {
            //Mario:ToDo Bugfixes:regFilesList.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);
        }

        private void regFilesList_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
        {
            Registry.SetValue(regEditor.Masterkey, regEditor.Currentkey[regFilesList.Items.IndexOf(e.Item)].ToString(), e.DisplayText);
        }

        ListViewSorter fsFilesListSorter = new FileListViewSorter();
        private void fsFilesList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            fsFilesListSorter.Sort(fsFilesList, e.Column);
        }

        ListViewSorter regFilesListSorter = new RegListViewSorter();
        private void regFilesList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            regFilesListSorter.Sort(regFilesList, e.Column);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool OK = PackageClose();
            e.Cancel = !OK;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
          MainForm_Resize(null, null);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
          foreach(MRUitem item in mru.GetItems())
          {
            Icon ico = Win32Function.getIconFromFile(item.file);
            int imageId = imageListMRU.Images.Add(ico.ToBitmap(), Color.Empty);

            String fileName = Path.GetFileNameWithoutExtension(item.file);
            if (fileName.EndsWith(".virtual")) fileName = fileName.Remove(fileName.Length-8);
            if (fileName.EndsWith(".cameyo"))  fileName = fileName.Remove(fileName.Length-7);
            
            ListViewItem lvitem = listViewMRU.Items.Add(fileName);
            lvitem.ImageIndex = imageId;
            lvitem.Tag = item.file;
          }
          panelWelcome.Parent = this;
          panelWelcome.BringToFront();
          panelWelcome.Dock = DockStyle.None;
          panelWelcome.Dock = DockStyle.Fill;
          
          tabControl.TabPages.Remove(tabWelcome);
        }

        private void rdb_CheckedChanged(object sender, EventArgs e)
        {
            bool cleanup = !rdbCleanNone.Checked;
            chkCleanAsk.Enabled = cleanup;
            chkCleanDoneDialog.Enabled = cleanup;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PackageClose())
                return;
            if (!virtPackage.Create("New Package ID", 
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "AppVirtDll.dll"),
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Loader.exe")))
            {
                MessageBox.Show("Faild to create a new package.");
                return;
            }
            dirty = false;
            this.OnPackageOpen();
            fsEditor.OnPackageOpen();
            regEditor.OnPackageOpenBeforeUI();
            tabControl.SelectedIndex = 0;

            rdbCleanNone.Checked = true;
            rdbCleanRegOnly.Checked = false;
            rdbCleanAll.Checked = false;
            chkCleanAsk.Checked = true;
            chkCleanDoneDialog.Checked = false;

            EnableDisablePackageControls(true);
            regLoaded = true;
            regProgressTimer_Tick(null, null);
        }

        private void toolStripMenuItemExport_Click(object sender, EventArgs e)
        {
          regEditor.toolStripMenuItemExport_Click(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
          regRemoveBtn.PerformClick();
        }

        private void tabRegistry_DragDrop(object sender, DragEventArgs e)
        {
          dragging = true;

          String[] paths = (String[])e.Data.GetData(DataFormats.FileDrop);
          foreach (String path in paths)
          {
            this.BeginInvoke(regEditor.Del_AddFOrF, new object[] { path });
          }
        }

        private void tabRegistry_DragEnter(object sender, DragEventArgs e)
        {
          if (e.Data.GetDataPresent(DataFormats.FileDrop))
          {
            ((Control)sender).Focus();
            e.Effect = DragDropEffects.Copy;
            dragging = false;
          }
          else
          {
            e.Effect = DragDropEffects.None;
          }
        }

        private void tabRegistry_DragOver(object sender, DragEventArgs e)
        {
          //
        }

        private void listViewMRU_Resize(object sender, EventArgs e)
        {
          columnFileN.Width = listViewMRU.ClientSize.Width;
        }

        private void btnNewPackage_Click(object sender, EventArgs e)
        {
          newToolStripMenuItem_Click(sender, e);
        }

        private void btnEditPackage_Click(object sender, EventArgs e)
        {
          openToolStripMenuItem_Click(sender, e);
        }

        private void listViewMRU_Click(object sender, EventArgs e)
        {
          if (listViewMRU.SelectedItems.Count != 1)
            return;
          
          //ListViewItem[] l = listViewMRU.SelectedItems.ToString();
          PackageOpen((String)listViewMRU.SelectedItems[0].Tag);
        }

        void centerOnForm(Control c,int offset)
        {
          c.Left = (this.Width - c.Width) / 2 + offset;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
          centerOnForm(panelRecentPackages,0);
          centerOnForm(btnNewPackage, -btnNewPackage.Width / 2 - 10);
          centerOnForm(btnEditPackage, btnEditPackage.Width / 2 + 10);
          pictureBox2.Width = panelWelcome.ClientSize.Width;
        }

        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {
          Rectangle r = pictureBox2.ClientRectangle;
          if (r.Width > 0)
          {
            Bitmap bm = new Bitmap(r.Width, r.Height);

            pictureBox2.Image = null;
            pictureBox2.Image = bm;

            System.Drawing.Graphics graphicsObj;
            graphicsObj = Graphics.FromImage(bm); //pictureBox2.CreateGraphics();
            graphicsObj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            Rectangle myRectangle = new Rectangle(-20, -100, r.Width+40, 200);
            graphicsObj.FillEllipse(new SolidBrush(Color.FromArgb(64,64,64)), myRectangle);
            
            myRectangle = new Rectangle(0, 135, r.Width, 10);
            graphicsObj.FillRectangle(new SolidBrush(Color.FromArgb(64,64,64)), myRectangle);
            myRectangle = new Rectangle(0, 145, r.Width, 10);
            graphicsObj.FillRectangle(Brushes.DarkGray, myRectangle);
          }
        }
        private ListViewItem mHoverItem;
        private void listViewMRU_MouseHover(object sender, EventArgs e)
        {
          Point p = listViewMRU.PointToClient(new Point(MousePosition.X, MousePosition.Y));
          ListViewItem item = listViewMRU.GetItemAt(p.X, p.Y);
          if (object.ReferenceEquals(mHoverItem, item)) 
            return;
          if (mHoverItem != null) 
             mHoverItem.Font = listViewMRU.Font;
          if (item != null)
            item.Font = new Font(listViewMRU.Font, FontStyle.Underline);
          mHoverItem = item;
        }

        private void regImportBtn_Click(object sender, EventArgs e)
        {
          regEditor.RegFileExport();
        }

        private void regExportBtn_Click(object sender, EventArgs e)
        {
          regEditor.RegFileImport();
        }

        private void fileContextMenuDelete_Click(object sender, EventArgs e)
        { 
          fsRemoveBtn.PerformClick();
        }

        private void fileContextMenuProperties_Click(object sender, EventArgs e)
        {
          fsEditor.ShowProperties();
        }
    }

    class RegListViewSorter : ListViewSorter
    {
        protected override int CompareItems(ListViewItem x, ListViewItem y)
        {
            return String.Compare(x.SubItems[currentcolumn].Text, y.SubItems[currentcolumn].Text);
        }
    }

    public class FileListViewItem : ListViewItem
    {
        public ulong fileSize = 0;
        public VIRT_FILE_FLAGS flags = VIRT_FILE_FLAGS.NO_FLAGS;
    }

    class FileListViewSorter : ListViewSorter
    {
        bool isFileSizeColumn = false;
        public override void Sort(ListView List, int ColumnNumber)
        {
            isFileSizeColumn = List.Columns[ColumnNumber].Text == "Size";
            base.Sort(List, ColumnNumber);
        }
        protected override int CompareItems(ListViewItem x, ListViewItem y)
        {
            int res;
            if (isFileSizeColumn)
                res = ((FileListViewItem)x).fileSize.CompareTo(((FileListViewItem)y).fileSize);
            else
                res = String.Compare(x.SubItems[currentcolumn].Text, y.SubItems[currentcolumn].Text);
            return res;
        }
    }

    abstract class ListViewSorter : IComparer
    {
        protected int currentcolumn = -1;
        bool currentAsc = true;
        public virtual void Sort(ListView List, int ColumnNumber)
        {
            if (currentcolumn == ColumnNumber)
            {
                currentAsc = !currentAsc;
            }
            else
                currentAsc = true;
            currentcolumn = ColumnNumber;
            List.ListViewItemSorter = this;
            List.Sort();
        }
        public int Compare(object x, object y)
        {
            int res = CompareItems((ListViewItem)x, (ListViewItem)y);
            if (!currentAsc)
                res = -res;
            return res;
        }
        abstract protected int CompareItems(ListViewItem x, ListViewItem y);
    }

    public class MRUitem
    {
      public MRUitem(String name,String file){
        this.name = name;
        this.file = file;
      }
      public String name;
      public String file;
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
            regKey.DeleteValue(Convert.ToString(maxItems - 1), false);

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

        public List<MRUitem> GetItems()
        {
          List<MRUitem> result = new List<MRUitem>();

          String[] items = regKey.GetValueNames();
          List<String> list = new List<string>(items);
          list.Sort();
          for (int i = 0; i < list.Count; i++)
          {
            String fileNameValue = (String)regKey.GetValue(list[i]);
            if (fileNameValue == null || !File.Exists(fileNameValue))
              continue;
            result.Add(new MRUitem(list[i], fileNameValue));            
          }
          return result;
        }

        public void AddFile(String fileName)
        {
            // First browse through MRU items, deleting those that already contain this fileName
            String[] items = regKey.GetValueNames();
            for (int i = 0; i < items.Length; i++)
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
