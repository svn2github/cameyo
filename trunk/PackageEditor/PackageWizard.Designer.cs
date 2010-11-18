namespace PackageEditor
{
    partial class PackageWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageWizard));
            this.wizard1 = new Gui.Wizard.Wizard();
            this.wpIntro = new Gui.Wizard.WizardPage();
            this.wpNameAndAutorun = new Gui.Wizard.WizardPage();
            this.wpIsolation = new Gui.Wizard.WizardPage();
            this.wpDone = new Gui.Wizard.WizardPage();
            this.infoPage1 = new Gui.Wizard.InfoPage();
            this.infoPage2 = new Gui.Wizard.InfoPage();
            this.header1 = new Gui.Wizard.Header();
            this.header2 = new Gui.Wizard.Header();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAppID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbAutoLaunchExe = new System.Windows.Forms.ListBox();
            this.wpLocalCache = new Gui.Wizard.WizardPage();
            this.header3 = new Gui.Wizard.Header();
            this.wizard1.SuspendLayout();
            this.wpIntro.SuspendLayout();
            this.wpNameAndAutorun.SuspendLayout();
            this.wpIsolation.SuspendLayout();
            this.wpDone.SuspendLayout();
            this.wpLocalCache.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizard1
            // 
            this.wizard1.Controls.Add(this.wpDone);
            this.wizard1.Controls.Add(this.wpLocalCache);
            this.wizard1.Controls.Add(this.wpIsolation);
            this.wizard1.Controls.Add(this.wpNameAndAutorun);
            this.wizard1.Controls.Add(this.wpIntro);
            this.wizard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizard1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizard1.Location = new System.Drawing.Point(0, 0);
            this.wizard1.Name = "wizard1";
            this.wizard1.Pages.AddRange(new Gui.Wizard.WizardPage[] {
            this.wpIntro,
            this.wpNameAndAutorun,
            this.wpIsolation,
            this.wpLocalCache,
            this.wpDone});
            this.wizard1.Size = new System.Drawing.Size(572, 412);
            this.wizard1.TabIndex = 0;
            // 
            // wpIntro
            // 
            this.wpIntro.Controls.Add(this.infoPage1);
            this.wpIntro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpIntro.IsFinishPage = false;
            this.wpIntro.Location = new System.Drawing.Point(0, 0);
            this.wpIntro.Margin = new System.Windows.Forms.Padding(0);
            this.wpIntro.Name = "wpIntro";
            this.wpIntro.Size = new System.Drawing.Size(572, 364);
            this.wpIntro.TabIndex = 1;
            this.wpIntro.ShowFromNext += new System.EventHandler(this.wpIntro_ShowFromNext);
            // 
            // wpNameAndAutorun
            // 
            this.wpNameAndAutorun.Controls.Add(this.lbAutoLaunchExe);
            this.wpNameAndAutorun.Controls.Add(this.label2);
            this.wpNameAndAutorun.Controls.Add(this.tbAppID);
            this.wpNameAndAutorun.Controls.Add(this.label1);
            this.wpNameAndAutorun.Controls.Add(this.header2);
            this.wpNameAndAutorun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpNameAndAutorun.IsFinishPage = false;
            this.wpNameAndAutorun.Location = new System.Drawing.Point(0, 0);
            this.wpNameAndAutorun.Name = "wpNameAndAutorun";
            this.wpNameAndAutorun.Size = new System.Drawing.Size(572, 364);
            this.wpNameAndAutorun.TabIndex = 2;
            // 
            // wpIsolation
            // 
            this.wpIsolation.Controls.Add(this.header1);
            this.wpIsolation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpIsolation.IsFinishPage = false;
            this.wpIsolation.Location = new System.Drawing.Point(0, 0);
            this.wpIsolation.Name = "wpIsolation";
            this.wpIsolation.Size = new System.Drawing.Size(572, 364);
            this.wpIsolation.TabIndex = 3;
            // 
            // wpDone
            // 
            this.wpDone.Controls.Add(this.infoPage2);
            this.wpDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpDone.IsFinishPage = false;
            this.wpDone.Location = new System.Drawing.Point(0, 0);
            this.wpDone.Name = "wpDone";
            this.wpDone.Size = new System.Drawing.Size(572, 364);
            this.wpDone.TabIndex = 4;
            // 
            // infoPage1
            // 
            this.infoPage1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoPage1.BackColor = System.Drawing.Color.White;
            this.infoPage1.Image = ((System.Drawing.Image)(resources.GetObject("infoPage1.Image")));
            this.infoPage1.Location = new System.Drawing.Point(1, 1);
            this.infoPage1.Margin = new System.Windows.Forms.Padding(1);
            this.infoPage1.Name = "infoPage1";
            this.infoPage1.PageText = "This wizard will allow you to quickly configure the behavior of your virtual appl" +
                "ication";
            this.infoPage1.PageTitle = "Virtual Application Wizard";
            this.infoPage1.Size = new System.Drawing.Size(571, 362);
            this.infoPage1.TabIndex = 0;
            // 
            // infoPage2
            // 
            this.infoPage2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoPage2.BackColor = System.Drawing.Color.White;
            this.infoPage2.Image = ((System.Drawing.Image)(resources.GetObject("infoPage2.Image")));
            this.infoPage2.Location = new System.Drawing.Point(1, 1);
            this.infoPage2.Margin = new System.Windows.Forms.Padding(1);
            this.infoPage2.Name = "infoPage2";
            this.infoPage2.PageText = "The wizard is ready to apply your settings to the virtual package.";
            this.infoPage2.PageTitle = "Ready!";
            this.infoPage2.Size = new System.Drawing.Size(571, 362);
            this.infoPage2.TabIndex = 2;
            // 
            // header1
            // 
            this.header1.BackColor = System.Drawing.SystemColors.Control;
            this.header1.CausesValidation = false;
            this.header1.Description = "Choose whether to isolate this program from the system\'s files and registry";
            this.header1.Image = ((System.Drawing.Image)(resources.GetObject("header1.Image")));
            this.header1.Location = new System.Drawing.Point(0, 0);
            this.header1.Margin = new System.Windows.Forms.Padding(0);
            this.header1.Name = "header1";
            this.header1.Size = new System.Drawing.Size(572, 64);
            this.header1.TabIndex = 0;
            this.header1.Title = "Isolation mode";
            // 
            // header2
            // 
            this.header2.BackColor = System.Drawing.SystemColors.Control;
            this.header2.CausesValidation = false;
            this.header2.Description = "Choose a unique identifier for this application, and the program that will execut" +
                "e when this package is opened.";
            this.header2.Image = ((System.Drawing.Image)(resources.GetObject("header2.Image")));
            this.header2.Location = new System.Drawing.Point(0, 0);
            this.header2.Margin = new System.Windows.Forms.Padding(0);
            this.header2.Name = "header2";
            this.header2.Size = new System.Drawing.Size(572, 64);
            this.header2.TabIndex = 1;
            this.header2.Title = "Application ID and main executable";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Application ID:";
            // 
            // tbAppID
            // 
            this.tbAppID.Location = new System.Drawing.Point(64, 101);
            this.tbAppID.Name = "tbAppID";
            this.tbAppID.Size = new System.Drawing.Size(438, 21);
            this.tbAppID.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Auto-launch executable:";
            // 
            // lbAutoLaunchExe
            // 
            this.lbAutoLaunchExe.FormattingEnabled = true;
            this.lbAutoLaunchExe.Location = new System.Drawing.Point(64, 156);
            this.lbAutoLaunchExe.Name = "lbAutoLaunchExe";
            this.lbAutoLaunchExe.Size = new System.Drawing.Size(438, 95);
            this.lbAutoLaunchExe.TabIndex = 6;
            // 
            // wpLocalCache
            // 
            this.wpLocalCache.Controls.Add(this.header3);
            this.wpLocalCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpLocalCache.IsFinishPage = false;
            this.wpLocalCache.Location = new System.Drawing.Point(0, 0);
            this.wpLocalCache.Name = "wpLocalCache";
            this.wpLocalCache.Size = new System.Drawing.Size(572, 364);
            this.wpLocalCache.TabIndex = 5;
            // 
            // header3
            // 
            this.header3.BackColor = System.Drawing.SystemColors.Control;
            this.header3.CausesValidation = false;
            this.header3.Description = "Choose where the application keeps its local cache and traces";
            this.header3.Image = ((System.Drawing.Image)(resources.GetObject("header3.Image")));
            this.header3.Location = new System.Drawing.Point(0, 0);
            this.header3.Margin = new System.Windows.Forms.Padding(0);
            this.header3.Name = "header3";
            this.header3.Size = new System.Drawing.Size(572, 64);
            this.header3.TabIndex = 1;
            this.header3.Title = "Mobility mode";
            // 
            // PackageWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 412);
            this.Controls.Add(this.wizard1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PackageWizard";
            this.Text = "Virtual application wizard";
            this.wizard1.ResumeLayout(false);
            this.wpIntro.ResumeLayout(false);
            this.wpNameAndAutorun.ResumeLayout(false);
            this.wpNameAndAutorun.PerformLayout();
            this.wpIsolation.ResumeLayout(false);
            this.wpDone.ResumeLayout(false);
            this.wpLocalCache.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Gui.Wizard.Wizard wizard1;
        private Gui.Wizard.WizardPage wpIntro;
        private Gui.Wizard.InfoPage infoPage1;
        private Gui.Wizard.WizardPage wpNameAndAutorun;
        private Gui.Wizard.WizardPage wpIsolation;
        private Gui.Wizard.WizardPage wpDone;
        private Gui.Wizard.Header header1;
        private Gui.Wizard.InfoPage infoPage2;
        private Gui.Wizard.Header header2;
        private System.Windows.Forms.ListBox lbAutoLaunchExe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbAppID;
        private System.Windows.Forms.Label label1;
        private Gui.Wizard.WizardPage wpLocalCache;
        private Gui.Wizard.Header header3;
    }
}