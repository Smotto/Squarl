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
    private Process? _process;

    // Default Constructor
    public ProcessViewModel()
    {
    }

    // Constructor takes in a single process
    public ProcessViewModel(Process process)
    {
        _process = process;
    }
    
    public Process? Process
    {
        get => _process;
        private set => this.RaiseAndSetIfChanged(ref _process, value);
    }

    public string Name => _process!.ProcessName;
    public nint Handle => _process!.Handle;

    public async Task AttachToProcess()
    {
        
    }

    // private Processes _processes;
    // private Process _selectedProcess;
    //
    // public ProcessViewModel()
    // {
    //     AttachToProcessCommand = ReactiveCommand.Create(() =>
    //     {
    //         return _selectedProcess;
    //     });
    // }
    //
    // public ObservableCollection<ProcessViewModel> SearchResults { get; } = new();
    // public ReactiveCommand<Unit, Process?> AttachToProcessCommand { get;  }
    //
    // public Process[] getProcesses()
    // {
    //     return _processes.AllRunningProcesses;
    // }
    //
    // public List<string> getProcessNames()
    // {
    //     List<string> processNames = new List<string>();
    //     foreach (Process process in _processes.AllRunningProcesses)
    //     {
    //         processNames.Add(process.ProcessName);
    //     }
    //
    //     return processNames;
    // }
}