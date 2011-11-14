using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Cameyo.OpenSrc.Common
{
    public class Utils
    {
        static public String MyPath()
        {
            return Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        static public bool ExecProg(String fileName, String args, bool wait, ref int exitCode)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo(fileName, args);
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                procStartInfo.CreateNoWindow = true;
                procStartInfo.UseShellExecute = false;
                proc.StartInfo = procStartInfo;
                proc.Start();
                if (wait)
                {
                    proc.WaitForExit();
                    exitCode = proc.ExitCode;
                }
                return true;
            }
            catch
            {
            }
            return false;
        }

        static public long GetFileSize(String fileName)
        {
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
                return (fi.Length);
            }
            catch
            {
                return -1;
            }
        }
    }

    public class Win32Function
    {
        public static Icon getIconFromFile(String fileName)
        {
            /* The Icon.ExtractAssociatedIcon function does not support network drives. */
            Icon ico;
            if (fileName.StartsWith(@"\\"))
            {
                Win32imports.SHFILEINFO shinfo = new Win32imports.SHFILEINFO();
                IntPtr hIcon = Win32imports.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), (uint)(Win32imports.SHGFI.Icon | Win32imports.SHGFI.LargeIcon));
                ico = Icon.FromHandle(shinfo.hIcon);
            }
            else
            {
                ico = Icon.ExtractAssociatedIcon(fileName);
            }
            return ico;
        }

        public static string StrFormatByteSize64(ulong qdw)
        {
            StringBuilder StrSize = new StringBuilder(64);
            Win32imports.StrFormatByteSize64(qdw, StrSize, 64U);
            return StrSize.ToString();
        }
    }

    public class Win32imports
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [Flags]
        public enum SHGFI : int
        {
            /// <summary>get icon</summary>
            Icon = 0x000000100,
            /// <summary>get display name</summary>
            DisplayName = 0x000000200,
            /// <summary>get type name</summary>
            TypeName = 0x000000400,
            /// <summary>get attributes</summary>
            Attributes = 0x000000800,
            /// <summary>get icon location</summary>
            IconLocation = 0x000001000,
            /// <summary>return exe type</summary>
            ExeType = 0x000002000,
            /// <summary>get system icon index</summary>
            SysIconIndex = 0x000004000,
            /// <summary>put a link overlay on icon</summary>
            LinkOverlay = 0x000008000,
            /// <summary>show icon in selected state</summary>
            Selected = 0x000010000,
            /// <summary>get only specified attributes</summary>
            Attr_Specified = 0x000020000,
            /// <summary>get large icon</summary>
            LargeIcon = 0x000000000,
            /// <summary>get small icon</summary>
            SmallIcon = 0x000000001,
            /// <summary>get open icon</summary>
            OpenIcon = 0x000000002,
            /// <summary>get shell size icon</summary>
            ShellIconSize = 0x000000004,
            /// <summary>pszPath is a pidl</summary>
            PIDL = 0x000000008,
            /// <summary>use passed dwFileAttribute</summary>
            UseFileAttributes = 0x000000010,
            /// <summary>apply the appropriate overlays</summary>
            AddOverlays = 0x000000020,
            /// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
            OverlayIndex = 0x000000040,
        }

        // Misc internal functions
        [DllImport("shlwapi")]
        public static extern int StrFormatByteSize64(ulong qdw, StringBuilder pszBuf, uint cchBuf);
    }

    public class PleaseWait
    {
        #region PleaseWaitDialog
        private class PleaseWaitMsg
        {
            public String iconFileName;
            public String title;
            public String msg;
            public PleaseWaitMsg(String title, String msg, String iconFileName)
            {
                this.title = title;
                this.msg = msg;
                this.iconFileName = iconFileName;
            }
        }

        //System.Threading.AutoResetEvent pleaseWaitDialogEvent;
        private class PleaseWaitDialog
        {
            private PictureBox icon;
            private Label msg;
            private Form dialog;
            Icon iconFile;
            //PleaseWaitMsg pleaseWaitMsg;

            private void InitializeComponent()
            {
                icon = new PictureBox();
                icon.Location = new Point(12, 12);
                icon.Size = new Size(48, 48);

                msg = new Label();
                msg.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                msg.AutoSize = true;
                msg.Location = new Point(70, 12);

                dialog = new Form();
                dialog.ClientSize = new Size(400, 70);
                dialog.Controls.Add(this.msg);
                dialog.Controls.Add(this.icon);
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MinimizeBox = false;
                dialog.ShowInTaskbar = false;
                dialog.ControlBox = false;
                dialog.StartPosition = FormStartPosition.CenterScreen;
                dialog.TopMost = true;
            }

            public PleaseWaitDialog()
            {
                InitializeComponent();
            }

            public void Display(PleaseWaitMsg pleaseWaitMsg)
            {
                try
                {
                    if (!String.IsNullOrEmpty(pleaseWaitMsg.iconFileName))
                    {
                        iconFile = Win32Function.getIconFromFile(pleaseWaitMsg.iconFileName);
                        icon.Image = iconFile.ToBitmap();
                    }
                }
                catch { }
                dialog.Text = pleaseWaitMsg.title;
                msg.Text = pleaseWaitMsg.msg;
                dialog.ClientSize = new Size(Math.Max(msg.Width + 100, 250), 70);
                msg.Location = new Point(dialog.ClientSize.Width / 2 - msg.Width / 2, 12);
                dialog.Show(null);
                EventWaitHandle pleaseWaitDialogEvent = AutoResetEvent.OpenExisting("pleaseWaitDialogEvent");
                while (!pleaseWaitDialogEvent.WaitOne(10, false))
                    Application.DoEvents();
            }
        }
        #endregion

        static public void PleaseWaitJob(object data)
        {
            PleaseWaitMsg pleaseWaitMsg = (PleaseWaitMsg)data;
            PleaseWait.PleaseWaitDialog pleaseWaitDialog = new PleaseWaitDialog();
            pleaseWaitDialog.Display(pleaseWaitMsg);
        }

        static public EventWaitHandle PleaseWaitBegin(String title, String msg, String iconFileName)
        {
            EventWaitHandle pleaseWaitDialogEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "pleaseWaitDialogEvent");
            Thread thread = new Thread(new ParameterizedThreadStart(PleaseWaitJob));
            PleaseWaitMsg pleaseWaitMsg = new PleaseWaitMsg(title, msg, iconFileName);
            thread.Start(pleaseWaitMsg);
            Thread.Sleep(500);
            return pleaseWaitDialogEvent;
        }

        static public void PleaseWaitEnd()
        {
            EventWaitHandle pleaseWaitDialogEvent = EventWaitHandle.OpenExisting("pleaseWaitDialogEvent");
            pleaseWaitDialogEvent.Set();
        }
    }
}
