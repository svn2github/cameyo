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
            lvApps.ContextMenu.MenuItems[0].PerformClick();
            //AppItem appItem = (AppItem)(lvApps.SelectedItems[0].Tag);
            //ToDo: appItem.DefaultAction() ?
        }
    }

    public class AppItem
    {
        public DeployedApp deployedApp;

        public AppItem(DeployedApp deployedApp)
        {
            this.deployedApp = deployedApp;
        }
    }

    public class DeployedAppItem : AppItem
    {
        public DeployedAppItem(DeployedApp deployedApp) : base(deployedApp) { }
    }

    public class RunningAppItem : AppItem
    {
        public VirtPackage.RunningApp runningApp;
        public RunningAppItem(DeployedApp deployedApp) : base(deployedApp) { }
    }
}
