using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using ReactiveUI;
using Squarl.Models;
using Squarl.ViewModels.Environments;

namespace Squarl.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Process _currentAttachedAttachProcess = null!;
    private ObservableCollection<Process>? _processes;
    private ObservableCollection<MemoryRecord> _memoryRecords = new();

    public MainViewModel(ProcessViewModel processViewModel)
    {
        ShowDialog = new Interaction<EnvironmentViewModel, ProjectViewModel?>();

        Task.Run(async () =>
        {
            await processViewModel.LoadApplicationsAsync();
            Processes = new ObservableCollection<Process>(processViewModel.Processes!);
        });

        MemorySource = new FlatTreeDataGridSource<MemoryRecord>(_memoryRecords)
        {
            Columns =
            {
                new TextColumn<MemoryRecord, string>("Address", x => x.address),
                new TextColumn<MemoryRecord, string>("Value", x => x.value),
                new TextColumn<MemoryRecord, string>("Previous Label", x => x.previousValue),
                new TextColumn<MemoryRecord, string>("Label", x => x.label),
            },
        };
        
        OpenFolderCommand = ReactiveCommand.Create(async () =>
        {
            // Code Here Executed When Button Is Clicked.
            var enviroment = new EnvironmentViewModel();
            var result = await ShowDialog.Handle(enviroment);
        });
        
        AttachToProcessCommand = ReactiveCommand.Create(async () =>
        {
            await processViewModel.LoadApplicationsAsync();
            Processes = new ObservableCollection<Process>(processViewModel.Processes!);
        });
        
        LoadRunningProcessesCommand = ReactiveCommand.Create(async () => { });

        ReloadRunningProcessesCommand = ReactiveCommand.Create(async () => { });
    }
    
    public bool IsChanged => CurrentAttachedProcess != _currentAttachedAttachProcess;

    /// <summary>
    /// Getter and setter for current attached process.
    /// </summary>
    public Process CurrentAttachedProcess
    {
        get => _currentAttachedAttachProcess;
        set => this.RaiseAndSetIfChanged(ref _currentAttachedAttachProcess, value);
    }

    /// <summary>
    /// Getter and setter for process list.
    /// </summary>
    public ObservableCollection<Process> Processes
    {
        get => _processes!;
        set => this.RaiseAndSetIfChanged(ref _processes, value);
    }

    public ICommand OpenFolderCommand { get; }
    public ICommand AttachToProcessCommand { get; }
    public ICommand LoadRunningProcessesCommand { get; }
    public ICommand ReloadRunningProcessesCommand { get; }
    
    public FlatTreeDataGridSource<MemoryRecord> MemorySource { get; }
    public Interaction<EnvironmentViewModel, ProjectViewModel?> ShowDialog { get; }
}