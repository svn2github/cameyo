namespace Cameyo.OpenSrc.Client
{
    partial class AppsListView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Currently running", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Recently edited", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("On this computer", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppsListView));
            this.lvApps = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOccupiedSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgsAppsLarge = new System.Windows.Forms.ImageList(this.components);
            this.imgsAppsSmall = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.viewBtnDetails = new System.Windows.Forms.ToolStripButton();
            this.viewBtnIcons = new System.Windows.Forms.ToolStripButton();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvApps
            // 
            this.lvApps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colOccupiedSize,
            this.colVersion});
            this.lvApps.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "Currently running";
            listViewGroup1.Name = "runningGroup";
            listViewGroup2.Header = "Recently edited";
            listViewGroup2.Name = "recentlyEditedGroup";
            listViewGroup3.Header = "On this computer";
            listViewGroup3.Name = "deployedGroup";
            this.lvApps.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.lvApps.LargeImageList = this.imgsAppsLarge;
            this.lvApps.Location = new System.Drawing.Point(0, 25);
            this.lvApps.Name = "lvApps";
            this.lvApps.Size = new System.Drawing.Size(335, 270);
            this.lvApps.SmallImageList = this.imgsAppsSmall;
            this.lvApps.TabIndex = 5;
            this.lvApps.UseCompatibleStateImageBehavior = false;
            this.lvApps.DoubleClick += new System.EventHandler(this.lvApps_DoubleClick);
            this.lvApps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvApps_KeyDown);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 260;
            // 
            // colOccupiedSize
            // 
            this.colOccupiedSize.Text = "Occupied space";
            this.colOccupiedSize.Width = 100;
            // 
            // colVersion
            // 
            this.colVersion.Text = "Version";
            this.colVersion.Width = 100;
            // 
            // imgsAppsLarge
            // 
            this.imgsAppsLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgsAppsLarge.ImageSize = new System.Drawing.Size(32, 32);
            this.imgsAppsLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imgsAppsSmall
            // 
            this.imgsAppsSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgsAppsSmall.ImageSize = new System.Drawing.Size(16, 16);
            this.imgsAppsSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewBtnDetails,
            this.viewBtnIcons});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(335, 25);
            this.toolStrip.TabIndex = 4;
            this.toolStrip.Text = "toolStrip1";
            // 
            // viewBtnDetails
            // 
            this.viewBtnDetails.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.viewBtnDetails.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.viewBtnDetails.Image = ((System.Drawing.Image)(resources.GetObject("viewBtnDetails.Image")));
            this.viewBtnDetails.ImageTransparentColor = System.Drawing.Color.Red;
            this.viewBtnDetails.Name = "viewBtnDetails";
            this.viewBtnDetails.Size = new System.Drawing.Size(23, 22);
            this.viewBtnDetails.Text = "Details";
            this.viewBtnDetails.Click += new System.EventHandler(this.viewBtnDetails_Click);
            // 
            // viewBtnIcons
            // 
            this.viewBtnIcons.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.viewBtnIcons.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.viewBtnIcons.Image = ((System.Drawing.Image)(resources.GetObject("viewBtnIcons.Image")));
            this.viewBtnIcons.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.viewBtnIcons.Name = "viewBtnIcons";
            this.viewBtnIcons.Size = new System.Drawing.Size(23, 22);
            this.viewBtnIcons.Text = "Icons";
            this.viewBtnIcons.Click += new System.EventHandler(this.viewBtnIcons_Click);
            // 
            // refreshTimer
            // 
            this.refreshTimer.Enabled = true;
            this.refreshTimer.Interval = 300;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // AppsListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvApps);
            this.Controls.Add(this.toolStrip);
            this.Name = "AppsListView";
            this.Size = new System.Drawing.Size(335, 295);
            this.Load += new System.EventHandler(this.AppsListView_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView lvApps;
        public System.Windows.Forms.ColumnHeader colName;
        public System.Windows.Forms.ColumnHeader colOccupiedSize;
        public System.Windows.Forms.ColumnHeader colVersion;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton viewBtnDetails;
        private System.Windows.Forms.ToolStripButton viewBtnIcons;
        public System.Windows.Forms.ImageList imgsAppsLarge;
        public System.Windows.Forms.ImageList imgsAppsSmall;
        private System.Windows.Forms.Timer refreshTimer;
    }
}
