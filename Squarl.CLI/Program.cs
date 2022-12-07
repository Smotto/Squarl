using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Memory.Win64;
using Squarl.Engine;

namespace Squarl.CLI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter Process Name: ");
            string? processName = Console.ReadLine();
            Console.WriteLine("Getting Process...");

            Process[] allRunningProcesses = Process.GetProcesses();

            foreach (Process process in allRunningProcesses)
            {
                Console.WriteLine("Process: {0} ID: {1}", process.ProcessName, process.Id);
            }

            Console.WriteLine("Getting Process Complete!\n");

            Process? p = Process.GetProcessesByName(processName).FirstOrDefault();
            List<Module> collectedModules = ScanEngine.CollectModules(p!);
            MemoryHelper64 helper64 = new MemoryHelper64(p!);
            Console.WriteLine("Selected Process Base Address: " + helper64.GetBaseAddress(0) + "\n");

            foreach (var module in collectedModules)
            {
                if (module.ModuleName.StartsWith(processName!))
                {
                    int byteSize = 2;
                    byte[] bytes = helper64.ReadMemoryBytes((ulong)module.BaseAddress, byteSize);
                    
                    Console.WriteLine("Reading Memory Bytes Of Size: " + byteSize);
                    foreach (var bite in bytes)
                    {
                        Console.WriteLine(bite);
                    }
                    Console.WriteLine(module.ModuleName);
                    break;
                }
            }
        }

        public static class VirtualQueryStuff
        {
            public struct MEMORY_BASIC_INFORMATION
            {
                public IntPtr BaseAddress;
                public IntPtr AllocationBase;
                public AllocationProtectEnum AllocationProtect;
                public IntPtr RegionSize;
                public StateEnum State;
                public AllocationProtectEnum Protect;
                public TypeEnum Type;
            }
            public enum AllocationProtectEnum : uint
            {
                PAGE_EXECUTE = 0x00000010,
                PAGE_EXECUTE_READ = 0x00000020,
                PAGE_EXECUTE_READWRITE = 0x00000040,
                PAGE_EXECUTE_WRITECOPY = 0x00000080,
                PAGE_NOACCESS = 0x00000001,
                PAGE_READONLY = 0x00000002,
                PAGE_READWRITE = 0x00000004,
                PAGE_WRITECOPY = 0x00000008,
                PAGE_GUARD = 0x00000100,
                PAGE_NOCACHE = 0x00000200,
                PAGE_WRITECOMBINE = 0x00000400
            }

            public enum StateEnum : uint
            {
                MEM_COMMIT = 0x1000,
                MEM_FREE = 0x10000,
                MEM_RESERVE = 0x2000
            }

            public enum TypeEnum : uint
            {
                MEM_IMAGE = 0x1000000,
                MEM_MAPPED = 0x40000,
                MEM_PRIVATE = 0x20000
            }
            
            [DllImport("kernel32.dll")]
            private static extern int VirtualQuery (
                ref UIntPtr lpAddress,
                ref MEMORY_BASIC_INFORMATION lpBuffer,
                int dwLength
            );
        }
        
        
    }
}