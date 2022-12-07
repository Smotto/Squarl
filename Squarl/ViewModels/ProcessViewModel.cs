using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Squarl.Engine;

namespace Squarl.ViewModels;

public class ProcessViewModel : ViewModelBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ProcessViewModel()
    {
    }

    /// <summary>
    /// Getters and setters
    /// </summary>
    public IEnumerable<Process>? Processes { get; private set; }

    /// <summary>
    /// Loading All Running Processes on Machine into IEnumerable _processes
    /// </summary>
    public async Task LoadProcessesAsync()
    {
        Processes = await ProcessEngine.GrabAllRunningProcesses();
    }

    /// <summary>
    /// Loads all relevant running processes that appear at the top of Windows Task Manager.
    /// </summary>
    public async Task LoadApplicationsAsync()
    {
        var processes = await ProcessEngine.GrabAllRunningProcesses();
        await Task.Run(() =>
        {
            var processList = processes!.Where(process => process.MainWindowHandle != IntPtr.Zero).ToList();
            Processes = processList;
        });
    }
}