using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using VirtPackageAPI;
using System.Collections;
using System.IO;
using System.Threading;

namespace PackageEditor
{
    class RegistryEditor
    {
        private VirtPackage virtPackage;
        private TreeView fsFolderTree;
        private ListView fsFilesList;
        private Label fsFolderInfoFullName;
        private ComboBox fsFolderInfoIsolationCombo;
        private ToolStripButton regRemoveBtn;
        private ToolStripButton regEditBtn;
        private TreeHelper treeHelper;
        private RegistryKey workKey;
        public bool dirty;
        private string masterkey;
        private ArrayList currentkey;
        private AutoResetEvent regLoadAutoResetEvent;
        
        public string Masterkey
        {
            get { return masterkey; }
            set { masterkey = value; }
        }

        public ArrayList Currentkey
        {
            get { return currentkey; }
            set { currentkey = value; }
        }   

        public RegistryEditor(VirtPackage virtPackage, TreeView fsFolderTree, ListView fsFilesList, 
            Label fsFolderInfoFullName, ComboBox fsFolderInfoIsolationCombo,
            ToolStripButton regRemoveBtn, ToolStripButton regEditBtn)
        {
            this.virtPackage = virtPackage;
            this.fsFolderTree = fsFolderTree;
            this.fsFilesList = fsFilesList;
            this.fsFolderInfoFullName = fsFolderInfoFullName;
            this.fsFolderInfoIsolationCombo = fsFolderInfoIsolationCombo;
            this.regRemoveBtn = regRemoveBtn;
            this.regEditBtn = regEditBtn;

            fsFolderInfoFullName.Text = "";
            fsFolderInfoIsolationCombo.Text = "";
            fsFolderInfoIsolationCombo.Items.Add("Full acces");
            fsFolderInfoIsolationCombo.Items.Add("Isolated");
            fsFolderTree.AfterSelect += OnFolderTreeSelect;
            fsFolderInfoIsolationCombo.SelectionChangeCommitted += OnFolderSandboxChange;
            regRemoveBtn.Click += OnRemoveBtnClick;
            regEditBtn.Click += OnEditClick;
            dirty = false;
            treeHelper = new TreeHelper(virtPackage);
        }

        public void OnPackageOpenBeforeUI()     // Slow operation, for threading. Must NOT perform any UI.
        {
            try
            {
              regLoadAutoResetEvent = new AutoResetEvent(false);
              workKey = virtPackage.GetRegWorkKeyEx(regLoadAutoResetEvent);
              if (workKey == null)    // No virtual registry?
                return;
            }
            catch
            {
                // ThreadAbortException
            }
        }

        public void OnPackageOpenUI()
        {
            fsFolderTree.Nodes.Clear();
            TreeNode rootNode = fsFolderTree.Nodes.Add("Registry");
            if (workKey != null)
                PopulateSubKeys(workKey, "", rootNode);
            fsFolderTree.Nodes[0].Expand();     // Expand the "Registry" node
            fsFolderTree.SelectedNode = fsFolderTree.Nodes[0];
            dirty = false;
        }

        public void OnTabActivate()
        {
            // Force Isolation combo to refresh. This is in case the General Properties'
            // Isolation radio was changed, we need to refresh the Isolation combo here.
            if (fsFolderTree.Nodes.Count > 0)
            {
                fsFolderTree.SelectedNode = null;
                fsFolderTree.SelectedNode = fsFolderTree.Nodes[0];
            }
        }

        public void OnPackageClose()
        {
            fsFolderTree.Nodes.Clear();
            fsFilesList.Items.Clear();
            fsFolderInfoFullName.Text = "";
            fsFolderInfoIsolationCombo.Text = "";
            if (workKey != null)
            {
                /*try
                {
                    MessageBox.Show(workKey.Name);
                    workKey.DeleteSubKeyTree("");
                }
                catch
                {
                }*/
                workKey.Close();
                workKey = null;
            }
        }

        public bool OnPackageSave()
        {
            return (virtPackage.SaveRegWorkKey());
        }

        private void PopulateSubKeys(RegistryKey parentKey, String subKeyName, TreeNode curNode)
        {
            //if (subKeyName == "") return;   // No virtual registry exists (programs with no registry modifications)
            RegistryKey curKey = parentKey.OpenSubKey(subKeyName, false);
            if (curKey == null) return;     // Should never happen

            String[] subKeys = curKey.GetSubKeyNames();
            foreach (String subKey in subKeys)
            {
                TreeNode newNode;
                /*if (curNode == null)    // Very first tree node
                    newNode = fsFolderTree.Nodes.Add(subKey);
                else*/
                newNode = curNode.Nodes.Add(subKey);

                // Update image
                String fullName = treeHelper.GetFullNodeName(newNode);
                UInt32 sandboxFlags = virtPackage.GetRegistrySandbox(fullName, false);
                treeHelper.SetFolderNodeImage(newNode, false, sandboxFlags);

                // Recurse
                PopulateSubKeys(curKey, subKey, newNode);
            }

            // Add values
            /*Items := TStringList.Create;
            Reg.GetValueNames(Items);
            For I:=0 To Items.Count-1 Do Begin
            NewNode := TreeView.Items.AddChild(CurNode, Items[I]);
            SetNodeImage(NewNode, IMG_FILE);
            End;*/
            curKey.Close();
        }

        public void OnFolderTreeSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode folderNode = e.Node;
            fsFolderInfoFullName.Text = "";
            fsFolderInfoIsolationCombo.SelectedIndex = -1;
            if (folderNode == null)
                return;
            fsFolderInfoIsolationCombo.Enabled = (folderNode != fsFolderTree.Nodes[0]);

            // Fill info panel
            String fullName = treeHelper.GetFullNodeName(folderNode);
            fsFolderInfoFullName.Text = fullName;
            fsFolderInfoIsolationCombo.SelectedIndex = treeHelper.SandboxFlagsToComboIndex(
                virtPackage.GetRegistrySandbox(fsFolderInfoFullName.Text, false));

            // Fill fsFilesList
            fsFilesList.Items.Clear();
            if (workKey == null)
                return;
            RegistryKey regKey = workKey.OpenSubKey(fullName);
            if (regKey == null) 
                return;
            String[] values = regKey.GetValueNames();
            currentkey = new ArrayList();
            masterkey = "";
            currentkey.Clear();
            for (int i = 0; i < values.Length; i++)
            {
                ListViewItem newItem = new ListViewItem();
                newItem.Text = values[i];
                masterkey = regKey.ToString();
                currentkey.Add(newItem.Text);
                newItem.SubItems.Add((string)regKey.GetValue(values[i]).ToString());
                newItem.SubItems.Add((string)regKey.GetValueKind(values[i]).ToString());
                //newItem.SubItems.Add(ToDo: read value);
                fsFilesList.Items.Add(newItem);
            }
        }

        private void OnFolderSandboxChange(object sender, EventArgs e)
        {
            TreeNode node = fsFolderTree.SelectedNode;
            if (node == null)
                return;
            String fullName = treeHelper.GetFullNodeName(node);
            virtPackage.SetRegistrySandbox(fullName,
                treeHelper.ComboIndexToSandboxFlags(fsFolderInfoIsolationCombo.SelectedIndex), false);
            RefreshFolderNodeRecursively(node, 0);
            dirty = true;
        }

        private void OnRemoveBtnClick(object sender, EventArgs e)
        {
            TreeNode node = fsFolderTree.SelectedNode;
            if (node == null)
                return;
            if (MessageBox.Show("Delete key?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            String fullName = treeHelper.GetFullNodeName(node);
            try
            {
                workKey.DeleteSubKeyTree(fullName);
                node.Remove();
            }
            catch
            {
            }
            dirty = true;
        }

        private void OnEditClick(object sender, EventArgs e)
        {
            // Notification window
            PackageBuiltNotify packageBuiltNotify = new PackageBuiltNotify();
            String friendlyKeyName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(workKey.Name));
            packageBuiltNotify.Do("Registry editor will now open. Please edit entries under the key '" +
                friendlyKeyName + "'. Once you close the registry editor, your changes will be applied.", 
                "", "", "RegeditExplain");

            // Set LastKey to the work key
            RegistryKey regKey;
            try
            {
                regKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Applets\\Regedit");
                regKey.SetValue("LastKey", "Computer\\" + workKey.Name);
                regKey.Close();
            }
            catch
            {
                return;
            }

            // Run RegEdit
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("Regedit.exe", "-m");
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
            proc.StartInfo = procStartInfo;
            try
            {
                proc.Start();
                proc.WaitForExit();
                OnPackageOpenUI();
                dirty = true;
            }
            catch
            {
                // Exception will occur if user refuses UAC elevation for example..
            }
            
        }

        private void RefreshFolderNodeRecursively(TreeNode curNode, int iteration)
        {
            String fullName = treeHelper.GetFullNodeName(curNode);
            UInt32 sandboxFlags = virtPackage.GetRegistrySandbox(fullName, false);
            if (iteration == 0)
            {
                treeHelper.SetFolderNodeImage(curNode, false, sandboxFlags);
                if (curNode.Nodes.Count > 0)
                    RefreshFolderNodeRecursively(curNode.Nodes[0], iteration + 1);
            }
            else
            {
                while (curNode != null)
                {
                    treeHelper.SetFolderNodeImage(curNode, false, sandboxFlags);
                    if (curNode.Nodes.Count > 0)
                        RefreshFolderNodeRecursively(curNode.Nodes[0], iteration + 1);
                    curNode = curNode.NextNode;
                }
            }
        }

        class TRegistrySubstitute
        {
          public String originalKey;
          public String virtualKey;
          public TRegistrySubstitute(String originalKey, String virtualKey)
          {
            this.originalKey = originalKey;
            this.virtualKey = virtualKey;
          }
        }

        void enumRegKeyValues(StreamWriter sw, int rootKeyLen, RegistryKey key)
        {
          //string keyname = key.Name;      
          string keyname = key.Name.Substring(rootKeyLen);
          if (String.IsNullOrEmpty(keyname))
            return;

          List<TRegistrySubstitute> virtualKeys = new List<TRegistrySubstitute>();
          virtualKeys.Add(new TRegistrySubstitute("HKEY_CURRENT_USER", "\\%CurrentUser%\\"));
          virtualKeys.Add(new TRegistrySubstitute("HKEY_LOCAL_MACHINE", "\\MACHINE\\"));
          virtualKeys.Add(new TRegistrySubstitute("HKEY_CURRENT_USER\\Software\\Classes", "\\%CurrentUser%_Classes\\"));
          virtualKeys.Add(new TRegistrySubstitute("HKEY_USERS", "\\USER\\"));

          bool found = false;
          string keynameCheck = keyname + '\\';
          foreach (TRegistrySubstitute rs in virtualKeys)
          {
            if (keynameCheck.StartsWith(rs.virtualKey, StringComparison.InvariantCultureIgnoreCase))
            {
              keyname = keyname.Substring(rs.virtualKey.Length - 1).Insert(0, rs.originalKey);
              found = true;
              break;
            }

          }
          if (!found)
          {
            MessageBox.Show(String.Format("Not yet implemented registry key: {0}", keyname));
            return;
          }
          sw.WriteLine("[{0}]", keyname);
          foreach (string valuename in key.GetValueNames())
          {
            object value = null;
            String regvalue = null;
            RegistryValueKind kind = key.GetValueKind(valuename);
            if (kind != RegistryValueKind.Unknown)
            {
              value = key.GetValue(valuename);
              regvalue = value.ToString();
            }

            string regvaluename = valuename.Replace("\\", "\\\\");
            regvaluename = regvaluename.Replace("\"", "\\\"");
            if (regvaluename.Length == 0)
              regvaluename = "@";
            else
              regvaluename = "\"" + regvaluename + "\"";

            switch (kind)
            {
              case RegistryValueKind.Binary:
                {
                  //hex:12,3a,bd,ef,ff,a0
                  System.Byte[] x = (System.Byte[])value;
                  regvalue = "hex:";
                  for (int i = 0; i < x.Length; i++)
                  {
                    regvalue = regvalue + string.Format("{0:x2},", x[i]);
                  }
                  if (x.Length > 0)
                    regvalue = regvalue.Remove(regvalue.Length - 1);
                  break;
                }
              case RegistryValueKind.DWord:
                regvalue = string.Format("dword:{0:x8}", value);
                break;
              case RegistryValueKind.ExpandString:
                regvalue = "hex(2):";
                Char[] x2 = ((String)value).ToCharArray();
                for (int i = 0; i < x2.Length; i++)
                {
                  regvalue = regvalue + string.Format("{0:x2},{1:x2},", (int)x2[i], 0);
                }
                regvalue = regvalue + "00,00,";

                break;
              case RegistryValueKind.MultiString:
                string[] sa = (string[])value;

                regvalue = "hex(7):";
                for (int m = 0; m < sa.Length; m++)
                {

                  Char[] x = sa[m].ToCharArray();
                  for (int i = 0; i < x.Length; i++)
                  {
                    regvalue = regvalue + string.Format("{0:x2},{1:x2},", (int)x[i], 0);
                  }
                  regvalue = regvalue + "00,00,";
                }
                regvalue = regvalue + "00,00";
                break;
              case RegistryValueKind.QWord:
                break;
              case RegistryValueKind.String:
                regvalue = ((string)value).Replace("\\", "\\\\");
                regvalue = regvalue.Replace("\"", "\\\"");
                regvalue = string.Format("\"{0}\"", regvalue);
                break;
              case RegistryValueKind.Unknown:
                regvalue = "hex(0):";
                break;
              default:
                break;
            }
            sw.WriteLine("{0}={1}", regvaluename, regvalue);
          }
          sw.WriteLine("");
        }

        void enumRegKeys(StreamWriter sw, int rootKeyLen, RegistryKey key)
        {
          enumRegKeyValues(sw, rootKeyLen, key);
          string[] subkeys = key.GetSubKeyNames();
          for (int t = 0; t < subkeys.Length; t++)
          {
            RegistryKey subkey = key.OpenSubKey(subkeys[t]);
            String keyName = subkey.Name.Substring(rootKeyLen);
            enumRegKeys(sw, rootKeyLen, subkey);
            subkey.Close();
          }
        }

        public void toolStripMenuItemExport_Click(object sender, EventArgs e)
        {
          SaveFileDialog sfd = new SaveFileDialog();
          sfd.AddExtension = true;
          sfd.Filter = "Registry file (*.reg)|*.reg";
          if (sfd.ShowDialog() == DialogResult.OK)
          {
            String RegFileName = sfd.FileName;

            FileStream fs;
            BufferedStream bfs;
            StreamWriter sw;
            int rootKeyLen;
            fs = new FileStream(RegFileName, FileMode.Create);
            bfs = new BufferedStream(fs);
            sw = new StreamWriter(bfs);

            sw.WriteLine("Windows Registry Editor Version 5.00");
            sw.WriteLine("");

            // Read Registry Keys in the package
            RegistryKey key = workKey;

            String fullName = treeHelper.GetFullNodeName(fsFolderTree.SelectedNode);
            rootKeyLen = key.Name.Length;
            if (!String.IsNullOrEmpty(fullName))
              key = key.OpenSubKey(fullName);
            enumRegKeys(sw, rootKeyLen, key);
            sw.Close();
            bfs.Close();
            fs.Close();
          }
        }

        internal void threadedRegLoadStop()
        {
          if (regLoadAutoResetEvent != null)
            regLoadAutoResetEvent.Set();
        }
    }
}
