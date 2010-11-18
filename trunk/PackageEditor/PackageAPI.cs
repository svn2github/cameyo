﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

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
        private const int APIRET_SUCCESS				= 0;
        private const int APIRET_FAILURE                = 1;
        private const int APIRET_VIRTFILES_DB_ERROR		= 2;
        private const int APIRET_VIRTFILES_ZIP_ERROR	= 3;
        private const int APIRET_NOT_FOUND				= 5;
        private const int APIRET_INVALID_PARAMETER		= 6;
        private const int APIRET_FILE_CREATE_ERROR		= 7;
        private const int APIRET_PE_RESOURCE_ERROR		= 8;
        private const int APIRET_MEMORY_ERROR			= 9;
        private const int APIRET_COMMIT_ERROR			= 10;
        private const int APIRET_VIRTREG_DEPLOY_ERROR	= 11;
        private const int APIRET_OUTPUT_ERROR			= 12;
        private const int APIRET_INSUFFICIENT_BUFFER    = 13;

        public const int VIRT_FILE_FLAGS_ISFILE	        = 0x0001; 	// File or directory?
        public const int VIRT_FILE_FLAGS_DELETED	    = 0x0002; 	// Deleted by virtual app (NOT_FOUND)
        public const int VIRT_FILE_FLAGS_DEPLOYED	    = 0x0008; 	// Set upon first file opening
        public const int VIRT_FILE_FLAGS_DISCONNECTED	= 0x0010; 	// Set when on-disk file is modified from DB

        public const int SANDBOXFLAGS_MERGE				= 1;
        public const int SANDBOXFLAGS_WRITE_COPY		= 2;
        public const int SANDBOXFLAGS_FULL_ISOLATION	= 3;

        private const String DLLNAME                    = "PackagerDll.dll";
        private const int MAX_STRING                    = 64 * 1024;

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
            bool bVariablizeName );

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtFsAddEmptyDir(
            IntPtr hPkg,
            String DirName,
            bool bVariablizeName );

        [DllImport(DLLNAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private extern static int VirtFsExtract(
            IntPtr hPkg,
            String FileName,
            String TargetDir );

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

        //
        // .NET wrapper
        public VirtPackage()
        {
            opened = false;
            openedFile = "";
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
            int Ret = PackageSave(hPkg, FileName);
            if (Ret == APIRET_SUCCESS)
                return true;
            else
                return false;
        }

        public bool Open(String PackageExeFile)
        {
            int Ret = PackageOpen(PackageExeFile, 0, ref hPkg);
            if (Ret == APIRET_SUCCESS)
            {
                opened = true;
                openedFile = PackageExeFile;
                return true;
            }
            else
                return false;
        }

        public bool GetProperty(String Name, ref String Value)
        {
            StringBuilder sbValue = new StringBuilder(MAX_STRING);
            int Ret = PackageGetProperty(hPkg, Name, sbValue, MAX_STRING);
            if (Ret == APIRET_SUCCESS)
            {
                Value = sbValue.ToString();
                return true;
            }
            else if (Ret == APIRET_NOT_FOUND)
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
            int Ret = PackageSetProperty(hPkg, Name, Value);
            if (Ret == APIRET_SUCCESS)
                return true;
            else if (Ret == APIRET_FILE_CREATE_ERROR)
                return false;
            else
                return false;
        }

        public bool SetIcon(String FileName)
        {
            int Ret = PackageSetIconFile(hPkg, FileName);
            if (Ret == APIRET_SUCCESS)
                return true;
            else if (Ret == APIRET_NOT_FOUND)
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
            int Ret = VirtFsAdd(hPkg, SrcFileName, DestFileName, bVariablizeName);
            if (Ret == APIRET_SUCCESS)
                return true;
            else if (Ret == APIRET_VIRTFILES_DB_ERROR)
                return true;
            else if (Ret == APIRET_NOT_FOUND)
                return false;
            else
                return false;
        }

        public bool AddEmptyDir(
            String DirName,
            bool bVariablizeName)
        {
            int Ret = VirtFsAddEmptyDir(hPkg, DirName, bVariablizeName);
            if (Ret == APIRET_SUCCESS)
                return true;
            else if (Ret == APIRET_VIRTFILES_DB_ERROR)
                return true;
            else
                return false;
        }

        public bool ExtractFile(
            String FileName,
            String TargetDir)
        {
            int Ret = VirtFsExtract(hPkg, FileName, TargetDir);
            if (Ret == APIRET_SUCCESS)
                return true;
            else if (Ret == APIRET_NOT_FOUND)
                return false;
            else if (Ret == APIRET_FILE_CREATE_ERROR)
                return false;
            else
                return false;
        }

        public bool DeleteFile(
            String FileName)
        {
            int Ret = VirtFsDelete(hPkg, FileName);
            if (Ret == APIRET_SUCCESS)
                return true;
            else if (Ret == APIRET_NOT_FOUND)
                return false;
            else
                return false;
        }

        // VirtReg functions
        public RegistryKey GetRegWorkKey()
        {
            StringBuilder sbWorkKey = new StringBuilder(MAX_STRING);
            int Ret = VirtRegGetWorkKey(hPkg, sbWorkKey, MAX_STRING);
            if (Ret == APIRET_SUCCESS)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(sbWorkKey.ToString(), true);
                return (key);
            }
            else if (Ret == APIRET_INSUFFICIENT_BUFFER)
                return null;
            else
                return null;
        }

        public bool SaveRegWorkKey()
        {
            int Ret = VirtRegSaveWorkKey(hPkg);
            if (Ret == APIRET_SUCCESS)
                return true;
            else if (Ret == APIRET_INVALID_PARAMETER)
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

        public String FriendlyShortcutName(String rawShortcutName)
        {
            String friendly = System.IO.Path.GetFileName(rawShortcutName);
            if (friendly.EndsWith(".lnk", StringComparison.InvariantCultureIgnoreCase))
                friendly = friendly.Substring(0, friendly.Length - 4);
            return (friendly);
        }
    }
}
