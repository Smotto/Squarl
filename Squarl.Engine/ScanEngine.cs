using System.Diagnostics;
using System.Text;
using static Squarl.Engine.Native;

// ReSharper disable All

namespace Squarl.Engine;

public class ScanEngine
{
    public static List<Module> CollectModules(Process process)
    {
        List<Module> collectedModules = new List<Module>();

        IntPtr[] modulePointers = Array.Empty<nint>();

        // Determine number of modules
        if (!EnumProcessModulesEx(process.Handle, modulePointers, 0, out var bytesNeeded,
                (uint)ModuleFilter.ListModulesAll))
        {
            return collectedModules;
        }

        var totalNumberOfModules = bytesNeeded / IntPtr.Size;
        modulePointers = new IntPtr[totalNumberOfModules];

        // Collect modules from the process
        if (EnumProcessModulesEx(process.Handle, modulePointers, bytesNeeded, out bytesNeeded,
                (uint)ModuleFilter.ListModulesAll))
        {
            for (int index = 0; index < totalNumberOfModules; index++)
            {
                StringBuilder moduleFilePath = new StringBuilder(1024);
                GetModuleFileNameEx(process.Handle, modulePointers[index], moduleFilePath,
                    (uint)(moduleFilePath.Capacity));

                string moduleName = Path.GetFileName(moduleFilePath.ToString());
                ModuleInformation moduleInformation = new ModuleInformation();
                GetModuleInformation(process.Handle, modulePointers[index], out moduleInformation,
                    (uint)(IntPtr.Size * (modulePointers.Length)));

                // Convert to a normalized module and add it to our list
                Module module = new Module(moduleName, moduleInformation.lpBaseOfDll,
                    moduleInformation.SizeOfImage);
                collectedModules.Add(module);
            }
        }

        return collectedModules;
    }
}