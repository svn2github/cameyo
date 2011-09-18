using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
#if DropBox
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.DropBox;
using AppLimit.CloudComputing.OAuth.Token;
#endif

namespace PackageEditor
{
    public partial class DropboxLogin : Form
    {
        #if DropBox
        public String username = "";
        public String password = "";
        public bool keepLogged = false;

        public DropboxLogin()
        {
            InitializeComponent();
        }

        public void Publish(String path)
        {
            if (path == null || !File.Exists(path))
            {
                MessageBox.Show("Path incorrect or file doesn't exist");
                return;
            }

            if (this.ConsumerKey == "" || this.ConsumerSecret == "")
            {
                MessageBox.Show("You need to set the ConsumerKey and ConsumerSecret parameters");
                return;
            }

            CloudStorage storage = new CloudStorage();
            DropBoxCredentialsToken storageToken = null;
            DropBoxConfiguration configuration = DropBoxConfiguration.GetStandardConfiguration();
            RegistryKey cameyoKey = Registry.CurrentUser.CreateSubKey(@"Software\Cameyo");
            Object DropBoxTokenKey = cameyoKey.GetValue("DropBoxTokenKey");
            Object DropBoxTokenSecret = cameyoKey.GetValue("DropBoxTokenSecret");

            if (DropBoxTokenKey != null
                    && DropBoxTokenSecret != null)
                storageToken = new DropBoxCredentialsToken(this.ConsumerKey,
                    this.ConsumerSecret,
                    DropBoxTokenKey.ToString(),
                    DropBoxTokenSecret.ToString());
            
            //*** Begin of connection code
            try
            {
                if (storageToken == null)
                {
                    if (this.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return;

                    DropBoxCredentials credentials = new DropBoxCredentials();
                    credentials.ConsumerKey = this.ConsumerKey;
                    credentials.ConsumerSecret = this.ConsumerSecret;
                    credentials.UserName = this.username;
                    credentials.Password = this.password;
                    this.password = "********************";
                    storageToken = new DropBoxCredentialsToken();
                    storageToken.AccessToken = storage.Open(configuration, credentials);
                    credentials.Password = "********************";
                    if (this.keepLogged)
                    {
                        cameyoKey.SetValue("DropBoxTokenKey", ((OAuthToken)storageToken.AccessToken).TokenKey);
                        cameyoKey.SetValue("DropBoxTokenSecret", ((OAuthToken)storageToken.AccessToken).TokenSecret);
                    }
                }
                else
                {
                    storage.Open(configuration, storageToken);
                }
            }
            catch
            {
                MessageBox.Show("Connection failed:\nInvalid e-mail or password?");
                return;
            }
            //*** End of connection code

            
            //*** Begin of publish file code
            // Create folder if it doesn't exist
            ICloudDirectoryEntry dir = null;
            try
            {
                dir = storage.GetFolder(@"/Cameyo Packages");
            }
            catch
            {
                try
                {
                    dir = storage.CreateFolder(@"/Cameyo Packages");
                }
                catch
                {
                    MessageBox.Show("Unable to create \"Dropbox/Cameyo Packages\" folder");
                    return;
                }
            }

            // If file exists and the user does not overwrite then return
            try
            {
                if (storage.GetFile(Path.GetFileName(path), dir) != null
                    && MessageBox.Show("The file \"" + Path.GetFileName(path) + "\" exists or it has not been \"permanently\" deleted from Dropbox\nDo you want to overwrite it?", "Overwrite?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }
            catch { }

            // Upload the file
            ICloudFileSystemEntry fsEntry = null;
            if ((fsEntry = storage.UploadFile(path, dir)) != null)
                MessageBox.Show("Uploaded successfully to Dropbox/Cameyo Packages/" + fsEntry.Name);
            else
                MessageBox.Show("Upload to Dropbox failed");
            //*** End of publish file code
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            username = userBox.Text;
            password = pwdBox.Text;
            pwdBox.Text = "********************";
            if (keepLoggedCheckBox.Checked)
                keepLogged = true;
            DialogResult = DialogResult.OK;
        }
        #endif
    }
}
