namespace PackageEditor
{
    partial class DataStorageForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.propertyLocalStorageExeDir = new System.Windows.Forms.RadioButton();
            this.propertyLocalStorageCustomDir = new System.Windows.Forms.TextBox();
            this.propertyLocalStorageCustom = new System.Windows.Forms.RadioButton();
            this.propertyLocalStorageDefault = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 119);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(473, 35);
            this.panel1.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(273, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 35);
            this.panel2.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(32, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(113, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(473, 119);
            this.panel3.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.propertyLocalStorageExeDir);
            this.groupBox1.Controls.Add(this.propertyLocalStorageCustomDir);
            this.groupBox1.Controls.Add(this.propertyLocalStorageCustom);
            this.groupBox1.Controls.Add(this.propertyLocalStorageDefault);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(473, 119);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Where should Cameyo store its local data for this application:";
            // 
            // propertyLocalStorageExeDir
            // 
            this.propertyLocalStorageExeDir.AutoSize = true;
            this.propertyLocalStorageExeDir.Location = new System.Drawing.Point(12, 42);
            this.propertyLocalStorageExeDir.Name = "propertyLocalStorageExeDir";
            this.propertyLocalStorageExeDir.Size = new System.Drawing.Size(177, 17);
            this.propertyLocalStorageExeDir.TabIndex = 1;
            this.propertyLocalStorageExeDir.TabStop = true;
            this.propertyLocalStorageExeDir.Text = "Under the executable\'s directory";
            this.propertyLocalStorageExeDir.UseVisualStyleBackColor = true;
            // 
            // propertyLocalStorageCustomDir
            // 
            this.propertyLocalStorageCustomDir.Location = new System.Drawing.Point(39, 88);
            this.propertyLocalStorageCustomDir.Name = "propertyLocalStorageCustomDir";
            this.propertyLocalStorageCustomDir.Size = new System.Drawing.Size(395, 20);
            this.propertyLocalStorageCustomDir.TabIndex = 3;
            // 
            // propertyLocalStorageCustom
            // 
            this.propertyLocalStorageCustom.AutoSize = true;
            this.propertyLocalStorageCustom.Location = new System.Drawing.Point(12, 65);
            this.propertyLocalStorageCustom.Name = "propertyLocalStorageCustom";
            this.propertyLocalStorageCustom.Size = new System.Drawing.Size(103, 17);
            this.propertyLocalStorageCustom.TabIndex = 2;
            this.propertyLocalStorageCustom.TabStop = true;
            this.propertyLocalStorageCustom.Text = "Custom location:";
            this.propertyLocalStorageCustom.UseVisualStyleBackColor = true;
            // 
            // propertyLocalStorageDefault
            // 
            this.propertyLocalStorageDefault.AutoSize = true;
            this.propertyLocalStorageDefault.Location = new System.Drawing.Point(12, 19);
            this.propertyLocalStorageDefault.Name = "propertyLocalStorageDefault";
            this.propertyLocalStorageDefault.Size = new System.Drawing.Size(362, 17);
            this.propertyLocalStorageDefault.TabIndex = 0;
            this.propertyLocalStorageDefault.TabStop = true;
            this.propertyLocalStorageDefault.Text = "Default: hard disk / Dropbox / USB (according to executable\'s location)";
            this.propertyLocalStorageDefault.UseVisualStyleBackColor = true;
            // 
            // DataStorageForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(473, 154);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "DataStorageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data storage";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox propertyLocalStorageCustomDir;
        private System.Windows.Forms.RadioButton propertyLocalStorageCustom;
        private System.Windows.Forms.RadioButton propertyLocalStorageDefault;
        private System.Windows.Forms.RadioButton propertyLocalStorageExeDir;
    }
}