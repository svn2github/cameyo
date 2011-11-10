using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using VirtPackageAPI;
using Cameyo.OpenSrc.Common;

namespace Cameyo.OpenSrc.Client
{
    public partial class AppsListView : UserControl
    {
        private EventHandler OnRefresh;

        public AppsListView(EventHandler OnRefresh)
        {
            InitializeComponent();
            this.OnRefresh = OnRefresh;
        }

        private void AppsListView_Load(object sender, EventArgs e)
        {
            RefreshApps();
            refreshTimer.Enabled = true;
        }

        public void RefreshApps()
        {
            lock (lvApps)
            {
                // Clear list
                lvApps.Items.Clear();
                imgsAppsLarge.Images.Clear();
                imgsAppsSmall.Images.Clear();

                if (OnRefresh != null)
                    OnRefresh(this, new EventArgs());
            }
        }

        public ListViewItem AddApp(AppItem appItem, string groupName, bool extractIcon)
        {
            ListViewItem item = new ListViewItem(appItem.deployedApp.AppID);
            if (extractIcon)
            {
                Icon ico = Win32Function.getIconFromFile(appItem.deployedApp.CarrierExeName);
                int imgIndex = imgsAppsLarge.Images.Add(ico.ToBitmap(), Color.Empty);
                imgsAppsSmall.Images.Add(ico.ToBitmap(), Color.Empty);
                item.ImageIndex = imgIndex;
            }
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            if (appItem.deployedApp.IniProperties != null)
                item.SubItems[colVersion.Index].Text = appItem.deployedApp.Version;
            item.Tag = appItem;
            item.Group = lvApps.Groups[groupName];
            lvApps.Items.Add(item);
            return item;
        }

        public void SetItemImage(int index, Image img)
        {
            ListViewItem item = lvApps.Items[index];
            SetItemImage(item, img);
        }

        public void SetItemImage(ListViewItem item, Image img)
        {
            int imgIndex = imgsAppsLarge.Images.Add(img, Color.Empty);
            imgsAppsSmall.Images.Add(img);
            item.ImageIndex = imgIndex;
        }

        private void viewBtnIcons_Click(object sender, EventArgs e)
        {
            lvApps.View = View.LargeIcon;
        }

        private void viewBtnDetails_Click(object sender, EventArgs e)
        {
            lvApps.View = View.Details;
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            lock (lvApps)
            {
                foreach (ListViewItem item in lvApps.Items)
                {
                    AppItem appItem = (AppItem)item.Tag;
                    if (colOccupiedSize.Index != -1)
                    {
                        if (string.IsNullOrEmpty(item.SubItems[colOccupiedSize.Index].Text))
                            item.SubItems[colOccupiedSize.Index].Text = Win32Function.StrFormatByteSize64((ulong)appItem.deployedApp.OccupiedSize);
                    }
                }
            }
        }

        private void lvApps_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                RefreshApps();
        }

        private void lvApps_DoubleClick(object sender, EventArgs e)
        {
            if (lvApps.SelectedItems.Count != 1)
                return;
            AppItem appItem = (AppItem)(lvApps.SelectedItems[0].Tag);
            //appItem.ContextMenuStrip.Items[0].PerformClick();
            //ToDo: appItem.DefaultAction() ?
        }

        private void lvApps_MouseUp(object sender, MouseEventArgs e)
        {
            lvApps.ContextMenuStrip = dummyContextMenu;
            if (lvApps.SelectedItems.Count == 0)
                return;
            string className = "";
            for (int i = 0; i < lvApps.SelectedItems.Count; i++)
            {
                AppItem appItem = (AppItem)lvApps.SelectedItems[i].Tag;
                if (i == 0)
                    className = appItem.className;
                else
                {
                    if (appItem.className != className)
                        return;   // Class mismatch: several items selected, with different classes
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                AppItem appItem = (AppItem)lvApps.SelectedItems[0].Tag;
                lvApps.ContextMenuStrip = appItem.contextMenu;
                //dummyContextMenu.Items.Clear();
                /*for (int i = 0; i < appItem.contextMenu.Items.Count; i++)
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(
                        appItem.contextMenu.Items[i].Text, appItem.contextMenu.Items[i].Image, appItem.contextMenu.Items[i].Click);
                    menuItem.Click += appItem.contextMenu.Items[i].Click;
                    dummyContextMenu.Items.Add(menuItem);
                }*/
                //lvApps.ContextMenu.Show(lvApps, new Point(e.X, e.Y));
            }
        }
    }

    public class AppItem
    {
        public DeployedApp deployedApp;
        public string className;
        public ContextMenuStrip contextMenu;

        public AppItem(DeployedApp deployedApp, ContextMenuStrip contextMenu)
        {
            this.deployedApp = deployedApp;
            this.contextMenu = contextMenu;
        }
    }

    public class DeployedAppItem : AppItem
    {
        public DeployedAppItem(DeployedApp deployedApp, ContextMenuStrip contextMenu)
            : base(deployedApp, contextMenu) 
        {
            className = "DeployedAppItem";
        }
    }

    public class RunningAppItem : AppItem
    {
        public VirtPackage.RunningApp runningApp;
        public RunningAppItem(DeployedApp deployedApp, ContextMenuStrip contextMenu)
            : base(deployedApp, contextMenu)
        {
            className = "RunningAppItem";
        }
    }
}
