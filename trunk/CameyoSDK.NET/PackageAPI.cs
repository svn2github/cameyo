using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace VirtPackageAPI
{
    public struct VirtFsNode
    {
        public String FileName;
        public UInt32 FileFlags;
        public UInt64 CreationTime;
        public UInt64 LastAccessTime;
        public UInt64 LastWriteTime;
        public UInt64 ChangeTime;
        public UInt64 EndOfFile;
        public UInt32 FileAttributes;
        public Object ClientData;     // Arbitrary client data; you can use this however you like
    };
    public class VirtPackage
    {
        public enum APIRET
        {
            SUCCESS = 0,
            FAILURE = 1,
            VIRTFILES_DB_ERROR = 2,
            VIRTFILES_ZIP_ERROR = 3,
            NOT_FOUND = 5,
            INVALID_PARAMETER = 6,
            FILE_CREATE_ERROR = 7,
            PE_RESOURCE_ERROR = 8,
            MEMORY_ERROR = 9,
            COMMIT_ERROR = 10,
            VIRTREG_DEPLOY_ERROR = 11,
            OUTPUT_ERROR = 12,
            INSUFFICIENT_BUFFER = 13,
        }
        public const uint VIRT_FILE_FLAGS_ISFILE = 0x0001; 	// File or directory?
        public const uint VIRT_FILE_FLAGS_DELETED = 0x0002; 	// Deleted by virtual app (NOT_FOUND)
        public const uint VIRT_FILE_FLAGS_DEPLOYED = 0x0008; 	// Set upon first file opening
        public const uint VIRT_FILE_FLAGS_DISCONNECTED = 0x0010; 	// Set when on-disk file is modified from DB

        public const int SANDBOXFLAGS_MERGE = 1;
        public const int SANDBOXFLAGS_WRITE_COPY = 2;
        public const int SANDBOXFLAGS_FULL_ISOLATION = 3;

        // UI isolation constants:
        public const int ISOLATIONMODE_CUSTOM = 0;
        public const int ISOLATIONMODE_ISOLATED = 1;
        public const int ISOLATIONMODE_FULL_ACCESS = 2;
        public const int ISOLATIONMODE_DATA = 3;

        private const String DLLNAME = "PackagerDll.dll";
        private const int MAX_STRING = 64 * 1024;

        private IntPtr hPkg;
        public bool opened;
        public String openedFile;

        //
        // DLL imports
        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int PackageOpen(
            String PackageExeFile,
            UInt32 Reserved,
            ref IntPtr hPkg);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int PackageCreate(
            String AppID,
            String AppVirtDll,
            String LoaderExe,
            ref IntPtr hPkg);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static void PackageClose(
            IntPtr hPkg);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int PackageSave(
            IntPtr hPkg,
            String OutFileName);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int PackageGetProperty(
            IntPtr hPkg,
            String Name,
            StringBuilder Value,
            UInt32 ValueLen);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int PackageSetProperty(
            IntPtr hPkg,
            String Name,
            String Value);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int PackageSetIconFile(
            IntPtr hPkg,
            String FileName);

        // VirtFs functions
        private delegate bool VIRTFS_ENUM_CALLBACK(
            ref Object Data,
            [MarshalAs(UnmanagedType.LPWStr)] String FileName,
            UInt32 FileFlags,
            UInt64 CreationTime,
            UInt64 LastAccessTime,
            UInt64 LastWriteTime,
            UInt64 ChangeTime,
            UInt64 EndOfFile,
            UInt32 FileAttributes);
        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtFsEnum(
            IntPtr hPkg,
            VIRTFS_ENUM_CALLBACK Callback,
            ref Object Data);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtFsAdd(
            IntPtr hPkg,
            String SrcFileName,
            String DestFileName,
            bool bVariablizeName);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtFsAddEmptyDir(
            IntPtr hPkg,
            String DirName,
            bool bVariablizeName);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtFsExtract(
            IntPtr hPkg,
            String FileName,
            String TargetDir);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtFsDelete(
            IntPtr hPkg,
            String FileName);

        // VirtReg functions
        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtRegGetWorkKey(
            IntPtr hPkg,
            StringBuilder WorkKey,
            UInt32 WorkKeyLen);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtRegGetWorkKeyEx(
            IntPtr hPkg,
            StringBuilder WorkKey,
            UInt32 WorkKeyLen,
            SafeWaitHandle hAbortEvent);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtRegSaveWorkKey(
            IntPtr hPkg);

        // Sandbox functions
        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int SandboxGetRegistryFlags(
            IntPtr hPkg,
            String Path,
            bool bVariablizeName,
            ref UInt32 SandboxFlags);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int SandboxGetFileFlags(
            IntPtr hPkg,
            String Path,
            bool bVariablizeName,
            ref UInt32 SandboxFlags);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int SandboxSetRegistryFlags(
            IntPtr hPkg,
            String Path,
            bool bVariablizeName,
            UInt32 SandboxFlags);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int SandboxSetFileFlags(
            IntPtr hPkg,
            String Path,
            bool bVariablizeName,
            UInt32 SandboxFlags);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtFsGetFileFlags(
            IntPtr hPkg,
            String Path,
            ref UInt32 FileFlags);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtFsSetFileFlags(
            IntPtr hPkg,
            String Path,
            UInt32 FileFlags);

        //
        // .NET wrapper
        public VirtPackage()
        {
            opened = false;
            openedFile = "";
            //private const String DLLNAME = "PackagerDll.dll";
        }

        ~VirtPackage()
        {
            Close();
        }

        public bool Close()
        {
            if (opened)
            {
                PackageClose(hPkg);
                opened = false;
                openedFile = "";
            }
            return true;
        }

        public bool Save(String FileName)
        {
            APIRET apiRet;
            return SaveEx(FileName, out apiRet);
        }

        public bool SaveEx(String FileName, out APIRET apiRet)
        {
            int Ret = PackageSave(hPkg, FileName);
            apiRet = (APIRET)Ret;
            if (apiRet == APIRET.SUCCESS)
                return true;
            else
                return false;
        }

        public bool Open(String PackageExeFile)
        {
            APIRET apiRet;
            return Open(PackageExeFile, out apiRet);
        }

        public bool Open(String PackageExeFile, out APIRET apiRet)
        {
            apiRet = (APIRET)PackageOpen(PackageExeFile, 0, ref hPkg);
            if (apiRet == APIRET.SUCCESS)
            {
                opened = true;
                openedFile = PackageExeFile;
                return true;
            }
            else
                return false;
        }

        public bool Create(String AppID, String AppVirtDll, String LoaderExe)
        {
            APIRET Ret = (APIRET)PackageCreate(AppID, AppVirtDll, LoaderExe, ref hPkg);
            if (Ret == APIRET.SUCCESS)
            {
                opened = true;
                // Note: openedFile remains empty
                return true;
            }
            else
                return false;
        }

        public bool GetProperty(String Name, ref String Value)
        {
            StringBuilder sbValue = new StringBuilder(MAX_STRING);
            APIRET Ret = (APIRET)PackageGetProperty(hPkg, Name, sbValue, MAX_STRING);
            if (Ret == APIRET.SUCCESS)
            {
                Value = sbValue.ToString();
                return true;
            }
            else if (Ret == APIRET.NOT_FOUND)
                return false;
            else
                return false;
        }

        public String GetProperty(String Name)
        {
            String Value = "";
            if (GetProperty(Name, ref Value))
                return (Value);
            else
                return ("");
        }

        public bool SetProperty(String Name, String Value)
        {
            APIRET Ret = (APIRET)PackageSetProperty(hPkg, Name, Value);
            if (Ret == APIRET.SUCCESS)
                return true;
            else if (Ret == APIRET.FILE_CREATE_ERROR)
                return false;
            else
                return false;
        }

        public bool SetIcon(String FileName)
        {
            APIRET Ret = (APIRET)PackageSetIconFile(hPkg, FileName);
            if (Ret == APIRET.SUCCESS)
                return true;
            else if (Ret == APIRET.NOT_FOUND)
                return false;
            else
                return false;
        }

        // VirtFs functions
        private bool EnumFilesCallback(
            ref Object Data,
            [MarshalAs(UnmanagedType.LPWStr)] String FileName,
            UInt32 FileFlags,
            UInt64 CreationTime,
            UInt64 LastAccessTime,
            UInt64 LastWriteTime,
            UInt64 ChangeTime,
            UInt64 EndOfFile,
            UInt32 FileAttributes)
        {
            VirtFsNode virtFsNode = new VirtFsNode();
            virtFsNode.FileName = FileName;
            virtFsNode.FileFlags = FileFlags;
            virtFsNode.CreationTime = CreationTime;
            virtFsNode.LastAccessTime = LastAccessTime;
            virtFsNode.LastWriteTime = LastWriteTime;
            virtFsNode.ChangeTime = ChangeTime;
            virtFsNode.EndOfFile = EndOfFile;
            virtFsNode.FileAttributes = FileAttributes;
            ((List<VirtFsNode>)Data).Add(virtFsNode);
            return true;
        }

        public bool EnumFiles(
            ref List<VirtFsNode> VirtFsNodes)
        {
            VIRTFS_ENUM_CALLBACK Callback = new VIRTFS_ENUM_CALLBACK(EnumFilesCallback);
            Object Data = VirtFsNodes;
            VirtFsEnum(hPkg, Callback, ref Data);
            return true;
        }

        public bool AddFile(
            String SrcFileName,
            String DestFileName,
            bool bVariablizeName)
        {
            APIRET Ret = (APIRET)VirtFsAdd(hPkg, SrcFileName, DestFileName, bVariablizeName);
            if (Ret == APIRET.SUCCESS)
                return true;
            else if (Ret == APIRET.VIRTFILES_DB_ERROR)
                return true;
            else if (Ret == APIRET.NOT_FOUND)
                return false;
            else
                return false;
        }

        public bool AddEmptyDir(
            String DirName,
            bool bVariablizeName)
        {
            APIRET Ret = (APIRET)VirtFsAddEmptyDir(hPkg, DirName, bVariablizeName);
            if (Ret == APIRET.SUCCESS)
                return true;
            else if (Ret == APIRET.VIRTFILES_DB_ERROR)
                return true;
            else
                return false;
        }

        public bool AddDir(
            String SrcFolderName,
            String DestFolderName,
            bool bVariablizeName)
        {
            if (!System.IO.Directory.Exists(SrcFolderName))
                return false;

            AddEmptyDir(DestFolderName, bVariablizeName);

            string[] files = System.IO.Directory.GetFiles(SrcFolderName);
            foreach (string file in files)
            {
                if (!AddFile(file, DestFolderName + "\\" + System.IO.Path.GetFileName(file), bVariablizeName))
                    return false;
            }

            string[] subDirs = System.IO.Directory.GetDirectories(SrcFolderName);
            foreach (string dir in subDirs)
            {
                if (!AddDir(dir, DestFolderName + "\\" + System.IO.Path.GetFileName(dir), bVariablizeName))
                    return false;
            }
            return true;
        }

        public bool ExtractFile(
            String FileName,
            String TargetDir)
        {
            APIRET Ret = (APIRET)VirtFsExtract(hPkg, FileName, TargetDir);
            if (Ret == APIRET.SUCCESS)
                return true;
            else if (Ret == APIRET.NOT_FOUND)
                return false;
            else if (Ret == APIRET.FILE_CREATE_ERROR)
                return false;
            else
                return false;
        }

        public bool DeleteFile(
            String FileName)
        {
            APIRET Ret = (APIRET)VirtFsDelete(hPkg, FileName);
            if (Ret == APIRET.SUCCESS)
                return true;
            else if (Ret == APIRET.NOT_FOUND)
                return false;
            else
                return false;
        }

        // VirtReg functions
        public RegistryKey GetRegWorkKeyEx(System.Threading.AutoResetEvent abortEvent)
        {
            StringBuilder sbWorkKey = new StringBuilder(MAX_STRING);

            SafeWaitHandle waitHandle;
            if (abortEvent != null)
                waitHandle = abortEvent.SafeWaitHandle;
            else
                waitHandle = new SafeWaitHandle(IntPtr.Zero, true); ;
            APIRET Ret = (APIRET)VirtRegGetWorkKeyEx(hPkg, sbWorkKey, MAX_STRING, waitHandle);
            if (Ret == APIRET.SUCCESS)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(sbWorkKey.ToString(), true);
                if (key == null)
                {
                    key = Registry.CurrentUser.CreateSubKey(sbWorkKey.ToString());
                }
                return (key);
            }
            else if (Ret == APIRET.INSUFFICIENT_BUFFER)
                return null;
            else
                return null;
        }

        public RegistryKey GetRegWorkKey()
        {
            return GetRegWorkKeyEx(null);
        }

        public bool SaveRegWorkKey()
        {
            APIRET Ret = (APIRET)VirtRegSaveWorkKey(hPkg);
            if (Ret == APIRET.SUCCESS)
                return true;
            else if (Ret == APIRET.INVALID_PARAMETER)
                return false;
            else
                return false;
        }

        // Sandbox Get functions
        public UInt32 GetRegistrySandbox(String Path, bool bVariablizeName)
        {
            UInt32 SandboxFlags = 0;
            SandboxGetRegistryFlags(hPkg, Path, bVariablizeName, ref SandboxFlags);
            return SandboxFlags;
        }
        public UInt32 GetRegistrySandbox(String Path) { return GetRegistrySandbox(Path, false); }   // Overload

        public UInt32 GetFileSandbox(String Path, bool bVariablizeName)
        {
            UInt32 SandboxFlags = 0;
            SandboxGetFileFlags(hPkg, Path, bVariablizeName, ref SandboxFlags);
            return SandboxFlags;
        }
        public UInt32 GetFileSandbox(String Path) { return GetFileSandbox(Path, false); }   // Overload

        // Sandbox Set functions
        public void SetRegistrySandbox(String Path, UInt32 SandboxFlags, bool bVariablizeName)
        {
            SandboxSetRegistryFlags(hPkg, Path, bVariablizeName, SandboxFlags);
        }
        public void SetRegistrySandbox(String Path, UInt32 SandboxFlags) { SetRegistrySandbox(Path, SandboxFlags, false); }   // Overload

        public void SetFileSandbox(String Path, UInt32 SandboxFlags, bool bVariablizeName)
        {
            SandboxSetFileFlags(hPkg, Path, bVariablizeName, SandboxFlags);
        }
        public void SetFileSandbox(String Path, UInt32 SandboxFlags) { SetFileSandbox(Path, SandboxFlags, false); }   // Overload

        public void SetFileFlags(String Path, UInt32 FileFlags)
        {
            APIRET apiRet = (APIRET)VirtFsSetFileFlags(hPkg, Path, FileFlags);
        }
        public UInt32 GetFileFlags(String Path)
        {
            UInt32 FileFlags = 0;
            APIRET apiRet = (APIRET)VirtFsGetFileFlags(hPkg, Path, ref FileFlags);
            return FileFlags;
        }

        static public String FriendlyShortcutName(String rawShortcutName)
        {
            String friendly = System.IO.Path.GetFileName(rawShortcutName);
            if (friendly.EndsWith(".lnk", StringComparison.InvariantCultureIgnoreCase))
                friendly = friendly.Substring(0, friendly.Length - 4);
            return (friendly);
        }

        public int GetIsolationMode()
        {
            // Isolation. Note: it is allowed to have no checkbox selected at all.
            if (GetFileSandbox("") == VirtPackage.SANDBOXFLAGS_WRITE_COPY &&
                GetRegistrySandbox("") == VirtPackage.SANDBOXFLAGS_WRITE_COPY &&
                GetFileSandbox("%Personal%") == VirtPackage.SANDBOXFLAGS_MERGE &&
                GetFileSandbox("%Desktop%") == VirtPackage.SANDBOXFLAGS_MERGE &&
                GetFileSandbox("UNC") == VirtPackage.SANDBOXFLAGS_MERGE)
                return ISOLATIONMODE_DATA;
            else if (GetFileSandbox("") == VirtPackage.SANDBOXFLAGS_WRITE_COPY &&
                GetRegistrySandbox("") == VirtPackage.SANDBOXFLAGS_WRITE_COPY)
                return ISOLATIONMODE_ISOLATED;
            else if (GetFileSandbox("") == VirtPackage.SANDBOXFLAGS_MERGE &&
                GetRegistrySandbox("") == VirtPackage.SANDBOXFLAGS_MERGE)
                return ISOLATIONMODE_FULL_ACCESS;
            else
                return ISOLATIONMODE_CUSTOM;
        }

        public void SetIsolationMode(int IsolationMode)
        {
            uint sandboxMode = 0;
            if (IsolationMode == ISOLATIONMODE_ISOLATED || IsolationMode == ISOLATIONMODE_DATA)
                sandboxMode = VirtPackage.SANDBOXFLAGS_WRITE_COPY;
            else if (IsolationMode == ISOLATIONMODE_FULL_ACCESS)
                sandboxMode = VirtPackage.SANDBOXFLAGS_MERGE;
            if (sandboxMode != 0)
            {
                SetFileSandbox("", sandboxMode);
                SetRegistrySandbox("", sandboxMode);
            }

            // Do / undo special folders newly / previously set by Data Isolation mode
            if (IsolationMode == ISOLATIONMODE_DATA)
            {
                SetProperty("DataMode", "TRUE");
                SetFileSandbox("%Personal%", VirtPackage.SANDBOXFLAGS_MERGE);
                SetFileSandbox("%Desktop%", VirtPackage.SANDBOXFLAGS_MERGE);
                SetFileSandbox("UNC", VirtPackage.SANDBOXFLAGS_MERGE);
            }
            else
            {
                if (GetProperty("DataMode") == "TRUE")     // Need to undo special dirs changed by Data Isolation mode (as opposed to set by user)
                {
                    SetProperty("DataMode", "FALSE");
                    SetFileSandbox("%Personal%", sandboxMode);
                    SetFileSandbox("%Desktop%", sandboxMode);
                    SetFileSandbox("UNC", sandboxMode);
                }
            }
        }

    }
}
