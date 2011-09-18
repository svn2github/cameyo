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
    public partial class VirtFilesBrowse : Form
    {
        private VirtPackage virtPackage;
        private bool allowSelectFolder;
        const int IMGINDEX_FOLDER = 0;
        const int IMGINDEX_FILE = 3;

        public VirtFilesBrowse(VirtPackage virtPackage)
        {
            InitializeComponent();
            this.virtPackage = virtPackage;
        }

        private void Populate()
        {
            List<VirtFsNode> virtFsNodes = new List<VirtFsNode>();
            virtPackage.EnumFiles(ref virtFsNodes);
            fsFolderTree.Nodes.Clear();

            // Add first "FileSystem" root node"
            /*FolderTreeNode newNode = new FolderTreeNode();
            newNode.Text = "FileSystem";
            newNode.virtFsNode = new VirtFsNode();
            fsFolderTree.Nodes.Add(newNode);*/

            foreach (VirtFsNode virtFsNode in virtFsNodes)
            {
                AddFileOrFolder(virtFsNode, "");
            }
            //fsFolderTree.Nodes[0].Expand();             // Expand the "FileSystem" node
            //fsFolderTree.SelectedNode = fsFolderTree.Nodes[0];
        }

        private void AddFileOrFolder(VirtFsNode virtFsNode, String SourceFileName)
        {
            FolderTreeNode newNode;
            FolderTreeNode curParent;
            TreeNode node;
            bool bFound;

            curParent = null;
            String fileName = virtFsNode.FileName;
            String[] tokens = fileName.Split('\\');
            foreach (String curToken in tokens)
            {
                if (curParent == null)
                    node = (FolderTreeNode)(fsFolderTree.Nodes.Count > 0 ? fsFolderTree.Nodes[0] : null);
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
                    // Adding Folder
                    newNode = new FolderTreeNode();
                    newNode.Text = System.IO.Path.GetFileName(virtFsNode.FileName);
                    newNode.virtFsNode = virtFsNode;
                    newNode.ImageIndex = newNode.SelectedImageIndex =
                        (virtFsNode.FileFlags & VirtPackage.VIRT_FILE_FLAGS_ISFILE) > 0 ? IMGINDEX_FILE : IMGINDEX_FOLDER;
                    if (curParent != null)
                        curParent.Nodes.Add(newNode);
                    else
                        fsFolderTree.Nodes.Add(newNode);
                    curParent = newNode;
                    /*if ((virtFsNode.FileFlags & VirtPackage.VIRT_FILE_FLAGS_ISFILE) == 0)
                    {
                        // Adding Folder
                        newNode = new FolderTreeNode();
                        newNode.Text = System.IO.Path.GetFileName(virtFsNode.FileName);
                        newNode.virtFsNode = virtFsNode;
                        newNode.ImageIndex = newNode.SelectedImageIndex = 0;
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
                            curParent.childFiles.Add(childFile);
                        }
                    }*/
                }
            }
        }

        private String GetFullNodeName(TreeNode node)
        {
            String FullName = "";
            while (node != null)     // node.Parent != null avoids the first node ("FileSystem" or "Registry")
            {
                FullName = node.Text + "\\" + FullName;
                node = node.Parent;
            }
            FullName = FullName.Trim('\\');
            return (FullName);
        }

        public bool Do(ref String result, bool allowSelectFolder)
        {
            this.allowSelectFolder = allowSelectFolder;
            Populate();
            if (ShowDialog() == DialogResult.OK)
            {
                result = GetFullNodeName((TreeNode)fsFolderTree.SelectedNode);
                return true;
            }
            else
                return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (fsFolderTree.SelectedNode != null)
            {
                if (allowSelectFolder || fsFolderTree.SelectedNode.ImageIndex == IMGINDEX_FILE)
                    DialogResult = DialogResult.OK;
            }
        }
    }
}
