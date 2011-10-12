using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace PackageEditor
{
    class Win32Function
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

    class Win32imports
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
}
