namespace Cameyo.OpenSrc.Client
{
    partial class AppPropertiesForm
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
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.tbExePath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblOccupiedSize = new System.Windows.Forms.Label();
            this.lblVirtEngine = new System.Windows.Forms.Label();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblPublisher = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbRepository = new System.Windows.Forms.TextBox();
            this.icon = new System.Windows.Forms.PictureBox();
            this.tabControl.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(347, 436);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabProperties);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(417, 418);
            this.tabControl.TabIndex = 2;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.tbRepository);
            this.tabProperties.Controls.Add(this.label14);
            this.tabProperties.Controls.Add(this.lblVersion);
            this.tabProperties.Controls.Add(this.lblPublisher);
            this.tabProperties.Controls.Add(this.lblDescription);
            this.tabProperties.Controls.Add(this.lblFileSize);
            this.tabProperties.Controls.Add(this.lblVirtEngine);
            this.tabProperties.Controls.Add(this.lblOccupiedSize);
            this.tabProperties.Controls.Add(this.label7);
            this.tabProperties.Controls.Add(this.label4);
            this.tabProperties.Controls.Add(this.tbExePath);
            this.tabProperties.Controls.Add(this.icon);
            this.tabProperties.Controls.Add(this.label6);
            this.tabProperties.Controls.Add(this.label5);
            this.tabProperties.Controls.Add(this.label3);
            this.tabProperties.Controls.Add(this.label2);
            this.tabProperties.Controls.Add(this.label1);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabProperties.Size = new System.Drawing.Size(409, 392);
            this.tabProperties.TabIndex = 1;
            this.tabProperties.Text = "Properties";
            this.tabProperties.UseVisualStyleBackColor = true;
            // 
            // tbExePath
            // 
            this.tbExePath.Location = new System.Drawing.Point(68, 22);
            this.tbExePath.Name = "tbExePath";
            this.tbExePath.ReadOnly = true;
            this.tbExePath.Size = new System.Drawing.Size(321, 20);
            this.tbExePath.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Occupied space:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "File size:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Description:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Publisher:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Version:";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(16, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(373, 2);
            this.label4.TabIndex = 15;
            this.label4.Text = "label4";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 181);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Virtualization:";
            // 
            // lblOccupiedSize
            // 
            this.lblOccupiedSize.AutoSize = true;
            this.lblOccupiedSize.Location = new System.Drawing.Point(107, 159);
            this.lblOccupiedSize.Name = "lblOccupiedSize";
            this.lblOccupiedSize.Size = new System.Drawing.Size(35, 13);
            this.lblOccupiedSize.TabIndex = 17;
            this.lblOccupiedSize.Text = "label8";
            // 
            // lblVirtEngine
            // 
            this.lblVirtEngine.AutoSize = true;
            this.lblVirtEngine.Location = new System.Drawing.Point(107, 181);
            this.lblVirtEngine.Name = "lblVirtEngine";
            this.lblVirtEngine.Size = new System.Drawing.Size(35, 13);
            this.lblVirtEngine.TabIndex = 18;
            this.lblVirtEngine.Text = "label9";
            // 
            // lblFileSize
            // 
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Location = new System.Drawing.Point(107, 137);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(41, 13);
            this.lblFileSize.TabIndex = 19;
            this.lblFileSize.Text = "label10";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(107, 102);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(41, 13);
            this.lblDescription.TabIndex = 20;
            this.lblDescription.Text = "label11";
            // 
            // lblPublisher
            // 
            this.lblPublisher.AutoSize = true;
            this.lblPublisher.Location = new System.Drawing.Point(107, 80);
            this.lblPublisher.Name = "lblPublisher";
            this.lblPublisher.Size = new System.Drawing.Size(41, 13);
            this.lblPublisher.TabIndex = 21;
            this.lblPublisher.Text = "label12";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(107, 58);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(41, 13);
            this.lblVersion.TabIndex = 22;
            this.lblVersion.Text = "label13";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 203);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 13);
            this.label14.TabIndex = 23;
            this.label14.Text = "Repository:";
            // 
            // tbRepository
            // 
            this.tbRepository.Location = new System.Drawing.Point(110, 200);
            this.tbRepository.Name = "tbRepository";
            this.tbRepository.ReadOnly = true;
            this.tbRepository.Size = new System.Drawing.Size(279, 20);
            this.tbRepository.TabIndex = 24;
            // 
            // icon
            // 
            this.icon.Location = new System.Drawing.Point(16, 16);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(32, 32);
            this.icon.TabIndex = 13;
            this.icon.TabStop = false;
            // 
            // AppPropertiesForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(441, 467);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppPropertiesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AppPropertiesForm";
            this.tabControl.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            this.tabProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabProperties;
        private System.Windows.Forms.TextBox tbRepository;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblPublisher;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.Label lblVirtEngine;
        private System.Windows.Forms.Label lblOccupiedSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbExePath;
        private System.Windows.Forms.PictureBox icon;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}