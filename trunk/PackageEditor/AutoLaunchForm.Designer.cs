namespace PackageEditor
{
    partial class AutoLaunchForm
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
          this.bottomPanel = new System.Windows.Forms.Panel();
          this.panel12 = new System.Windows.Forms.Panel();
          this.panel11 = new System.Windows.Forms.Panel();
          this.bkPanel = new System.Windows.Forms.Panel();
          this.panel1 = new System.Windows.Forms.Panel();
          this.btnCancel = new System.Windows.Forms.Button();
          this.propertyCmdRadio = new System.Windows.Forms.RadioButton();
          this.groupBox2 = new System.Windows.Forms.GroupBox();
          this.btnDown = new System.Windows.Forms.Button();
          this.btnUp = new System.Windows.Forms.Button();
          this.btnModify = new System.Windows.Forms.Button();
          this.btnRemove = new System.Windows.Forms.Button();
          this.btnAdd = new System.Windows.Forms.Button();
          this.propertyMenuLV = new System.Windows.Forms.ListView();
          this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
          this.btnVirtFilesBrowse = new System.Windows.Forms.Button();
          this.label4 = new System.Windows.Forms.Label();
          this.btnOk = new System.Windows.Forms.Button();
          this.propertyCmdArgs = new System.Windows.Forms.TextBox();
          this.propertyCmdText = new System.Windows.Forms.ComboBox();
          this.propertyMenuRadio = new System.Windows.Forms.RadioButton();
          this.bottomPanel.SuspendLayout();
          this.bkPanel.SuspendLayout();
          this.panel1.SuspendLayout();
          this.groupBox2.SuspendLayout();
          this.SuspendLayout();
          // 
          // bottomPanel
          // 
          this.bottomPanel.Controls.Add(this.panel12);
          this.bottomPanel.Controls.Add(this.panel11);
          this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
          this.bottomPanel.Location = new System.Drawing.Point(0, 319);
          this.bottomPanel.Name = "bottomPanel";
          this.bottomPanel.Size = new System.Drawing.Size(557, 47);
          this.bottomPanel.TabIndex = 20;
          // 
          // panel12
          // 
          this.panel12.BackgroundImage = global::PackageEditor.Properties.Resources.PackedgeEditorBG_BottomClient;
          this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panel12.Location = new System.Drawing.Point(0, 0);
          this.panel12.Name = "panel12";
          this.panel12.Size = new System.Drawing.Size(401, 47);
          this.panel12.TabIndex = 1;
          // 
          // panel11
          // 
          this.panel11.BackgroundImage = global::PackageEditor.Properties.Resources.PackedgeEditorBG_BottomRight;
          this.panel11.Dock = System.Windows.Forms.DockStyle.Right;
          this.panel11.Location = new System.Drawing.Point(401, 0);
          this.panel11.Name = "panel11";
          this.panel11.Size = new System.Drawing.Size(156, 47);
          this.panel11.TabIndex = 0;
          // 
          // bkPanel
          // 
          this.bkPanel.BackgroundImage = global::PackageEditor.Properties.Resources.PackedgeEditorBG_Client;
          this.bkPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
          this.bkPanel.Controls.Add(this.panel1);
          this.bkPanel.Dock = System.Windows.Forms.DockStyle.Fill;
          this.bkPanel.Location = new System.Drawing.Point(0, 0);
          this.bkPanel.Name = "bkPanel";
          this.bkPanel.Size = new System.Drawing.Size(557, 366);
          this.bkPanel.TabIndex = 21;
          // 
          // panel1
          // 
          this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.panel1.Controls.Add(this.btnCancel);
          this.panel1.Controls.Add(this.propertyCmdRadio);
          this.panel1.Controls.Add(this.groupBox2);
          this.panel1.Controls.Add(this.btnVirtFilesBrowse);
          this.panel1.Controls.Add(this.label4);
          this.panel1.Controls.Add(this.btnOk);
          this.panel1.Controls.Add(this.propertyCmdArgs);
          this.panel1.Controls.Add(this.propertyCmdText);
          this.panel1.Controls.Add(this.propertyMenuRadio);
          this.panel1.Location = new System.Drawing.Point(12, 12);
          this.panel1.Name = "panel1";
          this.panel1.Size = new System.Drawing.Size(532, 295);
          this.panel1.TabIndex = 28;
          // 
          // btnCancel
          // 
          this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
          this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          this.btnCancel.Location = new System.Drawing.Point(449, 259);
          this.btnCancel.Name = "btnCancel";
          this.btnCancel.Size = new System.Drawing.Size(75, 30);
          this.btnCancel.TabIndex = 0;
          this.btnCancel.Text = "Cancel";
          this.btnCancel.UseVisualStyleBackColor = true;
          this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
          // 
          // propertyCmdRadio
          // 
          this.propertyCmdRadio.AutoSize = true;
          this.propertyCmdRadio.Location = new System.Drawing.Point(12, 12);
          this.propertyCmdRadio.Name = "propertyCmdRadio";
          this.propertyCmdRadio.Size = new System.Drawing.Size(115, 17);
          this.propertyCmdRadio.TabIndex = 16;
          this.propertyCmdRadio.TabStop = true;
          this.propertyCmdRadio.Text = "Specific command:";
          this.propertyCmdRadio.UseVisualStyleBackColor = true;
          // 
          // groupBox2
          // 
          this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.groupBox2.Controls.Add(this.btnDown);
          this.groupBox2.Controls.Add(this.btnUp);
          this.groupBox2.Controls.Add(this.btnModify);
          this.groupBox2.Controls.Add(this.btnRemove);
          this.groupBox2.Controls.Add(this.btnAdd);
          this.groupBox2.Controls.Add(this.propertyMenuLV);
          this.groupBox2.Location = new System.Drawing.Point(31, 106);
          this.groupBox2.Name = "groupBox2";
          this.groupBox2.Size = new System.Drawing.Size(496, 147);
          this.groupBox2.TabIndex = 24;
          this.groupBox2.TabStop = false;
          // 
          // btnDown
          // 
          this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.btnDown.Image = global::PackageEditor.Properties.Resources.down;
          this.btnDown.Location = new System.Drawing.Point(468, 61);
          this.btnDown.Name = "btnDown";
          this.btnDown.Size = new System.Drawing.Size(24, 24);
          this.btnDown.TabIndex = 29;
          this.btnDown.UseVisualStyleBackColor = true;
          this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
          // 
          // btnUp
          // 
          this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.btnUp.Image = global::PackageEditor.Properties.Resources.up;
          this.btnUp.Location = new System.Drawing.Point(468, 34);
          this.btnUp.Name = "btnUp";
          this.btnUp.Size = new System.Drawing.Size(24, 24);
          this.btnUp.TabIndex = 28;
          this.btnUp.UseVisualStyleBackColor = true;
          this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
          // 
          // btnModify
          // 
          this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
          this.btnModify.Location = new System.Drawing.Point(186, 117);
          this.btnModify.Name = "btnModify";
          this.btnModify.Size = new System.Drawing.Size(83, 23);
          this.btnModify.TabIndex = 27;
          this.btnModify.Text = "&Modify";
          this.btnModify.UseVisualStyleBackColor = true;
          this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
          // 
          // btnRemove
          // 
          this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
          this.btnRemove.Location = new System.Drawing.Point(97, 117);
          this.btnRemove.Name = "btnRemove";
          this.btnRemove.Size = new System.Drawing.Size(83, 23);
          this.btnRemove.TabIndex = 26;
          this.btnRemove.Text = "&Remove";
          this.btnRemove.UseVisualStyleBackColor = true;
          this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
          // 
          // btnAdd
          // 
          this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
          this.btnAdd.Location = new System.Drawing.Point(8, 117);
          this.btnAdd.Name = "btnAdd";
          this.btnAdd.Size = new System.Drawing.Size(83, 23);
          this.btnAdd.TabIndex = 25;
          this.btnAdd.Text = "&Add";
          this.btnAdd.UseVisualStyleBackColor = true;
          this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
          // 
          // propertyMenuLV
          // 
          this.propertyMenuLV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.propertyMenuLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
          this.propertyMenuLV.FullRowSelect = true;
          this.propertyMenuLV.HideSelection = false;
          this.propertyMenuLV.Location = new System.Drawing.Point(8, 11);
          this.propertyMenuLV.MultiSelect = false;
          this.propertyMenuLV.Name = "propertyMenuLV";
          this.propertyMenuLV.Size = new System.Drawing.Size(454, 100);
          this.propertyMenuLV.TabIndex = 24;
          this.propertyMenuLV.UseCompatibleStateImageBehavior = false;
          this.propertyMenuLV.View = System.Windows.Forms.View.Details;
          // 
          // columnHeader1
          // 
          this.columnHeader1.Text = "Name";
          this.columnHeader1.Width = 211;
          // 
          // columnHeader2
          // 
          this.columnHeader2.Text = "Command";
          this.columnHeader2.Width = 229;
          // 
          // columnHeader3
          // 
          this.columnHeader3.Text = "Description";
          this.columnHeader3.Width = 180;
          // 
          // columnHeader4
          // 
          this.columnHeader4.Text = "Arguments";
          // 
          // btnVirtFilesBrowse
          // 
          this.btnVirtFilesBrowse.Image = global::PackageEditor.Properties.Resources.folder_closed_16_h;
          this.btnVirtFilesBrowse.Location = new System.Drawing.Point(499, 33);
          this.btnVirtFilesBrowse.Name = "btnVirtFilesBrowse";
          this.btnVirtFilesBrowse.Size = new System.Drawing.Size(24, 24);
          this.btnVirtFilesBrowse.TabIndex = 26;
          this.btnVirtFilesBrowse.UseVisualStyleBackColor = true;
          this.btnVirtFilesBrowse.Click += new System.EventHandler(this.btnVirtFilesBrowse_Click);
          // 
          // label4
          // 
          this.label4.AutoSize = true;
          this.label4.Location = new System.Drawing.Point(36, 65);
          this.label4.Name = "label4";
          this.label4.Size = new System.Drawing.Size(106, 13);
          this.label4.TabIndex = 28;
          this.label4.Text = "Arguments (optional):";
          // 
          // btnOk
          // 
          this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
          this.btnOk.Location = new System.Drawing.Point(368, 259);
          this.btnOk.Name = "btnOk";
          this.btnOk.Size = new System.Drawing.Size(75, 30);
          this.btnOk.TabIndex = 1;
          this.btnOk.Text = "OK";
          this.btnOk.UseVisualStyleBackColor = true;
          this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
          // 
          // propertyCmdArgs
          // 
          this.propertyCmdArgs.Location = new System.Drawing.Point(180, 62);
          this.propertyCmdArgs.Name = "propertyCmdArgs";
          this.propertyCmdArgs.Size = new System.Drawing.Size(314, 20);
          this.propertyCmdArgs.TabIndex = 27;
          // 
          // propertyCmdText
          // 
          this.propertyCmdText.FormattingEnabled = true;
          this.propertyCmdText.Location = new System.Drawing.Point(39, 34);
          this.propertyCmdText.Name = "propertyCmdText";
          this.propertyCmdText.Size = new System.Drawing.Size(455, 21);
          this.propertyCmdText.TabIndex = 25;
          // 
          // propertyMenuRadio
          // 
          this.propertyMenuRadio.AutoSize = true;
          this.propertyMenuRadio.Location = new System.Drawing.Point(12, 90);
          this.propertyMenuRadio.Name = "propertyMenuRadio";
          this.propertyMenuRadio.Size = new System.Drawing.Size(164, 17);
          this.propertyMenuRadio.TabIndex = 17;
          this.propertyMenuRadio.TabStop = true;
          this.propertyMenuRadio.Text = "Display menu to choose from:";
          this.propertyMenuRadio.UseVisualStyleBackColor = true;
          // 
          // AutoLaunchForm
          // 
          this.AcceptButton = this.btnOk;
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.CancelButton = this.btnCancel;
          this.ClientSize = new System.Drawing.Size(557, 366);
          this.Controls.Add(this.bottomPanel);
          this.Controls.Add(this.bkPanel);
          this.MinimumSize = new System.Drawing.Size(500, 400);
          this.Name = "AutoLaunchForm";
          this.ShowIcon = false;
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
          this.Text = "Auto launch";
          this.Load += new System.EventHandler(this.AutoLaunchForm_Load);
          this.bottomPanel.ResumeLayout(false);
          this.bkPanel.ResumeLayout(false);
          this.panel1.ResumeLayout(false);
          this.panel1.PerformLayout();
          this.groupBox2.ResumeLayout(false);
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel bkPanel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton propertyCmdRadio;
        private System.Windows.Forms.Button btnVirtFilesBrowse;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox propertyCmdText;
        private System.Windows.Forms.RadioButton propertyMenuRadio;
        private System.Windows.Forms.TextBox propertyCmdArgs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView propertyMenuLV;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
    }
}