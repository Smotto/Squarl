using System.Diagnostics;

namespace Squarl.Engine;

public class ProcessEngine
{
    /// <summary>
    /// Grab every single process running on the client's machine.
    /// </summary>
    /// <returns>
    /// A Task needed to run to generate a list of processes.
    /// </returns>
    public static async Task<IEnumerable<Process>?> GrabAllRunningProcesses()
    {
        return await Task.Run(Process.GetProcesses);
    }

    public static async Task<IEnumerable<Process>?> GrabAllRunningApplications()
    {
        Process[] allProcesses = await Task.Run(Process.GetProcesses);
        IEnumerable<Process>? processList = allProcesses;
        await Task.Run(() =>
        {
            return processList = allProcesses!.Where(process => process.MainWindowHandle != IntPtr.Zero).ToList();
        });

        return processList;
    }
}
