using System.Diagnostics;

namespace Squarl.Engine;

public class Processes
{
    public static async Task<Process[]> GrabAllRunningProcesses()
    {
        return await Task.Run(Process.GetProcesses);
    }
}