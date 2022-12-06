using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Squarl.Engine;

namespace Squarl.ViewModels;

public class ProcessViewModel : ViewModelBase
{
    private Process _currentProcess;
    private ObservableCollection<Process>? _processes;

    public ProcessViewModel()
    {
        _currentProcess = new();
        _processes = new();
    }

    public Process CurrentProcess => _currentProcess;
    public ObservableCollection<Process>? Processes => _processes;
    
    public async Task LoadProcessesAsync()
    {
        ProcessEngine processEngine = new ProcessEngine();
        foreach (var process in (await processEngine.GrabAllRunningProcesses())!)
        {
            _processes?.Add(process);
        }
    }
}