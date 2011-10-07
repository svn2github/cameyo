/*
 * 
 *  Source from: Mouse Extender http://me.codeplex.com/
 *
 *  Modified for use in Cameyo.
 */
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MouseExtender.Logic.Helpers
{
  public class IconHelper
  {
    public static Icon getIconFromFile(String fileName)
    {
      /* The Icon.ExtractAssociatedIcon function does not support network drives. */
      Icon ico;
      if (fileName.StartsWith(@"\\"))
      {
        Win32Helper.SHFILEINFO shinfo = new Win32Helper.SHFILEINFO();
        IntPtr hIcon = Win32Helper.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32Helper.SHGFI_ICON | Win32Helper.SHGFI_LARGEICON);
        ico = Icon.FromHandle(shinfo.hIcon);
      }
      else
      {
        ico = Icon.ExtractAssociatedIcon(fileName);
      }
      return ico;
    }
  }
}
