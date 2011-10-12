namespace PackageEditor.FilesEditing
{
  partial class FileProperties
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.lblFileSize = new System.Windows.Forms.Label();
      this.lblFileName = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.lblFileCount = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.chkFileFlagISFILE = new System.Windows.Forms.CheckBox();
      this.chkFileFlagPKG_FILE = new System.Windows.Forms.CheckBox();
      this.chkFileFlagDELETED = new System.Windows.Forms.CheckBox();
      this.chkFileFlagDISCONNECTED = new System.Windows.Forms.CheckBox();
      this.chkFileFlagDEPLOYED = new System.Windows.Forms.CheckBox();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 29);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(52, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Filename:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 42);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(44, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Filesize:";
      // 
      // lblFileSize
      // 
      this.lblFileSize.AutoSize = true;
      this.lblFileSize.Location = new System.Drawing.Point(64, 42);
      this.lblFileSize.Name = "lblFileSize";
      this.lblFileSize.Size = new System.Drawing.Size(47, 13);
      this.lblFileSize.TabIndex = 10;
      this.lblFileSize.Text = "1.23 MB";
      // 
      // lblFileName
      // 
      this.lblFileName.AutoSize = true;
      this.lblFileName.Location = new System.Drawing.Point(64, 29);
      this.lblFileName.Name = "lblFileName";
      this.lblFileName.Size = new System.Drawing.Size(50, 13);
      this.lblFileName.TabIndex = 9;
      this.lblFileName.Text = "name.ext";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.lblFileCount);
      this.groupBox2.Controls.Add(this.label5);
      this.groupBox2.Controls.Add(this.lblFileSize);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Controls.Add(this.lblFileName);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
      this.groupBox2.Location = new System.Drawing.Point(10, 10);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(313, 75);
      this.groupBox2.TabIndex = 11;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "General properties";
      // 
      // lblFileCount
      // 
      this.lblFileCount.AutoSize = true;
      this.lblFileCount.Location = new System.Drawing.Point(64, 16);
      this.lblFileCount.Name = "lblFileCount";
      this.lblFileCount.Size = new System.Drawing.Size(19, 13);
      this.lblFileCount.TabIndex = 12;
      this.lblFileCount.Text = "12";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 16);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(53, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = "Filecount:";
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonOK.Location = new System.Drawing.Point(141, 273);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(88, 26);
      this.buttonOK.TabIndex = 12;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(235, 273);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(88, 26);
      this.buttonCancel.TabIndex = 13;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.chkFileFlagISFILE);
      this.groupBox1.Controls.Add(this.chkFileFlagPKG_FILE);
      this.groupBox1.Controls.Add(this.chkFileFlagDELETED);
      this.groupBox1.Controls.Add(this.chkFileFlagDISCONNECTED);
      this.groupBox1.Controls.Add(this.chkFileFlagDEPLOYED);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
      this.groupBox1.Location = new System.Drawing.Point(10, 85);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(313, 132);
      this.groupBox1.TabIndex = 14;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "File flags";
      // 
      // chkFileFlagISFILE
      // 
      this.chkFileFlagISFILE.AutoSize = true;
      this.chkFileFlagISFILE.Enabled = false;
      this.chkFileFlagISFILE.Location = new System.Drawing.Point(6, 19);
      this.chkFileFlagISFILE.Name = "chkFileFlagISFILE";
      this.chkFileFlagISFILE.Size = new System.Drawing.Size(144, 17);
      this.chkFileFlagISFILE.TabIndex = 0;
      this.chkFileFlagISFILE.Text = "ISFILE (File or directory?)";
      this.chkFileFlagISFILE.UseVisualStyleBackColor = true;
      // 
      // chkFileFlagPKG_FILE
      // 
      this.chkFileFlagPKG_FILE.AutoSize = true;
      this.chkFileFlagPKG_FILE.Location = new System.Drawing.Point(6, 111);
      this.chkFileFlagPKG_FILE.Name = "chkFileFlagPKG_FILE";
      this.chkFileFlagPKG_FILE.Size = new System.Drawing.Size(259, 17);
      this.chkFileFlagPKG_FILE.TabIndex = 4;
      this.chkFileFlagPKG_FILE.Text = "PKG_FILE (File/dir is part of the original package)";
      this.chkFileFlagPKG_FILE.UseVisualStyleBackColor = true;
      // 
      // chkFileFlagDELETED
      // 
      this.chkFileFlagDELETED.AutoSize = true;
      this.chkFileFlagDELETED.Location = new System.Drawing.Point(6, 42);
      this.chkFileFlagDELETED.Name = "chkFileFlagDELETED";
      this.chkFileFlagDELETED.Size = new System.Drawing.Size(188, 17);
      this.chkFileFlagDELETED.TabIndex = 1;
      this.chkFileFlagDELETED.Text = "DELETED (Deleted by virtual app)";
      this.chkFileFlagDELETED.UseVisualStyleBackColor = true;
      // 
      // chkFileFlagDISCONNECTED
      // 
      this.chkFileFlagDISCONNECTED.AutoSize = true;
      this.chkFileFlagDISCONNECTED.Enabled = false;
      this.chkFileFlagDISCONNECTED.Location = new System.Drawing.Point(6, 88);
      this.chkFileFlagDISCONNECTED.Name = "chkFileFlagDISCONNECTED";
      this.chkFileFlagDISCONNECTED.Size = new System.Drawing.Size(311, 17);
      this.chkFileFlagDISCONNECTED.TabIndex = 3;
      this.chkFileFlagDISCONNECTED.Text = "DISCONNECTED (Set when on-disk file is modified from DB)";
      this.chkFileFlagDISCONNECTED.UseVisualStyleBackColor = true;
      // 
      // chkFileFlagDEPLOYED
      // 
      this.chkFileFlagDEPLOYED.AutoSize = true;
      this.chkFileFlagDEPLOYED.Location = new System.Drawing.Point(6, 65);
      this.chkFileFlagDEPLOYED.Name = "chkFileFlagDEPLOYED";
      this.chkFileFlagDEPLOYED.Size = new System.Drawing.Size(212, 17);
      this.chkFileFlagDEPLOYED.TabIndex = 2;
      this.chkFileFlagDEPLOYED.Text = "DEPLOYED (Set upon first file opening)";
      this.chkFileFlagDEPLOYED.UseVisualStyleBackColor = true;
      // 
      // FileProperties
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(333, 312);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.groupBox2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "FileProperties";
      this.Padding = new System.Windows.Forms.Padding(10);
      this.Text = "FileProperties";
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lblFileSize;
    private System.Windows.Forms.Label lblFileName;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label lblFileCount;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox chkFileFlagISFILE;
    private System.Windows.Forms.CheckBox chkFileFlagPKG_FILE;
    private System.Windows.Forms.CheckBox chkFileFlagDELETED;
    private System.Windows.Forms.CheckBox chkFileFlagDISCONNECTED;
    private System.Windows.Forms.CheckBox chkFileFlagDEPLOYED;
  }
}