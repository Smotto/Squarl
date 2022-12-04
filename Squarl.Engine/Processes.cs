using System.Diagnostics;

namespace Squarl.Engine;

public class Processes
{
    private Process[] _allRunningProcesses = Process.GetProcesses();
    public Process[] AllRunningProcesses
    {
        get => _allRunningProcesses;
        set => _allRunningProcesses = value ?? throw new ArgumentNullException(nameof(value));
    }
    
}