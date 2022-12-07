using Memory.Utils;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Squarl.CLI;

namespace Squarl.CLI
{
    class MemoryHelper32
    {
        Process process;
        public MemoryHelper32(Process TargetProcess)
        {
            process = TargetProcess;
        }
 
        public uint GetBaseAddress(uint StartingAddress)
        {
            return (uint)process.MainModule.BaseAddress + StartingAddress;
        }
 
        public byte[] ReadMemoryBytes(uint MemoryAddress, uint Bytes)
        {
            byte[] data = new byte[Bytes];
            ReadProcessMemory(process.Handle, MemoryAddress, data, data.Length, IntPtr.Zero);
            return data;
        }
 
        public T ReadMemory<T>(uint MemoryAddress)
        {
            byte[] data = ReadMemoryBytes(MemoryAddress, (uint)Marshal.SizeOf(typeof(T)));
 
            T t;
            GCHandle PinnedStruct = GCHandle.Alloc(data, GCHandleType.Pinned);
            try { t = (T)Marshal.PtrToStructure(PinnedStruct.AddrOfPinnedObject(), typeof(T)); }
            catch (Exception ex) { throw ex; }
            finally { PinnedStruct.Free(); }
 
            return t;
        }
 
        public bool WriteMemory<T>(uint MemoryAddress, T Value)
        {
            IntPtr bw = IntPtr.Zero;
 
            int sz = ObjectType.GetSize<T>();
            byte[] data = ObjectType.GetBytes<T>(Value);
            bool result = WriteProcessMemory(process.Handle, MemoryAddress, data, sz, out bw);
            return result && bw != IntPtr.Zero;
        }
 
        public void Close()
        {
            CloseHandle(process.Handle);
        }
 
        #region PInvoke
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            uint lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            IntPtr lpNumberOfBytesRead);
 
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(
            IntPtr hProcess,
            uint lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out IntPtr lpNumberOfBytesWritten
            );
 
        [DllImport("kernel32.dll")]
        private static extern Int32 CloseHandle(IntPtr hProcess);
        #endregion
    }
}
 
namespace Memory.Win64
{
    class MemoryHelper64
    {
        private Process _process;
 
        public MemoryHelper64(Process targetProcess)
        {
            _process = targetProcess;
        }
 
        public ulong GetBaseAddress(ulong startingAddress)
        {
            return (ulong)_process.MainModule!.BaseAddress.ToInt64() + startingAddress;
        }
 
        public byte[] ReadMemoryBytes(ulong memoryAddress, int bytes)
        {
            byte[] data = new byte[bytes];
            ReadProcessMemory(_process.Handle, memoryAddress, data, data.Length, IntPtr.Zero);
            return data;
        }
 
        public T ReadMemory<T>(ulong memoryAddress)
        {
            byte[] data = ReadMemoryBytes(memoryAddress, Marshal.SizeOf(typeof(T)));
 
            T t;
            GCHandle pinnedStruct = GCHandle.Alloc(data, GCHandleType.Pinned);
            try { t = (T)Marshal.PtrToStructure(pinnedStruct.AddrOfPinnedObject(), typeof(T))!; }
            finally { pinnedStruct.Free(); }
 
            return t;
        }
 
        public bool WriteMemory<T>(ulong memoryAddress, T value)
        {
            IntPtr bw = IntPtr.Zero;
 
            int sz = ObjectType.GetSize<T>();
            byte[] data = ObjectType.GetBytes<T>(value);
            bool result = WriteProcessMemory(_process.Handle, memoryAddress, data, sz, out bw);
            return result && bw != IntPtr.Zero;
        }
 
        public void Close()
        {
            CloseHandle(_process.Handle);
        }
 
        #region PInvoke
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            ulong lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            IntPtr lpNumberOfBytesRead);
 
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(
            IntPtr hProcess,
            ulong lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out IntPtr lpNumberOfBytesWritten
            );
 
        [DllImport("kernel32.dll")]
        private static extern Int32 CloseHandle(IntPtr hProcess);
        #endregion
    }
}
 
namespace Memory.Utils
{
    static class MemoryUtils
    {
        public static uint OffsetCalculator(MemoryHelper32 targetMemory, uint baseAddress, int[] offsets)
        {
            var address = baseAddress;
            foreach (uint offset in offsets)
            {
                address = targetMemory.ReadMemory<uint>(address) + offset;
            }
            return address;
        }
 
        public static ulong OffsetCalculator(Win64.MemoryHelper64 targetMemory, ulong baseAddress, int[] offsets)
        {
            var address = baseAddress;
            foreach (uint offset in offsets)
            {
                address = targetMemory.ReadMemory<ulong>(address) + offset;
            }
            return address;
        }
    }
 
    public static class ObjectType
    {
        public static int GetSize<T>()
        {
            return Marshal.SizeOf(typeof(T));
        }
 
        public static byte[] GetBytes<T>(T value)
        {
            string typename = typeof(T).ToString();
            Console.WriteLine(typename);
            switch (typename)
            {
                case "System.Single":
                    return BitConverter.GetBytes((float)Convert.ChangeType(value, typeof(float))!);
                case "System.Int32":
                    return BitConverter.GetBytes((int)Convert.ChangeType(value, typeof(int))!);
                case "System.Int64":
                    return BitConverter.GetBytes((long)Convert.ChangeType(value, typeof(long))!);
                case "System.Double":
                    return BitConverter.GetBytes((double)Convert.ChangeType(value, typeof(double))!);
                // case "System.Byte":
                    // return BitConverter.GetBytes((byte)Convert.ChangeType(value, typeof(byte)));
                case "System.String":
                    return Encoding.Unicode.GetBytes((string)Convert.ChangeType(value, typeof(string))!);
                default:
                    return Array.Empty<byte>();
            }
        }
    }
}