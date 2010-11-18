﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using VirtPackageAPI;

namespace PackageEditor
{
    public class FileData
    {
        public VirtFsNode virtFsNode;
        public String addedFrom;
        public bool deleted;
    }

    public class FolderTreeNode : TreeNode
    {
        public VirtFsNode virtFsNode;
        public List<FileData> childFiles;
        public UInt32 sandboxFlags;
        public bool deleted;
        public bool addedEmpty;         // User added this as a new empty folder
    }

    public class FileSystemEditor
    {
        private VirtPackage virtPackage;
        private TreeView fsFolderTree;
        private ListView fsFilesList;
        private Label fsFolderInfoFullName;
        private ComboBox fsFolderInfoIsolationCombo;
        private ToolStripButton fsAddBtn;
        private ToolStripButton fsRemoveBtn;
        private ToolStripButton fsAddEmptyDirBtn;
        private ToolStripButton fsSaveFileAsBtn;
        private TreeHelper treeHelper;
        public bool dirty;

        public FileSystemEditor(VirtPackage virtPackage, TreeView fsFolderTree, ListView fsFilesList, 
            Label fsFolderInfoFullName, ComboBox fsFolderInfoIsolationCombo,
            ToolStripButton fsAddBtn, ToolStripButton fsRemoveBtn, ToolStripButton fsAddEmptyDirBtn, 
            ToolStripButton fsSaveFileAsBtn)
        {
            this.virtPackage = virtPackage;
            this.fsFolderTree = fsFolderTree;
            this.fsFilesList = fsFilesList;
            this.fsFolderInfoFullName = fsFolderInfoFullName;
            this.fsFolderInfoIsolationCombo = fsFolderInfoIsolationCombo;
            this.fsAddBtn = fsAddBtn;
            this.fsRemoveBtn = fsRemoveBtn;
            this.fsAddEmptyDirBtn = fsAddEmptyDirBtn;
            this.fsSaveFileAsBtn = fsSaveFileAsBtn;

            fsFolderInfoFullName.Text = "";
            fsFolderInfoIsolationCombo.Text = "";
            fsFolderInfoIsolationCombo.Items.Add("Full access");
            fsFolderInfoIsolationCombo.Items.Add("Isolated");
            fsFolderTree.AfterSelect += OnFolderTreeSelect;
            fsFolderInfoIsolationCombo.SelectionChangeCommitted += OnFolderSandboxChange;
            fsAddBtn.Click += OnAddBtnClick;
            fsRemoveBtn.Click += OnRemoveBtnClick;
            fsAddEmptyDirBtn.Click += OnAddEmptyDirBtnClick;
            fsSaveFileAsBtn.Click += OnSaveFileAsBtnClick;
            dirty = false;
            treeHelper = new TreeHelper(virtPackage);
        }

        public void OnPackageOpen()
        {
            List<VirtFsNode> virtFsNodes = new List<VirtFsNode>();
            virtPackage.EnumFiles(ref virtFsNodes);
            fsFolderTree.Nodes.Clear();

            // Add first "FileSystem" root node"
            FolderTreeNode newNode = new FolderTreeNode();
            newNode.Text = "FileSystem";
            newNode.virtFsNode = new VirtFsNode();
            fsFolderTree.Nodes.Add(newNode);

            foreach (VirtFsNode virtFsNode in virtFsNodes)
            {
                AddFileOrFolder(virtFsNode, "");
            }

            // %Temp Internet% has predefined "WriteCopy" attribute, set by Packager
            // Add it here just so that user can edit its Sandbox flags.
            /*VirtFsNode tempVirtFsNode = new VirtFsNode();
            tempVirtFsNode.FileName = "%Temp Internet%";
            tempVirtFsNode.FileFlags = 0;               // Folder, not file
            AddFileOrFolder(tempVirtFsNode, "");*/

            fsFolderTree.Nodes[0].Expand();             // Expand the "FileSystem" node
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
        }

        private bool OnPackageSaveRecursive(TreeNodeCollection curNodes)
        {
            foreach (FolderTreeNode curFolder in curNodes)
            {
                if (curFolder.deleted)                      // Deleted Folder
                {
                    virtPackage.DeleteFile(treeHelper.GetFullNodeName((TreeNode)curFolder));
                    // Stop recursing inside this node
                }
                else
                {
                    if (curFolder.childFiles != null)
                    {
                        foreach (FileData child in curFolder.childFiles)
                        {
                            if (child.deleted)                  // Deleted File
                                virtPackage.DeleteFile(child.virtFsNode.FileName);
                            else if (child.addedFrom != "")     // Added File
                                virtPackage.AddFile(child.addedFrom, child.virtFsNode.FileName, false);
                        }
                    }
                    if (curFolder.addedEmpty)
                        virtPackage.AddEmptyDir(treeHelper.GetFullNodeName((TreeNode)curFolder), false);
                    OnPackageSaveRecursive(curFolder.Nodes);
                }
            }
            return true;
        }

        public bool OnPackageSave()
        {
            bool Ret = true;
            if (fsFolderTree.Nodes.Count == 0 ||        // No node other than "FileSystem"
                fsFolderTree.Nodes[0].Nodes.Count == 0) // No node other than "FileSystem"
                return true;                            // Empty tree..
            OnPackageSaveRecursive(fsFolderTree.Nodes[0].Nodes);
            //for (int i = 1; i < fsFolderTree.Nodes.Count; i++)
            //FolderTreeNode curNode = (FolderTreeNode)fsFolderTree.Nodes[0].Nodes[0];
            return (Ret);
        }

        private FolderTreeNode AddFileOrFolder(VirtFsNode virtFsNode, String SourceFileName)
        {
            FolderTreeNode newNode = null;
            FolderTreeNode curParent;
            TreeNode node;
            bool bFound;

            curParent = null;
            String fileName = "FileSystem\\" + virtFsNode.FileName;
            String[] tokens = fileName.Split('\\');
            foreach (String curToken in tokens)
            {
                if (curParent == null)
                    node = fsFolderTree.Nodes[0]; // There's always a top-node, since we've added "FileSystem" node
                //(fsFolderTree.Nodes.Count > 0 ? fsFolderTree.Nodes[0] : null);   // Top-most node
                else
                    node = curParent.FirstNode;

                bFound = false;
                while (node != null)
                {
                    if (node.Text == curToken)
                    {
                        curParent = (FolderTreeNode)node;
                        bFound = true;
                        break;
                    }
                    node = node.NextNode;
                }
                if (bFound == false)
                {
                    if ((virtFsNode.FileFlags & VirtPackage.VIRT_FILE_FLAGS_ISFILE) == 0)
                    {
                        // Adding Folder
                        newNode = new FolderTreeNode();
                        newNode.Text = Path.GetFileName(virtFsNode.FileName);
                        newNode.virtFsNode = virtFsNode;
                        newNode.sandboxFlags = virtPackage.GetFileSandbox(virtFsNode.FileName, false);
                        newNode.deleted = false;
                        newNode.addedEmpty = false;
                        treeHelper.SetFolderNodeImage(newNode, newNode.deleted, newNode.sandboxFlags);
                        //if (newNode.sandboxFlags == SANDBOXFLAGS_WRITE_COPY) newNode.ImageIndex = 3;
                        if (curParent != null)
                            curParent.Nodes.Add(newNode);
                        else
                            fsFolderTree.Nodes.Add(newNode);
                        curParent = newNode;
                    }
                    else
                    {
                        // Adding File
                        if (curParent != null)
                        {
                            FileData childFile = new FileData();
                            childFile.virtFsNode = virtFsNode;
                            if (curParent.childFiles == null)
                                curParent.childFiles = new List<FileData>();
                            childFile.addedFrom = SourceFileName;
                            childFile.deleted = false;
                            curParent.childFiles.Add(childFile);
                        }
                    }
                    if (curParent != null)
                    {
                        FolderTreeNode upperParent = curParent;
                        while (upperParent != null)
                        {
                            #pragma warning disable 1690
                            upperParent.virtFsNode.EndOfFile += virtFsNode.EndOfFile;   // CS1690 is OK
                            #pragma warning restore 1690
                            upperParent = (FolderTreeNode)upperParent.Parent;
                        }
                    }
                }
            }
            dirty = true;
            return newNode;
        }

        public void OnFolderTreeSelect(object sender, TreeViewEventArgs e)
        {
            FolderTreeNode folderNode = (FolderTreeNode)e.Node;
            fsFolderInfoFullName.Text = "";
            fsFolderInfoIsolationCombo.SelectedIndex = -1;
            if (folderNode == null)
                return;
            fsFolderInfoIsolationCombo.Enabled = (folderNode != fsFolderTree.Nodes[0]);
            VirtFsNode virtFsNode = folderNode.virtFsNode;    // Avoids CS1690

            // Fill info panel
            String fullName = treeHelper.GetFullNodeName(folderNode);
            fsFolderInfoFullName.Text = "[" + StrFormatByteSize64(virtFsNode.EndOfFile) + "] " + fullName;
            fsFolderInfoIsolationCombo.SelectedIndex = treeHelper.SandboxFlagsToComboIndex(
                virtPackage.GetFileSandbox(fullName, false));

            // Fill fsFilesList
            fsFilesList.Items.Clear();
            if (folderNode.childFiles != null)
            {
                //foreach (FileData childFile in folderNode.childFiles)
                for (int i = folderNode.childFiles.Count - 1; i >= 0; i--)
                {
                    FileData childFile = folderNode.childFiles[i];
                    ListViewItem newItem = new ListViewItem();
                    newItem.Text = Path.GetFileName(childFile.virtFsNode.FileName);
                    newItem.SubItems.Add(StrFormatByteSize64(childFile.virtFsNode.EndOfFile));
                    if (childFile.addedFrom != "")
                    {
                        if (folderNode.deleted)
                        {
                            folderNode.childFiles.Remove(childFile);       // Added file in a Removed folder. Forbidden!!
                            continue;
                        }
                        else
                            newItem.ForeColor = Color.Green;               // Newly-added
                    }
                    else if (folderNode.ImageIndex == 5)                   // deleted
                        newItem.ForeColor = Color.Red;
                    else if (childFile.deleted)
                        newItem.ForeColor = Color.Red;
                    fsFilesList.Items.Add(newItem);
                }
            }
        }

        private void OnFolderSandboxChange(object sender, EventArgs e)
        {
            FolderTreeNode node = (FolderTreeNode)fsFolderTree.SelectedNode;
            if (node == null)
                return;
            String fullName = treeHelper.GetFullNodeName(node);
            virtPackage.SetFileSandbox(fullName,
                treeHelper.ComboIndexToSandboxFlags(fsFolderInfoIsolationCombo.SelectedIndex), false);
            node.sandboxFlags = virtPackage.GetFileSandbox(fullName, false);
            RefreshFolderNodeRecursively(node, 0);
            dirty = true;
        }

        private void OnAddBtnClick(object sender, EventArgs e)
        {
            FolderTreeNode parentNode = (FolderTreeNode)fsFolderTree.SelectedNode;
            if (parentNode == null)
            {
                MessageBox.Show("Please select a folder to add files into");
                return;
            }
            if (parentNode.deleted)
            {
                MessageBox.Show("Folder was deleted");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (String srcFileName in openFileDialog.FileNames)
                {
                    VirtFsNode virtFsNode = new VirtFsNode();
                    #pragma warning disable 1690
                    virtFsNode.FileName = TreeHelper.FullPath(parentNode.virtFsNode.FileName, Path.GetFileName(srcFileName));
                    #pragma warning restore 1690
                    virtFsNode.FileFlags = VirtPackage.VIRT_FILE_FLAGS_ISFILE;
                    System.IO.FileInfo fi = new System.IO.FileInfo(srcFileName);
                    virtFsNode.EndOfFile = (ulong)fi.Length;
                    AddFileOrFolder(virtFsNode, srcFileName);       // Also sets dirty = true
                }
                RefreshFolderNodeRecursively(parentNode, 0);
                TreeViewEventArgs ev = new TreeViewEventArgs(parentNode);
                OnFolderTreeSelect(sender, ev);
            }
        }

        private void OnRemoveBtnClick(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection fileItems = fsFilesList.SelectedItems;
            FolderTreeNode folderNode;
            if (sender == fsRemoveBtn)      // First recursion iteration
                folderNode = (FolderTreeNode)fsFolderTree.SelectedNode;
            else
                folderNode = (FolderTreeNode)sender;

            if (fileItems.Count > 0)    // In this case, folderNode is always selected too
            {
                // Removing a File
                foreach (ListViewItem item in fileItems)
                {
                    if (folderNode.childFiles.Count == 0) continue;     // Should never happen
                    FileData fileData;
                    for (int i = folderNode.childFiles.Count - 1; i >= 0; i--)
                    {
                        fileData = folderNode.childFiles[i];
                        if (Path.GetFileName(fileData.virtFsNode.FileName) == item.Text)
                        {
                            if (fileData.addedFrom != "")   // Just added
                            {
                                item.Remove();
                                folderNode.childFiles.Remove(fileData);
                            }
                            else
                                folderNode.childFiles[i].deleted = true;
                            break;
                        }
                    }

                    /*if (item.ForeColor == Color.Green)      // Just added
                        item.Remove();
                    else
                        item.ForeColor = Color.Red;*/
                    //String FileName = TreeHelper.FullPath(folderNode.virtFsNode.FileName, item.Text);
                    //virtPackage.DeleteFile(FileName);
                }
                TreeViewEventArgs ev = new TreeViewEventArgs(folderNode);
                OnFolderTreeSelect(sender, ev);
                dirty = true;
            }
            else if (folderNode != null)
            {
                // Removing a Folder: recurse!
                FolderTreeNode curNode;
                if (sender == fsRemoveBtn)      // First recursion iteration
                {
                    curNode = folderNode;
                    curNode.deleted = true; //curNode.ImageIndex = curNode.SelectedImageIndex = 5;      // deleted
                    if (curNode.Nodes.Count > 0)
                        OnRemoveBtnClick(curNode.Nodes[0], e);
                    RefreshFolderNodeRecursively(curNode, 0);
                }
                else
                {
                    for (curNode = folderNode; curNode != null; curNode = (FolderTreeNode)curNode.NextNode)
                    {
                        curNode.deleted = true; //curNode.ImageIndex = curNode.SelectedImageIndex = 5;      // deleted
                        if (curNode.Nodes.Count > 0)
                            OnRemoveBtnClick(curNode.Nodes[0], e);
                    }
                }
                dirty = true;
            }
            else
                MessageBox.Show("Please select a folder/file to remove");
        }

        private void OnAddEmptyDirBtnClick(object sender, EventArgs e)
        {
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
            String newFolderName = "";
            if (TreeHelper.InputBox("Add empty folder", "Folder name:", ref newFolderName) != DialogResult.OK)
                return;
            if (newFolderName.Contains('\\'))
            {
                MessageBox.Show("Folder must not contain '\\'. Please specify one folder at a time.");
                return;
            }

            VirtFsNode virtFsNode = new VirtFsNode();
            #pragma warning disable 1690
            virtFsNode.FileName = TreeHelper.FullPath(parentNode.virtFsNode.FileName, newFolderName);
            #pragma warning restore 1690
            virtFsNode.FileFlags = 0;                       // Folder, not file

            //String[] subdirs = newFolderName.Split('\\');
            FolderTreeNode curParentNode = parentNode;
            //foreach (String subdir in subdirs)
            {
                foreach (FolderTreeNode childNode in curParentNode.Nodes)
                {
                    if (childNode.Text.Equals(newFolderName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        MessageBox.Show("Folder already exists");
                        return;
                    }
                }
            }
            
            FolderTreeNode newNode = AddFileOrFolder(virtFsNode, newFolderName);     // Also sets dirty = true
            if (newNode != null)
                newNode.addedEmpty = true;
            RefreshFolderNodeRecursively(parentNode, 0);
            TreeViewEventArgs ev = new TreeViewEventArgs(parentNode);
            OnFolderTreeSelect(sender, ev);
        }

        private void OnSaveFileAsBtnClick(object sender, EventArgs e)
        {
            FolderTreeNode node = (FolderTreeNode)fsFolderTree.SelectedNode;
            if (node == null)
            {
                MessageBox.Show("Please select a file to save");
                return;
            }

            ListView.SelectedListViewItemCollection fileItems = fsFilesList.SelectedItems;
            if (fileItems.Count == 0)    // In this case, folderNode is always selected too
            {
                MessageBox.Show("Please select a file, not a folder");
                return;
            }

            String targetDir = "";
            if (TreeHelper.InputBox("Save file", "Destination path on your hard disk:", ref targetDir) != DialogResult.OK)
                return;

            // Save files
            FolderTreeNode folderNode = (FolderTreeNode)fsFolderTree.SelectedNode;
            foreach (ListViewItem item in fileItems)
            {
                if (folderNode.childFiles.Count == 0) continue;     // Should never happen
                FileData fileData;
                for (int i = folderNode.childFiles.Count - 1; i >= 0; i--)
                {
                    fileData = folderNode.childFiles[i];
                    if (Path.GetFileName(fileData.virtFsNode.FileName) == item.Text)
                    {
                        if (fileData.addedFrom != "")   // Just added
                            MessageBox.Show("Cannot save a file that was just added: " + fileData.virtFsNode.FileName);
                        else
                        {
                            if (!virtPackage.ExtractFile(fileData.virtFsNode.FileName, targetDir))
                                MessageBox.Show("Cannot save file: " + fileData.virtFsNode.FileName + " to " + targetDir);
                        }
                        break;
                    }
                }
            }
        }

        // Misc internal functions
        [DllImport("shlwapi")]
        private static extern int StrFormatByteSize64(ulong qdw, StringBuilder pszBuf, uint cchBuf);
        private static string StrFormatByteSize64(ulong qdw)
        {
            StringBuilder StrSize = new StringBuilder(64);
            StrFormatByteSize64(qdw, StrSize, 64U);
            return StrSize.ToString();
        }

        private void RefreshFolderNodeRecursively(FolderTreeNode node, int iteration)
        {
            FolderTreeNode curNode = node;

            if (iteration == 0)
            {
                VirtFsNode virtFsNode = curNode.virtFsNode;         // Avoids CS1690
                curNode.sandboxFlags = virtPackage.GetFileSandbox(virtFsNode.FileName, false);
                treeHelper.SetFolderNodeImage(curNode, curNode.deleted, curNode.sandboxFlags);
                if (curNode.Nodes.Count > 0)
                    RefreshFolderNodeRecursively((FolderTreeNode)curNode.Nodes[0], iteration + 1);
            }
            else
            {
                while (curNode != null)
                {
                    VirtFsNode virtFsNode = curNode.virtFsNode;     // Avoids CS1690
                    curNode.sandboxFlags = virtPackage.GetFileSandbox(virtFsNode.FileName, false);
                    treeHelper.SetFolderNodeImage(curNode, curNode.deleted, curNode.sandboxFlags);
                    if (curNode.Nodes.Count > 0)
                        RefreshFolderNodeRecursively((FolderTreeNode)curNode.Nodes[0], iteration + 1);
                    curNode = (FolderTreeNode)curNode.NextNode;
                }
            }
        }

        /*[DllImport("kernel32.dll")]
        static extern bool FileTimeToLocalFileTime([In] ref System.Runtime.InteropServices.ComTypes.FILETIME lpFileTime, out System.Runtime.InteropServices.ComTypes.FILETIME lpLocalFileTime);
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern long FileTimeToSystemTime(ref System.Runtime.InteropServices.ComTypes.FILETIME FileTime, ref SYSTEMTIME SystemTime);*/
    }

    public class TreeHelper
    {
        private VirtPackage virtPackage;

        public TreeHelper(VirtPackage virtPackage)
        {
            this.virtPackage = virtPackage;
        }

        public int SandboxFlagsToComboIndex(UInt32 SandboxFlags)
        {
            switch (SandboxFlags)
            {
                case VirtPackage.SANDBOXFLAGS_MERGE:
                    return 0;
                case VirtPackage.SANDBOXFLAGS_WRITE_COPY:
                    return 1;
                /*case VirtPackage.SANDBOXFLAGS_FULL_ISOLATION:
                    return 2;*/
                default:
                    return 0;
            }
        }

        public UInt32 ComboIndexToSandboxFlags(int ComboIndex)
        {
            switch (ComboIndex)
            {
                case 0:
                    return VirtPackage.SANDBOXFLAGS_MERGE;
                case 1:
                    return VirtPackage.SANDBOXFLAGS_WRITE_COPY;
                /*case 2:
                    return VirtPackage.SANDBOXFLAGS_FULL_ISOLATION;*/
                default:
                    return 0;
            }
        }

        public void SetFolderNodeImage(TreeNode node, bool deleted, UInt32 sandboxFlags)
        {
            if (deleted)
                node.ImageIndex = node.SelectedImageIndex = 5;
            else
            {
                switch (sandboxFlags)
                {
                    case VirtPackage.SANDBOXFLAGS_MERGE:
                        node.ImageIndex = node.SelectedImageIndex = 0;
                        break;
                    case VirtPackage.SANDBOXFLAGS_WRITE_COPY:
                        node.ImageIndex = node.SelectedImageIndex = 1;
                        break;
                    default:
                        node.ImageIndex = node.SelectedImageIndex = 0;
                        break;
                }
            }
        }

        public String GetFullNodeName(TreeNode node)
        {
            String FullName = "";
            while (node != null && node.Parent != null)     // node.Parent != null avoids the first node ("FileSystem" or "Registry")
            {
                FullName = TreeHelper.FullPath(node.Text, FullName);
                node = node.Parent;
            }
            FullName = FullName.Trim('\\');
            return (FullName);
        }

        public static String FullPath(String dir, String file)
        {
            if (dir == null || dir.EndsWith("\\") || file.StartsWith("\\"))
                return dir + file;
            else
                return dir + "\\" + file;
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}
