namespace PackageEditor
{
    partial class CustomEventsForm
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
          this.panel1 = new System.Windows.Forms.Panel();
          this.bottomPanel = new System.Windows.Forms.Panel();
          this.panel12 = new System.Windows.Forms.Panel();
          this.panel11 = new System.Windows.Forms.Panel();
          this.bkPanel = new System.Windows.Forms.Panel();
          this.panel8 = new System.Windows.Forms.Panel();
          this.btnOk = new System.Windows.Forms.Button();
          this.btnCancel = new System.Windows.Forms.Button();
          this.panel2 = new System.Windows.Forms.Panel();
          this.pictureBox1 = new System.Windows.Forms.PictureBox();
          this.btnDown = new System.Windows.Forms.Button();
          this.listBox = new System.Windows.Forms.ListBox();
          this.comboBox = new System.Windows.Forms.ComboBox();
          this.btnUp = new System.Windows.Forms.Button();
          this.btnErase = new System.Windows.Forms.Button();
          this.panel3 = new System.Windows.Forms.Panel();
          this.label2 = new System.Windows.Forms.Label();
          this.groupBox1 = new System.Windows.Forms.GroupBox();
          this.txtCmd = new System.Windows.Forms.TextBox();
          this.boxWait = new System.Windows.Forms.CheckBox();
          this.btnBrowse = new System.Windows.Forms.Button();
          this.panel4 = new System.Windows.Forms.Panel();
          this.btnAddSave = new System.Windows.Forms.Button();
          this.txtArgs = new System.Windows.Forms.TextBox();
          this.label1 = new System.Windows.Forms.Label();
          this.label4 = new System.Windows.Forms.Label();
          this.panel5 = new System.Windows.Forms.Panel();
          this.panel1.SuspendLayout();
          this.bottomPanel.SuspendLayout();
          this.panel8.SuspendLayout();
          this.panel2.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
          this.panel3.SuspendLayout();
          this.groupBox1.SuspendLayout();
          this.panel4.SuspendLayout();
          this.SuspendLayout();
          // 
          // panel1
          // 
          this.panel1.Controls.Add(this.bottomPanel);
          this.panel1.Controls.Add(this.bkPanel);
          this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panel1.Location = new System.Drawing.Point(0, 0);
          this.panel1.Name = "panel1";
          this.panel1.Size = new System.Drawing.Size(580, 423);
          this.panel1.TabIndex = 0;
          // 
          // bottomPanel
          // 
          this.bottomPanel.Controls.Add(this.panel12);
          this.bottomPanel.Controls.Add(this.panel11);
          this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
          this.bottomPanel.Location = new System.Drawing.Point(0, 376);
          this.bottomPanel.Name = "bottomPanel";
          this.bottomPanel.Size = new System.Drawing.Size(580, 47);
          this.bottomPanel.TabIndex = 22;
          // 
          // panel12
          // 
          this.panel12.BackgroundImage = global::PackageEditor.Properties.Resources.PackedgeEditorBG_BottomClient;
          this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panel12.Location = new System.Drawing.Point(0, 0);
          this.panel12.Name = "panel12";
          this.panel12.Size = new System.Drawing.Size(424, 47);
          this.panel12.TabIndex = 1;
          // 
          // panel11
          // 
          this.panel11.BackgroundImage = global::PackageEditor.Properties.Resources.PackedgeEditorBG_BottomRight;
          this.panel11.Dock = System.Windows.Forms.DockStyle.Right;
          this.panel11.Location = new System.Drawing.Point(424, 0);
          this.panel11.Name = "panel11";
          this.panel11.Size = new System.Drawing.Size(156, 47);
          this.panel11.TabIndex = 0;
          // 
          // bkPanel
          // 
          this.bkPanel.BackgroundImage = global::PackageEditor.Properties.Resources.PackedgeEditorBG_Client;
          this.bkPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
          this.bkPanel.Dock = System.Windows.Forms.DockStyle.Fill;
          this.bkPanel.Location = new System.Drawing.Point(0, 0);
          this.bkPanel.Name = "bkPanel";
          this.bkPanel.Size = new System.Drawing.Size(580, 423);
          this.bkPanel.TabIndex = 23;
          // 
          // panel8
          // 
          this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.panel8.Controls.Add(this.btnOk);
          this.panel8.Controls.Add(this.btnCancel);
          this.panel8.Controls.Add(this.panel2);
          this.panel8.Location = new System.Drawing.Point(12, 12);
          this.panel8.Name = "panel8";
          this.panel8.Size = new System.Drawing.Size(556, 354);
          this.panel8.TabIndex = 7;
          // 
          // btnOk
          // 
          this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.btnOk.Location = new System.Drawing.Point(392, 312);
          this.btnOk.Name = "btnOk";
          this.btnOk.Size = new System.Drawing.Size(75, 35);
          this.btnOk.TabIndex = 8;
          this.btnOk.Text = "&OK";
          this.btnOk.UseVisualStyleBackColor = true;
          this.btnOk.Click += new System.EventHandler(this.btnSave_Click);
          // 
          // btnCancel
          // 
          this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          this.btnCancel.Location = new System.Drawing.Point(473, 312);
          this.btnCancel.Name = "btnCancel";
          this.btnCancel.Size = new System.Drawing.Size(75, 35);
          this.btnCancel.TabIndex = 10;
          this.btnCancel.Text = "&Cancel";
          this.btnCancel.UseVisualStyleBackColor = true;
          this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
          // 
          // panel2
          // 
          this.panel2.Controls.Add(this.pictureBox1);
          this.panel2.Controls.Add(this.btnDown);
          this.panel2.Controls.Add(this.listBox);
          this.panel2.Controls.Add(this.comboBox);
          this.panel2.Controls.Add(this.btnUp);
          this.panel2.Controls.Add(this.btnErase);
          this.panel2.Controls.Add(this.panel3);
          this.panel2.Location = new System.Drawing.Point(0, 0);
          this.panel2.Name = "panel2";
          this.panel2.Size = new System.Drawing.Size(554, 306);
          this.panel2.TabIndex = 5;
          // 
          // pictureBox1
          // 
          this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.pictureBox1.Location = new System.Drawing.Point(3, 305);
          this.pictureBox1.Name = "pictureBox1";
          this.pictureBox1.Size = new System.Drawing.Size(556, 1);
          this.pictureBox1.TabIndex = 11;
          this.pictureBox1.TabStop = false;
          // 
          // btnDown
          // 
          this.btnDown.Image = global::PackageEditor.Properties.Resources.down;
          this.btnDown.Location = new System.Drawing.Point(96, 265);
          this.btnDown.Name = "btnDown";
          this.btnDown.Size = new System.Drawing.Size(36, 32);
          this.btnDown.TabIndex = 7;
          this.btnDown.UseVisualStyleBackColor = true;
          this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
          // 
          // listBox
          // 
          this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)));
          this.listBox.FormattingEnabled = true;
          this.listBox.Location = new System.Drawing.Point(12, 34);
          this.listBox.Name = "listBox";
          this.listBox.Size = new System.Drawing.Size(171, 225);
          this.listBox.TabIndex = 9;
          this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
          // 
          // comboBox
          // 
          this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
          this.comboBox.FormattingEnabled = true;
          this.comboBox.Items.AddRange(new object[] {
            "On start (unvirtualized)",
            "On start (virtualized)",
            "On stop (virtualized)",
            "On stop (unvirtualized)"});
          this.comboBox.Location = new System.Drawing.Point(12, 7);
          this.comboBox.Name = "comboBox";
          this.comboBox.Size = new System.Drawing.Size(171, 21);
          this.comboBox.TabIndex = 9;
          this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
          // 
          // btnUp
          // 
          this.btnUp.Image = global::PackageEditor.Properties.Resources.up;
          this.btnUp.Location = new System.Drawing.Point(54, 265);
          this.btnUp.Name = "btnUp";
          this.btnUp.Size = new System.Drawing.Size(36, 32);
          this.btnUp.TabIndex = 6;
          this.btnUp.UseVisualStyleBackColor = true;
          this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
          // 
          // btnErase
          // 
          this.btnErase.Image = global::PackageEditor.Properties.Resources.delete_16_h;
          this.btnErase.Location = new System.Drawing.Point(12, 265);
          this.btnErase.Name = "btnErase";
          this.btnErase.Size = new System.Drawing.Size(36, 32);
          this.btnErase.TabIndex = 5;
          this.btnErase.UseVisualStyleBackColor = true;
          this.btnErase.Click += new System.EventHandler(this.btnErase_Click);
          // 
          // panel3
          // 
          this.panel3.Controls.Add(this.label2);
          this.panel3.Controls.Add(this.groupBox1);
          this.panel3.Controls.Add(this.panel5);
          this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
          this.panel3.Location = new System.Drawing.Point(195, 0);
          this.panel3.Name = "panel3";
          this.panel3.Size = new System.Drawing.Size(359, 306);
          this.panel3.TabIndex = 1;
          // 
          // label2
          // 
          this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
          this.label2.Location = new System.Drawing.Point(34, 2);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(2, 337);
          this.label2.TabIndex = 43;
          // 
          // groupBox1
          // 
          this.groupBox1.Controls.Add(this.txtCmd);
          this.groupBox1.Controls.Add(this.boxWait);
          this.groupBox1.Controls.Add(this.btnBrowse);
          this.groupBox1.Controls.Add(this.panel4);
          this.groupBox1.Controls.Add(this.txtArgs);
          this.groupBox1.Controls.Add(this.label1);
          this.groupBox1.Controls.Add(this.label4);
          this.groupBox1.Location = new System.Drawing.Point(36, 0);
          this.groupBox1.Name = "groupBox1";
          this.groupBox1.Size = new System.Drawing.Size(323, 153);
          this.groupBox1.TabIndex = 38;
          this.groupBox1.TabStop = false;
          // 
          // txtCmd
          // 
          this.txtCmd.Location = new System.Drawing.Point(17, 30);
          this.txtCmd.Name = "txtCmd";
          this.txtCmd.Size = new System.Drawing.Size(262, 20);
          this.txtCmd.TabIndex = 0;
          // 
          // boxWait
          // 
          this.boxWait.AutoSize = true;
          this.boxWait.Location = new System.Drawing.Point(9, 95);
          this.boxWait.Name = "boxWait";
          this.boxWait.Size = new System.Drawing.Size(137, 17);
          this.boxWait.TabIndex = 3;
          this.boxWait.Text = "Wait until program ends";
          this.boxWait.UseVisualStyleBackColor = true;
          // 
          // btnBrowse
          // 
          this.btnBrowse.Image = global::PackageEditor.Properties.Resources.folder_closed_16_h;
          this.btnBrowse.Location = new System.Drawing.Point(282, 27);
          this.btnBrowse.Name = "btnBrowse";
          this.btnBrowse.Size = new System.Drawing.Size(25, 25);
          this.btnBrowse.TabIndex = 1;
          this.btnBrowse.UseVisualStyleBackColor = true;
          this.btnBrowse.Visible = false;
          // 
          // panel4
          // 
          this.panel4.Controls.Add(this.btnAddSave);
          this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
          this.panel4.Location = new System.Drawing.Point(3, 116);
          this.panel4.Name = "panel4";
          this.panel4.Size = new System.Drawing.Size(317, 34);
          this.panel4.TabIndex = 36;
          // 
          // btnAddSave
          // 
          this.btnAddSave.Location = new System.Drawing.Point(239, 6);
          this.btnAddSave.Name = "btnAddSave";
          this.btnAddSave.Size = new System.Drawing.Size(75, 23);
          this.btnAddSave.TabIndex = 4;
          this.btnAddSave.Text = "&Add";
          this.btnAddSave.UseVisualStyleBackColor = true;
          this.btnAddSave.Click += new System.EventHandler(this.btnAddSave_Click);
          // 
          // txtArgs
          // 
          this.txtArgs.Location = new System.Drawing.Point(17, 69);
          this.txtArgs.Name = "txtArgs";
          this.txtArgs.Size = new System.Drawing.Size(290, 20);
          this.txtArgs.TabIndex = 2;
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(6, 14);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(87, 13);
          this.label1.TabIndex = 34;
          this.label1.Text = "Command to run:";
          // 
          // label4
          // 
          this.label4.AutoSize = true;
          this.label4.Location = new System.Drawing.Point(6, 53);
          this.label4.Name = "label4";
          this.label4.Size = new System.Drawing.Size(106, 13);
          this.label4.TabIndex = 32;
          this.label4.Text = "Arguments (optional):";
          // 
          // panel5
          // 
          this.panel5.Location = new System.Drawing.Point(0, 0);
          this.panel5.Name = "panel5";
          this.panel5.Size = new System.Drawing.Size(36, 335);
          this.panel5.TabIndex = 0;
          // 
          // CustomEventsForm
          // 
          this.AcceptButton = this.btnOk;
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.CancelButton = this.btnCancel;
          this.ClientSize = new System.Drawing.Size(580, 423);
          this.Controls.Add(this.panel8);
          this.Controls.Add(this.panel1);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "CustomEventsForm";
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
          this.Text = "Custom events";
          this.Load += new System.EventHandler(this.CustomEventsForm_Load);
          this.panel1.ResumeLayout(false);
          this.bottomPanel.ResumeLayout(false);
          this.panel8.ResumeLayout(false);
          this.panel2.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
          this.panel3.ResumeLayout(false);
          this.groupBox1.ResumeLayout(false);
          this.groupBox1.PerformLayout();
          this.panel4.ResumeLayout(false);
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox boxWait;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnAddSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCmd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtArgs;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel bkPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBox;
    }
}