using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using Squarl.Engine;
namespace Squarl.ViewModels;

public class ProcessViewModel : ViewModelBase
{
    private Processes[] _runningProcesses;
    private Process? _process;

    public ProcessViewModel(Process process)
    {
        _process = process;
    }
    
    public Process? Process
    {
        get => _process;
        private set => this.RaiseAndSetIfChanged(ref _process, value);
    }

    public Processes[] Processes
    {
        get => _runningProcesses;
        private set => this.RaiseAndSetIfChanged(ref _runningProcesses, value);
    }

    public string Name => _process!.ProcessName;
    public nint Handle => _process!.Handle;

    public static async Task<Process[]> GrabAllRunningProcesses()
    {
        return await Task.Run(Process.GetProcesses);
    }
}