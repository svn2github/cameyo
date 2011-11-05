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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileProperties));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.chkFileFlagISFILE = new System.Windows.Forms.CheckBox();
            this.chkFileFlagPKG_FILE = new System.Windows.Forms.CheckBox();
            this.chkFileFlagDELETED = new System.Windows.Forms.CheckBox();
            this.chkFileFlagDISCONNECTED = new System.Windows.Forms.CheckBox();
            this.chkFileFlagDEPLOYED = new System.Windows.Forms.CheckBox();
            this.tbFullPath = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.pictureBox2);
            this.groupBox.Controls.Add(this.pictureBox1);
            this.groupBox.Controls.Add(this.tbFullPath);
            this.groupBox.Controls.Add(this.chkFileFlagDEPLOYED);
            this.groupBox.Controls.Add(this.chkFileFlagDELETED);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox.Location = new System.Drawing.Point(10, 10);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(438, 78);
            this.groupBox.TabIndex = 11;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Properties";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(264, 97);
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
            this.buttonCancel.Location = new System.Drawing.Point(358, 97);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(88, 26);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // chkFileFlagISFILE
            // 
            this.chkFileFlagISFILE.AutoSize = true;
            this.chkFileFlagISFILE.Enabled = false;
            this.chkFileFlagISFILE.Location = new System.Drawing.Point(13, 113);
            this.chkFileFlagISFILE.Name = "chkFileFlagISFILE";
            this.chkFileFlagISFILE.Size = new System.Drawing.Size(144, 17);
            this.chkFileFlagISFILE.TabIndex = 0;
            this.chkFileFlagISFILE.Text = "ISFILE (File or directory?)";
            this.chkFileFlagISFILE.UseVisualStyleBackColor = true;
            this.chkFileFlagISFILE.Visible = false;
            // 
            // chkFileFlagPKG_FILE
            // 
            this.chkFileFlagPKG_FILE.AutoSize = true;
            this.chkFileFlagPKG_FILE.Location = new System.Drawing.Point(19, 108);
            this.chkFileFlagPKG_FILE.Name = "chkFileFlagPKG_FILE";
            this.chkFileFlagPKG_FILE.Size = new System.Drawing.Size(259, 17);
            this.chkFileFlagPKG_FILE.TabIndex = 4;
            this.chkFileFlagPKG_FILE.Text = "PKG_FILE (File/dir is part of the original package)";
            this.chkFileFlagPKG_FILE.UseVisualStyleBackColor = true;
            this.chkFileFlagPKG_FILE.Visible = false;
            // 
            // chkFileFlagDELETED
            // 
            this.chkFileFlagDELETED.AutoSize = true;
            this.chkFileFlagDELETED.Location = new System.Drawing.Point(31, 55);
            this.chkFileFlagDELETED.Name = "chkFileFlagDELETED";
            this.chkFileFlagDELETED.Size = new System.Drawing.Size(246, 17);
            this.chkFileFlagDELETED.TabIndex = 1;
            this.chkFileFlagDELETED.Text = "Deleted: shown to virtual application as erased";
            this.chkFileFlagDELETED.UseVisualStyleBackColor = true;
            // 
            // chkFileFlagDISCONNECTED
            // 
            this.chkFileFlagDISCONNECTED.AutoSize = true;
            this.chkFileFlagDISCONNECTED.Enabled = false;
            this.chkFileFlagDISCONNECTED.Location = new System.Drawing.Point(30, 104);
            this.chkFileFlagDISCONNECTED.Name = "chkFileFlagDISCONNECTED";
            this.chkFileFlagDISCONNECTED.Size = new System.Drawing.Size(311, 17);
            this.chkFileFlagDISCONNECTED.TabIndex = 3;
            this.chkFileFlagDISCONNECTED.Text = "DISCONNECTED (Set when on-disk file is modified from DB)";
            this.chkFileFlagDISCONNECTED.UseVisualStyleBackColor = true;
            this.chkFileFlagDISCONNECTED.Visible = false;
            // 
            // chkFileFlagDEPLOYED
            // 
            this.chkFileFlagDEPLOYED.AutoSize = true;
            this.chkFileFlagDEPLOYED.Location = new System.Drawing.Point(31, 38);
            this.chkFileFlagDEPLOYED.Name = "chkFileFlagDEPLOYED";
            this.chkFileFlagDEPLOYED.Size = new System.Drawing.Size(266, 17);
            this.chkFileFlagDEPLOYED.TabIndex = 2;
            this.chkFileFlagDEPLOYED.Text = "Deployed: extracts immediately upon first execution";
            this.chkFileFlagDEPLOYED.UseVisualStyleBackColor = true;
            // 
            // tbFullPath
            // 
            this.tbFullPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFullPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFullPath.Location = new System.Drawing.Point(9, 19);
            this.tbFullPath.Name = "tbFullPath";
            this.tbFullPath.ReadOnly = true;
            this.tbFullPath.Size = new System.Drawing.Size(421, 13);
            this.tbFullPath.TabIndex = 13;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(9, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(9, 55);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            // 
            // FileProperties
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(458, 130);
            this.Controls.Add(this.chkFileFlagISFILE);
            this.Controls.Add(this.chkFileFlagPKG_FILE);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.chkFileFlagDISCONNECTED);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FileProperties";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FileProperties";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.CheckBox chkFileFlagISFILE;
    private System.Windows.Forms.CheckBox chkFileFlagPKG_FILE;
    private System.Windows.Forms.CheckBox chkFileFlagDELETED;
    private System.Windows.Forms.CheckBox chkFileFlagDISCONNECTED;
    private System.Windows.Forms.CheckBox chkFileFlagDEPLOYED;
    private System.Windows.Forms.TextBox tbFullPath;
    private System.Windows.Forms.PictureBox pictureBox2;
    private System.Windows.Forms.PictureBox pictureBox1;
  }
}