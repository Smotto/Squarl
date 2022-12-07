using System.Runtime.InteropServices;
using System.Text;

namespace Squarl.Engine;

public static class Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ModuleInformation
    {
        public readonly IntPtr lpBaseOfDll;
        public readonly uint SizeOfImage;
        private readonly IntPtr EntryPoint;
    }

    internal enum ModuleFilter
    {
        ListModulesDefault = 0x0,
        ListModules32Bit = 0x01,
        ListModules64Bit = 0x02,
        ListModulesAll = 0x03,
    }

    [DllImport("psapi.dll")]
    public static extern bool EnumProcessModulesEx(IntPtr hProcess,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] [In] [Out] IntPtr[] lphModule, int cb,
        [MarshalAs(UnmanagedType.U4)] out int lpcbNeeded, uint dwFilterFlag);

    [DllImport("psapi.dll")]
    public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName,
        [In] [MarshalAs(UnmanagedType.U4)] uint nSize);

    [DllImport("psapi.dll", SetLastError = true)]
    public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out ModuleInformation lpmodinfo,
        uint cb);
}