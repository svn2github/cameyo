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
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.listBox = new System.Windows.Forms.ListBox();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.boxWait = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtArgs = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnErase = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel8.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(669, 436);
            this.panel1.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panel2);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(669, 436);
            this.panel8.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.comboBox);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(669, 436);
            this.panel2.TabIndex = 5;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.listBox);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 21);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(310, 415);
            this.panel6.TabIndex = 5;
            // 
            // listBox
            // 
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(0, 0);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(310, 415);
            this.listBox.TabIndex = 9;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // comboBox
            // 
            this.comboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Items.AddRange(new object[] {
            "On start (unvirtualized)",
            "On start (virtualized)",
            "On stop (virtualized)",
            "On stop (unvirtualized)"});
            this.comboBox.Location = new System.Drawing.Point(0, 0);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(310, 21);
            this.comboBox.TabIndex = 9;
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.boxWait);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.txtCmd);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.txtArgs);
            this.panel3.Controls.Add(this.btnBrowse);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(310, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(359, 436);
            this.panel3.TabIndex = 1;
            // 
            // boxWait
            // 
            this.boxWait.AutoSize = true;
            this.boxWait.Location = new System.Drawing.Point(45, 91);
            this.boxWait.Name = "boxWait";
            this.boxWait.Size = new System.Drawing.Size(137, 17);
            this.boxWait.TabIndex = 3;
            this.boxWait.Text = "Wait until program ends";
            this.boxWait.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnAddSave);
            this.panel4.Location = new System.Drawing.Point(36, 114);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(323, 31);
            this.panel4.TabIndex = 36;
            // 
            // btnAddSave
            // 
            this.btnAddSave.Location = new System.Drawing.Point(234, 4);
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.Size = new System.Drawing.Size(75, 23);
            this.btnAddSave.TabIndex = 4;
            this.btnAddSave.Text = "&Add";
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.btnAddSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Command to run:";
            // 
            // txtCmd
            // 
            this.txtCmd.Location = new System.Drawing.Point(45, 26);
            this.txtCmd.Name = "txtCmd";
            this.txtCmd.Size = new System.Drawing.Size(273, 20);
            this.txtCmd.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Arguments (optional):";
            // 
            // txtArgs
            // 
            this.txtArgs.Location = new System.Drawing.Point(45, 65);
            this.txtArgs.Name = "txtArgs";
            this.txtArgs.Size = new System.Drawing.Size(301, 20);
            this.txtArgs.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = global::PackageEditor.Properties.Resources.folder_closed_16_h;
            this.btnBrowse.Location = new System.Drawing.Point(321, 23);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(25, 25);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnDown);
            this.panel5.Controls.Add(this.btnUp);
            this.panel5.Controls.Add(this.btnErase);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(36, 436);
            this.panel5.TabIndex = 0;
            // 
            // btnDown
            // 
            this.btnDown.Image = global::PackageEditor.Properties.Resources.down;
            this.btnDown.Location = new System.Drawing.Point(0, 58);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(33, 32);
            this.btnDown.TabIndex = 7;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Image = global::PackageEditor.Properties.Resources.up;
            this.btnUp.Location = new System.Drawing.Point(0, 30);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(33, 32);
            this.btnUp.TabIndex = 6;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnErase
            // 
            this.btnErase.Image = global::PackageEditor.Properties.Resources.delete_16_h;
            this.btnErase.Location = new System.Drawing.Point(0, 0);
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(33, 32);
            this.btnErase.TabIndex = 5;
            this.btnErase.UseVisualStyleBackColor = true;
            this.btnErase.Click += new System.EventHandler(this.btnErase_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnSave);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 383);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(669, 53);
            this.panel7.TabIndex = 42;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(580, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // CustomEventsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 436);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel1);
            this.Name = "CustomEventsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Custom events";
            this.Load += new System.EventHandler(this.CustomEventsForm_Load);
            this.panel8.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ListBox listBox;
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
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnSave;
    }
}