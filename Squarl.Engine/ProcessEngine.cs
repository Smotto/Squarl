using System.Diagnostics;

namespace Squarl.Engine;

public class ProcessEngine
{
    public ProcessEngine() { }
    
    public async Task<Process[]?> GrabAllRunningProcesses()
    {
        return await Task.Run(Process.GetProcesses);
    }
}