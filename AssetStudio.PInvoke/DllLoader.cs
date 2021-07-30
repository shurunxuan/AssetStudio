using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace AssetStudio.PInvoke
{
    public static class DllLoader
    {

        public static void PreloadDll(string dllName)
        {
            var dllDir = GetDirectedDllDirectory();

            // Not using OperatingSystem.Platform.
            // See: https://www.mono-project.com/docs/faq/technical/#how-to-detect-the-execution-platform
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Win32.LoadDll(dllDir, dllName);
            }
            else
            {
                Posix.LoadDll(dllDir, dllName);
            }
        }

        private static string GetDirectedDllDirectory()
        {
            var localPath = Process.GetCurrentProcess().MainModule.FileName;
            var localDir = Path.GetDirectoryName(localPath);

            return localDir;
        }

        private static class Win32
        {

            internal static void LoadDll(string dllDir, string dllName)
            {
                var dllFileName = $"{dllName}.dll";
                var directedDllPath = Path.Combine(dllDir, dllFileName);

                // Specify SEARCH_DLL_LOAD_DIR to load dependent libraries located in the same platform-specific directory.
                var hLibrary = LoadLibraryEx(directedDllPath, IntPtr.Zero, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);

                if (hLibrary == IntPtr.Zero)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    var exception = new Win32Exception(errorCode);

                    throw new DllNotFoundException(exception.Message, exception);
                }
            }

            // HMODULE LoadLibraryExA(LPCSTR lpLibFileName, HANDLE hFile, DWORD dwFlags);
            // HMODULE LoadLibraryExW(LPCWSTR lpLibFileName, HANDLE hFile, DWORD dwFlags);
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr LoadLibraryEx(string lpLibFileName, IntPtr hFile, uint dwFlags);

            private const uint LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x1000;
            private const uint LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x100;

        }

        private static class Posix
        {
            delegate IntPtr DLOpenType(string fileName, int flags);
            delegate IntPtr DLError();
            internal static void LoadDll(string dllDir, string dllName)
            {
                string dllExtension;
                DLOpenType dlopen;
                DLError dlerror;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    dllExtension = ".so";
                    dlopen = Linux.DlOpen;
                    dlerror = Linux.DlError;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    dllExtension = ".dylib";
                    dlopen = OSX.DlOpen;
                    dlerror = OSX.DlError;
                }
                else
                {
                    throw new NotSupportedException();
                }

                var dllFileName = $"lib{dllName}{dllExtension}";
                var directedDllPath = Path.Combine(dllDir, dllFileName);

                int ldFlags = 0x2 /*RTLD_NOW*/ | (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? 0x8 : 0x100) /*RTLD_GLOBAL*/;
                var hLibrary = dlopen(directedDllPath, ldFlags);

                if (hLibrary == IntPtr.Zero)
                {
                    var pErrStr = dlerror();
                    // `PtrToStringAnsi` always uses the specific constructor of `String` (see dotnet/core#2325),
                    // which in turn interprets the byte sequence with system default codepage. On OSX and Linux
                    // the codepage is UTF-8 so the error message should be handled correctly.
                    var errorMessage = Marshal.PtrToStringAnsi(pErrStr);

                    throw new DllNotFoundException(errorMessage);
                }
            }
        }

        private static class Linux
        {
            // OSX and most Linux OS use LP64 so `int` is still 32-bit even on 64-bit platforms.
            // void *dlopen(const char *filename, int flag);
            [DllImport("libdl", EntryPoint = "dlopen")]
            public static extern IntPtr DlOpen([MarshalAs(UnmanagedType.LPStr)] string fileName, int flags);

            // char *dlerror(void);
            [DllImport("libdl", EntryPoint = "dlerror")]
            public static extern IntPtr DlError();
        }

        private static class OSX
        {
            // OSX and most Linux OS use LP64 so `int` is still 32-bit even on 64-bit platforms.
            // void *dlopen(const char *filename, int flag);
            [DllImport("System.B", EntryPoint = "dlopen")]
            public static extern IntPtr DlOpen([MarshalAs(UnmanagedType.LPStr)] string fileName, int flags);

            // char *dlerror(void);
            [DllImport("System.B", EntryPoint = "dlerror")]
            public static extern IntPtr DlError();
        }

    }
}
