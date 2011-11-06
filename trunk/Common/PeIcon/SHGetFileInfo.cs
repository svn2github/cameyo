using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;

namespace PeFile
{
    public class PeIcon
    {
        static public System.Drawing.Image LoadPeIcon(string peFile)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                PeFile.PeIcon.SavePeIcon(peFile, stream, false);
                return System.Drawing.Image.FromStream(stream);
            }
            catch
            {
                return null;
            }
        }

        static public bool SavePeIcon(String peFile, String targetFile, bool jumboSize)
        {
            try
            {
                System.Windows.Media.Imaging.BitmapSource icosrc = icon_of_path_large(peFile, jumboSize, true);
                FileStream fileStream = new FileStream(targetFile, FileMode.Create);
                System.Windows.Media.Imaging.PngBitmapEncoder encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
                encoder.Interlace = System.Windows.Media.Imaging.PngInterlaceOption.On;
                encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(icosrc));
                encoder.Save(fileStream);
                return true;
            }
            catch { }
            return false;
        }

        static public bool SavePeIcon(String peFile, Stream target, bool jumboSize)
        {
            try
            {
                System.Windows.Media.Imaging.BitmapSource icosrc = icon_of_path_large(peFile, jumboSize, true);
                System.Windows.Media.Imaging.PngBitmapEncoder encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
                encoder.Interlace = System.Windows.Media.Imaging.PngInterlaceOption.On;
                encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(icosrc));
                encoder.Save(target);
                return true;
            }
            catch { }
            return false;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct ICONINFO
        {
            public bool fIcon;         // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 
            // an icon; FALSE specifies a cursor. 
            public Int32 xHotspot;     // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 
            // spot is always in the center of the icon, and this member is ignored.
            public Int32 yHotspot;     // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 
            // spot is always in the center of the icon, and this member is ignored. 
            public IntPtr hbmMask;     // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 
            // this bitmask is formatted so that the upper half is the icon AND bitmask and the lower half is 
            // the icon XOR bitmask. Under this condition, the height should be an even multiple of two. If 
            // this structure defines a color icon, this mask only defines the AND bitmask of the icon. 
            public IntPtr hbmColor;    // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
            // structure defines a black and white icon. The AND bitmask of hbmMask is applied with the SRCAND 
            // flag to the destination; subsequently, the color bitmap is applied (using XOR) to the 
            // destination by using the SRCINVERT flag. 
        }
        [DllImport("user32.dll")]
        static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("gdi32.dll")]
        static extern int GetObject(IntPtr hgdiobj, int cbBuffer, IntPtr lpvObject);

        public enum CSIDL
        {
            CSIDL_DESKTOP = 0x0000,    // <desktop>
            CSIDL_INTERNET = 0x0001,    // Internet Explorer (icon on desktop)
            CSIDL_PROGRAMS = 0x0002,    // Start Menu\Programs
            CSIDL_CONTROLS = 0x0003,    // My Computer\Control Panel
            CSIDL_PRINTERS = 0x0004,    // My Computer\Printers
            CSIDL_PERSONAL = 0x0005,    // My Documents
            CSIDL_FAVORITES = 0x0006,    // <user name>\Favorites
            CSIDL_STARTUP = 0x0007,    // Start Menu\Programs\Startup
            CSIDL_RECENT = 0x0008,    // <user name>\Recent
            CSIDL_SENDTO = 0x0009,    // <user name>\SendTo
            CSIDL_BITBUCKET = 0x000a,    // <desktop>\Recycle Bin
            CSIDL_STARTMENU = 0x000b,    // <user name>\Start Menu
            CSIDL_MYDOCUMENTS = 0x000c,    // logical "My Documents" desktop icon
            CSIDL_MYMUSIC = 0x000d,    // "My Music" folder
            CSIDL_MYVIDEO = 0x000e,    // "My Videos" folder
            CSIDL_DESKTOPDIRECTORY = 0x0010,    // <user name>\Desktop
            CSIDL_DRIVES = 0x0011,    // My Computer
            CSIDL_NETWORK = 0x0012,    // Network Neighborhood (My Network Places)
            CSIDL_NETHOOD = 0x0013,    // <user name>\nethood
            CSIDL_FONTS = 0x0014,    // windows\fonts
            CSIDL_TEMPLATES = 0x0015,
            CSIDL_COMMON_STARTMENU = 0x0016,    // All Users\Start Menu
            CSIDL_COMMON_PROGRAMS = 0X0017,    // All Users\Start Menu\Programs
            CSIDL_COMMON_STARTUP = 0x0018,    // All Users\Startup
            CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,    // All Users\Desktop
            CSIDL_APPDATA = 0x001a,    // <user name>\Application Data
            CSIDL_PRINTHOOD = 0x001b,    // <user name>\PrintHood

            CSIDL_LOCAL_APPDATA = 0x001c,    // <user name>\Local Settings\Applicaiton Data (non roaming)

            CSIDL_ALTSTARTUP = 0x001d,    // non localized startup
            CSIDL_COMMON_ALTSTARTUP = 0x001e,    // non localized common startup
            CSIDL_COMMON_FAVORITES = 0x001f,

            CSIDL_INTERNET_CACHE = 0x0020,
            CSIDL_COOKIES = 0x0021,
            CSIDL_HISTORY = 0x0022,
            CSIDL_COMMON_APPDATA = 0x0023,    // All Users\Application Data
            CSIDL_WINDOWS = 0x0024,    // GetWindowsDirectory()
            CSIDL_SYSTEM = 0x0025,    // GetSystemDirectory()
            CSIDL_PROGRAM_FILES = 0x0026,    // C:\Program Files
            CSIDL_MYPICTURES = 0x0027,    // C:\Program Files\My Pictures

            CSIDL_PROFILE = 0x0028,    // USERPROFILE
            CSIDL_SYSTEMX86 = 0x0029,    // x86 system directory on RISC
            CSIDL_PROGRAM_FILESX86 = 0x002a,    // x86 C:\Program Files on RISC

            CSIDL_PROGRAM_FILES_COMMON = 0x002b,    // C:\Program Files\Common

            CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c,    // x86 Program Files\Common on RISC
            CSIDL_COMMON_TEMPLATES = 0x002d,    // All Users\Templates

            CSIDL_COMMON_DOCUMENTS = 0x002e,    // All Users\Documents
            CSIDL_COMMON_ADMINTOOLS = 0x002f,    // All Users\Start Menu\Programs\Administrative Tools
            CSIDL_ADMINTOOLS = 0x0030,    // <user name>\Start Menu\Programs\Administrative Tools

            CSIDL_CONNECTIONS = 0x0031,    // Network and Dial-up Connections
            CSIDL_COMMON_MUSIC = 0x0035,    // All Users\My Music
            CSIDL_COMMON_PICTURES = 0x0036,    // All Users\My Pictures
            CSIDL_COMMON_VIDEO = 0x0037,    // All Users\My Video

            CSIDL_CDBURN_AREA = 0x003b    // USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAP
        {
            public Int32 bmType;
            public Int32 bmWidth;
            public Int32 bmHeight;
            public Int32 bmWidthBytes;
            public Int16 bmPlanes;
            public Int16 bmBitsPixel;
            public IntPtr bmBits;
        }

        // Constants that we need in the function call

        private const int SHGFI_ICON = 0x100;

        private const int SHGFI_SMALLICON = 0x1;

        private const int SHGFI_LARGEICON = 0x0;

        private const int SHIL_JUMBO = 0x4;
        private const int SHIL_EXTRALARGE = 0x2;

        // This structure will contain information about the file

        public struct SHFILEINFO
        {

            // Handle to the icon representing the file

            public IntPtr hIcon;

            // Index of the icon within the image list

            public int iIcon;

            // Various attributes of the file

            public uint dwAttributes;

            // Path to the file

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]

            public string szDisplayName;

            // File type

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]

            public string szTypeName;

        };

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern Boolean CloseHandle(IntPtr handle);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            private int _Left;
            private int _Top;
            private int _Right;
            private int _Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        private struct IMAGELISTDRAWPARAMS
        {
            public int cbSize;
            public IntPtr himl;
            public int i;
            public IntPtr hdcDst;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int xBitmap;        // x offest from the upperleft of bitmap
            public int yBitmap;        // y offset from the upperleft of bitmap
            public int rgbBk;
            public int rgbFg;
            public int fStyle;
            public int dwRop;
            public int fState;
            public int Frame;
            public int crEffect;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IMAGEINFO
        {
            public IntPtr hbmImage;
            public IntPtr hbmMask;
            public int Unused1;
            public int Unused2;
            public RECT rcImage;
        }

        #region Private ImageList COM Interop (XP)
        [ComImportAttribute()]
        [GuidAttribute("46EB5926-582E-4017-9FDF-E8998DAA0950")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        //helpstring("Image List"),
        interface IImageList
        {
            [PreserveSig]
            int Add(
                IntPtr hbmImage,
                IntPtr hbmMask,
                ref int pi);

            [PreserveSig]
            int ReplaceIcon(
                int i,
                IntPtr hicon,
                ref int pi);

            [PreserveSig]
            int SetOverlayImage(
                int iImage,
                int iOverlay);

            [PreserveSig]
            int Replace(
                int i,
                IntPtr hbmImage,
                IntPtr hbmMask);

            [PreserveSig]
            int AddMasked(
                IntPtr hbmImage,
                int crMask,
                ref int pi);

            [PreserveSig]
            int Draw(
                ref IMAGELISTDRAWPARAMS pimldp);

            [PreserveSig]
            int Remove(
            int i);

            [PreserveSig]
            int GetIcon(
                int i,
                int flags,
                ref IntPtr picon);

            [PreserveSig]
            int GetImageInfo(
                int i,
                ref IMAGEINFO pImageInfo);

            [PreserveSig]
            int Copy(
                int iDst,
                IImageList punkSrc,
                int iSrc,
                int uFlags);

            [PreserveSig]
            int Merge(
                int i1,
                IImageList punk2,
                int i2,
                int dx,
                int dy,
                ref Guid riid,
                ref IntPtr ppv);

            [PreserveSig]
            int Clone(
                ref Guid riid,
                ref IntPtr ppv);

            [PreserveSig]
            int GetImageRect(
                int i,
                ref RECT prc);

            [PreserveSig]
            int GetIconSize(
                ref int cx,
                ref int cy);

            [PreserveSig]
            int SetIconSize(
                int cx,
                int cy);

            [PreserveSig]
            int GetImageCount(
            ref int pi);

            [PreserveSig]
            int SetImageCount(
                int uNewCount);

            [PreserveSig]
            int SetBkColor(
                int clrBk,
                ref int pclr);

            [PreserveSig]
            int GetBkColor(
                ref int pclr);

            [PreserveSig]
            int BeginDrag(
                int iTrack,
                int dxHotspot,
                int dyHotspot);

            [PreserveSig]
            int EndDrag();

            [PreserveSig]
            int DragEnter(
                IntPtr hwndLock,
                int x,
                int y);

            [PreserveSig]
            int DragLeave(
                IntPtr hwndLock);

            [PreserveSig]
            int DragMove(
                int x,
                int y);

            [PreserveSig]
            int SetDragCursorImage(
                ref IImageList punk,
                int iDrag,
                int dxHotspot,
                int dyHotspot);

            [PreserveSig]
            int DragShowNolock(
                int fShow);

            [PreserveSig]
            int GetDragImage(
                ref POINT ppt,
                ref POINT pptHotspot,
                ref Guid riid,
                ref IntPtr ppv);

            [PreserveSig]
            int GetItemFlags(
                int i,
                ref int dwFlags);

            [PreserveSig]
            int GetOverlayImage(
                int iOverlay,
                ref int piIndex);
        };
        #endregion

        ///
        /// SHGetImageList is not exported correctly in XP.  See KB316931
        /// http://support.microsoft.com/default.aspx?scid=kb;EN-US;Q316931
        /// Apparently (and hopefully) ordinal 727 isn't going to change.
        ///
        [DllImport("shell32.dll", EntryPoint = "#727")]
        private extern static int SHGetImageList(
            int iImageList,
            ref Guid riid,
            out IImageList ppv
            );

        // The signature of SHGetFileInfo (located in Shell32.dll)
        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);

        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(IntPtr pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);

        [DllImport("shell32.dll", SetLastError = true)]
        static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, Int32 nFolder,
                 ref IntPtr ppidl);

        [DllImport("user32")]
        public static extern int DestroyIcon(IntPtr hIcon);

        public struct pair
        {
            public System.Drawing.Icon icon { get; set; }
            public IntPtr iconHandleToDestroy { set; get; }

        }

        public static int DestroyIcon2(IntPtr hIcon)
        {
            return DestroyIcon(hIcon);
        }

        private static BitmapSource bitmap_source_of_icon(System.Drawing.Icon ic)
        {
            var ic2 = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(ic.Handle,
                                                    System.Windows.Int32Rect.Empty,
                                                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            ic2.Freeze();
            return ((BitmapSource)ic2);
        }

        public static BitmapSource SystemIcon(bool small, CSIDL csidl)
        {

            IntPtr pidlTrash = IntPtr.Zero;
            int hr = SHGetSpecialFolderLocation(IntPtr.Zero, (int)csidl, ref pidlTrash);
            Debug.Assert(hr == 0);

            SHFILEINFO shinfo = new SHFILEINFO();

            uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

            // Get a handle to the large icon
            uint flags;
            uint SHGFI_PIDL = 0x000000008;
            if (!small)
            {
                flags = SHGFI_PIDL | SHGFI_ICON | SHGFI_LARGEICON | SHGFI_USEFILEATTRIBUTES;
            }
            else
            {
                flags = SHGFI_PIDL | SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES;
            }

            var res = SHGetFileInfo(pidlTrash, 0, ref shinfo, Marshal.SizeOf(shinfo), flags);
            Debug.Assert(res != 0);

            var myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
            Marshal.FreeCoTaskMem(pidlTrash);
            var bs = bitmap_source_of_icon(myIcon);
            myIcon.Dispose();
            bs.Freeze(); // importantissimo se no fa memory leak
            DestroyIcon(shinfo.hIcon);
            CloseHandle(shinfo.hIcon);
            return bs;

        }

        public static BitmapSource icon_of_path(string FileName, bool small, bool checkDisk, bool addOverlay)
        {
            SHFILEINFO shinfo = new SHFILEINFO();

            uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
            uint SHGFI_LINKOVERLAY = 0x000008000;

            uint flags;
            if (small)
            {
                flags = SHGFI_ICON | SHGFI_SMALLICON;
            }
            else
            {
                flags = SHGFI_ICON | SHGFI_LARGEICON;
            }
            if (!checkDisk)
                flags |= SHGFI_USEFILEATTRIBUTES;
            if (addOverlay)
                flags |= SHGFI_LINKOVERLAY;

            var res = SHGetFileInfo(FileName, 0, ref shinfo, Marshal.SizeOf(shinfo), flags);
            if (res == 0)
            {
                throw (new System.IO.FileNotFoundException());
            }

            var myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);

            var bs = bitmap_source_of_icon(myIcon);
            myIcon.Dispose();
            bs.Freeze(); // importantissimo se no fa memory leak
            DestroyIcon(shinfo.hIcon);
            CloseHandle(shinfo.hIcon);
            return bs;

        }

        public static BitmapSource icon_of_path_large(string FileName, bool jumbo, bool checkDisk)
        {

            SHFILEINFO shinfo = new SHFILEINFO();

            uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
            uint SHGFI_SYSICONINDEX = 0x4000;

            int FILE_ATTRIBUTE_NORMAL = 0x80;

            uint flags;
            flags = SHGFI_SYSICONINDEX;

            if (!checkDisk)  // This does not seem to work. If I try it, a folder icon is always returned.
                flags |= SHGFI_USEFILEATTRIBUTES;

            var res = SHGetFileInfo(FileName, FILE_ATTRIBUTE_NORMAL, ref shinfo, Marshal.SizeOf(shinfo), flags);
            if (res == 0)
            {
                throw (new System.IO.FileNotFoundException());
            }
            var iconIndex = shinfo.iIcon;

            // Get the System IImageList object from the Shell:
            Guid iidImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
            //Guid iidImageListSmaller = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");

            IImageList iml;
            int size = jumbo ? SHIL_JUMBO : SHIL_EXTRALARGE;
            var hres = SHGetImageList(size, ref iidImageList, out iml); // writes iml
            //if (hres == 0)
            //{
            //    throw (new System.Exception("Error SHGetImageList"));
            //}

            IntPtr hIcon = IntPtr.Zero;
            int ILD_TRANSPARENT = 1;
            hres = iml.GetIcon(iconIndex, ILD_TRANSPARENT, ref hIcon);
            //if (hres == 0)
            //{
            //    throw (new System.Exception("Error iml.GetIcon"));
            //}
            
            /*ICONINFO iconInfo;
            System.Drawing.Bitmap bitmap;
            if (GetIconInfo(hIcon, out iconInfo))
            {
                bitmap = System.Drawing.Bitmap.FromHbitmap(iconInfo.hbmColor);
                //BITMAP[] bmp = new BITMAP[] { new BITMAP() };
                //GetObject(iconInfo.hbmColor, Marshal.SizeOf(bmp), Marshal.UnsafeAddrOfPinnedArrayElement(bmp, 0));
            }*/

            System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(hIcon);
            var bs = bitmap_source_of_icon(myIcon);

            myIcon.Dispose();
            bs.Freeze(); // very important to avoid memory leak
            DestroyIcon(hIcon);
            //CloseHandle(hIcon);

            return bs;

        }
    }
}
