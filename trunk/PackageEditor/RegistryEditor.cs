using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using VirtPackageAPI;
using System.Collections;

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
                workKey = virtPackage.GetRegWorkKey();
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
                masterkey = regKey.ToString(); ;
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
                "", "RegeditExplain");

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

    }
}
