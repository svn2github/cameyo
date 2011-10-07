/*
 * 
 *  Source from: Mouse Extender http://me.codeplex.com/
 * 
 *  Modified for use in Cameyo.
 */
using System;
using System.Runtime.InteropServices;

namespace MouseExtender.Logic.Helpers
{
  /// <summary>
  /// This class holds all Win32 operations (Pinvoke, Constants, etc)
  /// </summary>
  public static class Win32Helper
  {
    #region Structures
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
      public IntPtr hIcon;
      public IntPtr iIcon;
      public uint dwAttributes;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
      public string szDisplayName;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
      public string szTypeName;
    }
    #endregion

    #region DllImports
    [DllImport("shell32.dll", CharSet = CharSet.Ansi)]
    public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlag);
    #endregion

    #region Constants

    public const uint SHGFI_ICON = 0x100;
    public const uint SHGFI_LARGEICON = 0x0;
    public const uint SHGFI_SMALLICON = 0x1;
    //see http://www.codeguru.com/cpp/com-tech/shell/article.php/c4511
    public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
    public const uint SHGFI_TYPENAME = 0x000000400;
    #endregion
  }
}