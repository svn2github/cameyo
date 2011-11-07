using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace VirtPackageAPI
{
    public struct VirtFsNode
    {
        public String FileName;
        public VIRT_FILE_FLAGS FileFlags;
        public UInt64 CreationTime;
        public UInt64 LastAccessTime;
        public UInt64 LastWriteTime;
        public UInt64 ChangeTime;
        public UInt64 EndOfFile;
        public UInt32 FileAttributes;
        public Object ClientData;     // Arbitrary client data; you can use this however you like
    };

    [Flags]
    public enum VIRT_FILE_FLAGS
    {
        NO_FLAGS = 0x0,
        ISFILE = 0x0001,        // File or directory?
        DELETED = 0x0002,       // Deleted by virtual app (NOT_FOUND)
        DEPLOYED = 0x0008,      // Set upon first file opening
        DISCONNECTED = 0x0010,  // Set when on-disk file is modified from DB
        PKG_FILE = 0x0020,      // File/dir is part of the original package (as opposed to files newly-added to sandbox during package use)
        ALL_FLAGS = ISFILE | DELETED | DEPLOYED | DISCONNECTED | PKG_FILE
    }

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

        private const int MAX_PATH = 260;
        public const int MAX_APPID_LENGTH = 128;

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            [MarshalAs(UnmanagedType.U2)]
            public short Year;
            [MarshalAs(UnmanagedType.U2)]
            public short Month;
            [MarshalAs(UnmanagedType.U2)]
            public short DayOfWeek;
            [MarshalAs(UnmanagedType.U2)]
            public short Day;
            [MarshalAs(UnmanagedType.U2)]
            public short Hour;
            [MarshalAs(UnmanagedType.U2)]
            public short Minute;
            [MarshalAs(UnmanagedType.U2)]
            public short Second;
            [MarshalAs(UnmanagedType.U2)]
            public short Milliseconds;
        }

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

        // VirtFs imports
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
        private extern static int VirtFsAddEx(
            IntPtr hPkg,
            String SrcFileName,
            String DestFileName,
            bool bVariablizeName,
            UInt32 FileFlags);

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

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static int VirtFsSetFileStreaming(
            IntPtr hPkg,
            String FileName);

        // VirtReg imports
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

        // Sandbox imports
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

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static int QuickReadIni(
            String PackageExeFile,
            StringBuilder IniBuf,
            UInt32 IniBufLen);

        [StructLayout(LayoutKind.Sequential)]
        public struct VIRT_PROCESS
        {
            UInt32 PID;
            UInt32 Flags;
        }

        // RunningApp imports
        [StructLayout(LayoutKind.Sequential)]
        private struct RUNNING_APP
        {
            public UInt32 Version;
            public UInt32 SerialId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_PATH * 4 * 2)]
            public char[] CarrierExeName;
            public UInt32 CarrierPID;
            public UInt32 StartTickTime;
            public UInt32 SyncStreamingDuration;
            public UInt32 TotalPIDs;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2048)]
            public VIRT_PROCESS[] Processes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_APPID_LENGTH * 2)]
            public char[] AppID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_APPID_LENGTH * 2)]
            public char[] FriendlyName;
        }
        public class RunningApp
        {
            public String AppID;
            public List<VIRT_PROCESS> Processes;
            public String CarrierExeName;
            public UInt32 SerialId;
            public UInt32 CarrierPID;
            public UInt32 StartTickTime;
            public String FriendlyName;
        }
        private delegate bool RUNNINGAPP_ENUM_CALLBACK(
            ref Object Data,
            ref RUNNING_APP RunningApp);
        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int RunningAppEnum(
            RUNNINGAPP_ENUM_CALLBACK Callback,
            ref Object Data);

        // DeployedApp imports
        private delegate bool DEPLOYEDAPP_ENUM_CALLBACK(
            ref Object Data,
            [MarshalAs(UnmanagedType.LPWStr)] String AppID);
        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int DeployedAppEnum(
            DEPLOYEDAPP_ENUM_CALLBACK Callback,
            ref Object Data);

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int DeployedAppGetDir(
            String AppID,
            StringBuilder BaseDirName,
            UInt32 BaseDirNameLen);

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

        //
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
            virtFsNode.FileFlags = (VIRT_FILE_FLAGS)FileFlags;
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
          VIRT_FILE_FLAGS fileFlags = VIRT_FILE_FLAGS.ISFILE & VIRT_FILE_FLAGS.DEPLOYED & VIRT_FILE_FLAGS.PKG_FILE;
          return AddFileEx(SrcFileName, DestFileName, bVariablizeName, fileFlags);
        }

        public bool AddFileEx(
            String SrcFileName,
            String DestFileName,
            bool bVariablizeName,
            VIRT_FILE_FLAGS fileFlags)
        {
          APIRET Ret = (APIRET)VirtFsAddEx(hPkg, SrcFileName, DestFileName, bVariablizeName, (uint)fileFlags);
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

        public bool SetFileStreaming(
            String FileName)
        {
            APIRET Ret = (APIRET)VirtFsSetFileStreaming(hPkg, FileName);
            if (Ret == APIRET.SUCCESS)
                return true;
            else if (Ret == APIRET.NOT_FOUND)
                return false;
            else
                return false;
        }

        //
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

        //
        // Sandbox functions

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

        public void SetFileFlags(String Path, VIRT_FILE_FLAGS FileFlags)
        {
          APIRET apiRet = (APIRET)VirtFsSetFileFlags(hPkg, Path, (UInt32)FileFlags);
        }
        public VIRT_FILE_FLAGS GetFileFlags(String Path)
        {
            UInt32 FileFlags = 0;
            APIRET apiRet = (APIRET)VirtFsGetFileFlags(hPkg, Path, ref FileFlags);
            return (VIRT_FILE_FLAGS)FileFlags;
        }

        static String LPWStrToString(char[] lpwstr)
        {
            String res = "";
            for (int i = 0; i < lpwstr.Length; i += 2)
            {
                if (lpwstr[i] == 0)
                    break;
                res += lpwstr[i];
            }
            return res;
        }

        static List<UInt32> ArrayToList(UInt32[] array, UInt32 count)
        {
            List<UInt32> res = new List<UInt32>();
            for (int i = 0; i < count; i++)
                res.Add(array[i]);
            return res;
        }

        static List<VIRT_PROCESS> ArrayToList(VIRT_PROCESS[] array, UInt32 count)
        {
            List<VIRT_PROCESS> res = new List<VIRT_PROCESS>();
            for (int i = 0; i < count; i++)
                res.Add(array[i]);
            return res;
        }

        //
        // RunningApp functions
        static private bool EnumRunningAppsCallback(
            ref Object Data,
            ref RUNNING_APP RunningAppRaw)
        {
            RunningApp runningApp = new RunningApp() {
                AppID = LPWStrToString(RunningAppRaw.AppID),
                CarrierExeName = LPWStrToString(RunningAppRaw.CarrierExeName),
                FriendlyName = LPWStrToString(RunningAppRaw.FriendlyName),
                CarrierPID = RunningAppRaw.CarrierPID,
                StartTickTime = RunningAppRaw.StartTickTime,
                SerialId = RunningAppRaw.SerialId,
                Processes = ArrayToList(RunningAppRaw.Processes, RunningAppRaw.TotalPIDs)
            };
            ((List<RunningApp>)Data).Add(runningApp);
            return true;
        }

        static public List<RunningApp> RunningApps()
        {
            RUNNINGAPP_ENUM_CALLBACK Callback = new RUNNINGAPP_ENUM_CALLBACK(EnumRunningAppsCallback);
            List<RunningApp> list = new List<RunningApp>();
            Object data = list;
            if ((APIRET)RunningAppEnum(Callback, ref data) == APIRET.SUCCESS)
                return list;
            else
                return null;
        }

        static public RunningApp FindRunningApp(string appID)
        {
            List<RunningApp> list = RunningApps();
            if (list == null)
                return null;
            foreach (RunningApp app in list)
            {
                if (app.AppID == appID)
                    return app;
            }
            return null;
        }

        //
        // DeployedApp functions
        static private bool EnumDeployedAppsCallback(
            ref Object Data,
            [MarshalAs(UnmanagedType.LPWStr)] String AppID)
        {
            ((List<String>)Data).Add(AppID);
            return true;
        }

        static public List<String> DeployedAppIDs()
        {
            DEPLOYEDAPP_ENUM_CALLBACK Callback = new DEPLOYEDAPP_ENUM_CALLBACK(EnumDeployedAppsCallback);
            List<String> list = new List<String>();
            Object data = list;
            if ((APIRET)DeployedAppEnum(Callback, ref data) == APIRET.SUCCESS)
                return list;
            else
                return null;
        }

        static public List<DeployedApp> DeployedApps()
        {
            List<String> appIDs = DeployedAppIDs();
            List<DeployedApp> deployedApps = new List<DeployedApp>();
            foreach (String appID in appIDs)
            {
                deployedApps.Add(DeployedApp.FromAppID(appID));
            }
            return deployedApps;
        }

        static public String DeployedAppDir(String AppID)
        {
            StringBuilder sbValue = new StringBuilder(MAX_STRING);
            APIRET Ret = (APIRET)DeployedAppGetDir(AppID, sbValue, MAX_STRING);
            if (Ret == APIRET.SUCCESS)
                return sbValue.ToString();
            else
                return null;  // Error
        }

        //
        // Helper functions
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

        static public System.Collections.Hashtable ReadIniSettings(String IniFile)
        {
            try
            {
                String iniBuf = File.ReadAllText(IniFile, Encoding.Unicode);
                if (iniBuf.IndexOf("AppID") == -1)
                    iniBuf = File.ReadAllText(IniFile, Encoding.ASCII);  // Happens when modified with notepad
                String[] lines = iniBuf.Split('\r', '\n');
                System.Collections.Hashtable values = new System.Collections.Hashtable();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (String.IsNullOrEmpty(lines[i]))
                        continue;
                    try
                    {
                        int equal = lines[i].IndexOf('=');
                        if (equal != -1)
                            values.Add(lines[i].Substring(0, equal), lines[i].Substring(equal + 1));
                        else
                            values.Add(lines[i], "");
                    }
                    catch { }
                }
                return values;
            }
            catch
            {
                return null;
            }
        }
    }

    public class DeployedApp
    {
        public String AppID { get { return m_AppID; } }
        internal String m_AppID;
        public String BaseDirName { get { return m_BaseDirName; } }
        internal String m_BaseDirName;
        public String CarrierExeName { get { return m_CarrierExeName; } }
        internal String m_CarrierExeName;
        public long OccupiedSize { get { return GetOccupiedSize(); } }
        internal long m_OccupiedSize = -1;
        public String EngineVersion { get { return GetEngineVersion(); } }
        internal String m_EngineVersion;

        // Basic ini settings
        public String BuildUid { get { return (String)IniProperties["BuildUID"]; } }
        public String CloudPkgId { get { return (String)IniProperties["CloudPkgId"]; } }
        public String Streamer { get { return (String)IniProperties["Streamer"]; } }
        public String Publisher { get { return (String)IniProperties["Publisher"]; } }
        public String Version { get { return (String)IniProperties["Version"]; } }
        public String FriendlyName { get { return (String)IniProperties["FriendlyName"]; } }
        public String AutoLaunch { get { return (String)IniProperties["AutoLaunch"]; } }
        public String Shortcuts { get { return (String)IniProperties["Shortcuts"]; } }
        public String StopInheritance { get { return (String)IniProperties["StopInheritance"]; } }

        public System.Collections.Hashtable IniProperties { get { return m_IniProperties; } }
        internal System.Collections.Hashtable m_IniProperties;

        public DeployedApp(String appID, String baseDirName, String carrierExeName)
        {
            m_AppID = appID;
            m_BaseDirName = baseDirName;
            m_CarrierExeName = carrierExeName;
            m_IniProperties = VirtPackage.ReadIniSettings(Path.Combine(baseDirName, "VirtApp.ini"));
        }

        private static long DirSize(DirectoryInfo d) 
        {    
            long Size = 0;    
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis) 
            {      
                Size += fi.Length;    
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis) 
            {
                Size += DirSize(di);   
            }
            return(Size);  
        }

        private long GetOccupiedSize()
        {
            if (m_OccupiedSize != -1)
                return m_OccupiedSize;
            if (!Directory.Exists(m_BaseDirName))
            {
                m_OccupiedSize = 0;
                return m_OccupiedSize;
            }
            DirectoryInfo d = new DirectoryInfo(m_BaseDirName);
            m_OccupiedSize = DirSize(d);
            return m_OccupiedSize;
        }

        private String GetEngineVersion()
        {
            if (!String.IsNullOrEmpty(m_EngineVersion))
                return m_EngineVersion;
            String appVirtDll = m_BaseDirName + "\\AppVirtDll_" + m_AppID + ".dll";
            if (!File.Exists(appVirtDll))
                return "";
            System.Diagnostics.FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(appVirtDll);
            m_EngineVersion = fileVersionInfo.FileVersion.Replace(" ", "").Replace(",", ".");  // virtPkg.EngineVer is in the form: "1, 7, 534, 0"
            return m_EngineVersion;
        }

        static public DeployedApp FromAppID(String appID)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\VOS\\" + appID, false);
                if (key == null)
                    return null;
                String baseDirName = (String)key.GetValue("BaseDirName");
                String carrierExeName = (String)key.GetValue("CarrierExeName");
                return new DeployedApp(appID, baseDirName, carrierExeName);
            }
            catch
            {
                return null;
            }
        }

        static public bool RunningInfoDword(string appID, string item, ref int ret)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\VOS\\" + appID + "\\RunningInfo", false);
                if (key == null)
                    return false;
                ret = (int)key.GetValue(item);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
