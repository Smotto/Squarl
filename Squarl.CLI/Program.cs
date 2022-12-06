﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using Memory.Utils;
using Memory.Win64;

namespace Squarl.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Process Name: ");
            string? processName = Console.ReadLine();
            Console.WriteLine("Getting Process...");
            
            Process[] allRunningProcesses = Process.GetProcesses();
            
            foreach (Process process in allRunningProcesses)
            {
                Console.WriteLine("Process: {0} ID: {1}", process.ProcessName, process.Id);

            }
            Console.WriteLine("Getting Process Complete.");
            
            Process? p = Process.GetProcessesByName(processName).FirstOrDefault();
            List<Module> collectedModules = CollectModules(p!);
            
            foreach (var module in collectedModules)
            {
                if (module.ModuleName.StartsWith(processName!))
                {
                    ulong baseAddress = ((ulong)module.BaseAddress);
                    Console.WriteLine("Base Process Address: " + baseAddress);
                    Console.WriteLine("Enter offset memory address: ");
                    Console.WriteLine(UInt64.Parse("0x022ADEF8"));
                    ulong offset = UInt64.Parse("0x022ADEF8");
                    ulong finalAddress = baseAddress + offset;
                    Console.WriteLine("Reading from base + offset address...");
                    MemoryHelper64 helper = new MemoryHelper64(p!);
                    Console.WriteLine("Read Result: " + helper.ReadMemory<int>(finalAddress));
                    Console.WriteLine("Change value to: ");
                    int value = Console.Read();
                    Console.WriteLine(helper.WriteMemory(finalAddress, value));
                }
            }
            
        }

        public static List<Module> CollectModules(Process process)
        {
            List<Module> collectedModules = new List<Module>();

            IntPtr[] modulePointers = new IntPtr[0];
            int bytesNeeded = 0;
            
            // Determine number of modules
            if (!Native.EnumProcessModulesEx(process.Handle, modulePointers, 0, out bytesNeeded,
                    (uint)Native.ModuleFilter.ListModulesAll))
            {
                return collectedModules;
            }
            
            int totalNumberofModules = bytesNeeded / IntPtr.Size;
            modulePointers = new IntPtr[totalNumberofModules];

            // Collect modules from the process
            if (Native.EnumProcessModulesEx(process.Handle, modulePointers, bytesNeeded, out bytesNeeded,
                    (uint)Native.ModuleFilter.ListModulesAll))
            {
                for (int index = 0; index < totalNumberofModules; index++)
                {
                    StringBuilder moduleFilePath = new StringBuilder(1024);
                    Native.GetModuleFileNameEx(process.Handle, modulePointers[index], moduleFilePath,
                        (uint)(moduleFilePath.Capacity));

                    string moduleName = Path.GetFileName(moduleFilePath.ToString());
                    Native.ModuleInformation moduleInformation = new Native.ModuleInformation();
                    Native.GetModuleInformation(process.Handle, modulePointers[index], out moduleInformation,
                        (uint)(IntPtr.Size * (modulePointers.Length)));

                    // Convert to a normalized module and add it to our list
                    Module module = new Module(moduleName, moduleInformation.lpBaseOfDll, moduleInformation.SizeOfImage);
                    collectedModules.Add(module);
                }
            }

            return collectedModules;
        }
    }

    public class Native
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ModuleInformation
        {
            public IntPtr lpBaseOfDll;
            public uint SizeOfImage;
            public IntPtr EntryPoint;
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

    public class Module
    {
        public Module(string moduleName, IntPtr baseAddress, uint size)
        {
            this.ModuleName = moduleName;
            this.BaseAddress = baseAddress;
            this.Size = size;
        }

        public string ModuleName { get; set; }
        public IntPtr BaseAddress { get; set; }
        public uint Size { get; set; }
    }
}