using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VirtPackageAPI;
using Cameyo.OpenSrc.Common;

namespace Cameyo.OpenSrc.Client
{
    public partial class AppPropertiesForm : Form
    {
        public DeployedApp deployedApp { set { SetDeployedApp(value); } }
        private DeployedApp m_deployedApp;

        private void SetDeployedApp(DeployedApp deployedApp)
        {
            m_deployedApp = deployedApp;

            Text = deployedApp.AppID;
            //int imgIndex = imgsAppsLarge.Images.Add(ico.ToBitmap(), Color.Empty);

            tbExePath.Text = deployedApp.CarrierExeName;
            tbRepository.Text = deployedApp.BaseDirName;

            lblDescription.Text = deployedApp.FriendlyName;
            lblFileSize.Text = Win32Function.StrFormatByteSize64((ulong)Utils.GetFileSize(deployedApp.CarrierExeName));
            lblOccupiedSize.Text = Win32Function.StrFormatByteSize64((ulong)deployedApp.OccupiedSize);
            lblPublisher.Text = deployedApp.Publisher;
            lblVersion.Text = deployedApp.Version;
            lblVirtEngine.Text = deployedApp.EngineVersion;
        }

        public AppPropertiesForm(DeployedApp deployedApp, Image image)
        {
            InitializeComponent();
            SetDeployedApp(deployedApp);
            icon.Image = image;
        }
    }
}
