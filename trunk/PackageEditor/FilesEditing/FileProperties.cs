using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VirtPackageAPI;
using System.IO;

namespace PackageEditor.FilesEditing
{
  public partial class FileProperties : Form
  {
    VirtPackage virtPackage;
    List<FileData> files;

    public FileProperties(VirtPackage virtPackage)
    {
      InitializeComponent();
      this.virtPackage = virtPackage;
    }

    internal bool Open(List<FileData> files)
    {
      this.files = files;
      bool result = false;

      int count = files.Count;
      lblFileCount.Text = count.ToString();

      String fileNames = null;
      ulong totalSize = 0;
      VIRT_FILE_FLAGS min = VIRT_FILE_FLAGS.ALL_FLAGS;
      VIRT_FILE_FLAGS max = VIRT_FILE_FLAGS.NO_FLAGS;
      foreach (FileData fd in files)
      {
        totalSize += fd.virtFsNode.EndOfFile;
        min &= (VIRT_FILE_FLAGS)fd.virtFsNode.FileFlags;
        max |= (VIRT_FILE_FLAGS)fd.virtFsNode.FileFlags;
        
        fileNames += (fileNames == null ? "" : ", ") + Path.GetFileName(fd.virtFsNode.FileName);
      }
      lblFileName.Text = fileNames;
      lblFileSize.Text = Win32Function.StrFormatByteSize64(totalSize);


      chkFileFlagISFILE.CheckState = getCheckedState(VIRT_FILE_FLAGS.ISFILE, min, max);
      chkFileFlagPKG_FILE.CheckState = getCheckedState(VIRT_FILE_FLAGS.PKG_FILE, min, max);
      chkFileFlagDELETED.CheckState = getCheckedState(VIRT_FILE_FLAGS.DELETED, min, max);
      chkFileFlagDISCONNECTED.CheckState = getCheckedState(VIRT_FILE_FLAGS.DISCONNECTED, min, max);
      chkFileFlagDEPLOYED.CheckState = getCheckedState(VIRT_FILE_FLAGS.DEPLOYED, min, max);
      if (ShowDialog() == DialogResult.OK)
      {
        min = VIRT_FILE_FLAGS.NO_FLAGS;
        max = VIRT_FILE_FLAGS.ALL_FLAGS;
        if (chkFileFlagISFILE.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.ISFILE;
        if (chkFileFlagISFILE.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.ISFILE;
        if (chkFileFlagPKG_FILE.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.PKG_FILE;
        if (chkFileFlagPKG_FILE.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.PKG_FILE;
        if (chkFileFlagDELETED.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.DELETED;
        if (chkFileFlagDELETED.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.DELETED;
        if (chkFileFlagDISCONNECTED.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.DISCONNECTED;
        if (chkFileFlagDISCONNECTED.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.DISCONNECTED;
        if (chkFileFlagDEPLOYED.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.DEPLOYED;
        if (chkFileFlagDEPLOYED.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.DEPLOYED;

        foreach (FileData fd in files)
        {
          VIRT_FILE_FLAGS flags = fd.virtFsNode.FileFlags;
          flags |= min;
          flags &= max;
          fd.virtFsNode.FileFlags = flags;
        }
        result = true;
      }
      return result;
    }

    private CheckState getCheckedState(VIRT_FILE_FLAGS flag,VIRT_FILE_FLAGS min,VIRT_FILE_FLAGS max)
    {
      CheckState state = CheckState.Indeterminate;
      if ((min & flag) == (max & flag))
      {
        state = (min & flag) == flag ? CheckState.Checked : CheckState.Unchecked;
      } 
      return state;
    }
  }
}
