using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using ReactiveUI;
using Squarl.Models;
using Squarl.ViewModels.Environments;

namespace Squarl.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Process _currentProcess = new();
    private ProcessViewModel _processViewModel = new();
    private ObservableCollection<Process> _processes = new();
    private ObservableCollection<MemoryRecord> _memoryRecords = new();
    
    public MainViewModel()
    {
        ProcessComboBoxSource = new ComboBox();
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
        ShowDialog = new Interaction<EnvironmentViewModel, ProjectViewModel?>();
        OpenFolderCommand = ReactiveCommand.Create(async () =>
        {
            // Code Here Executed When Button Is Clicked.
            var enviroment = new EnvironmentViewModel();
            var result = await ShowDialog.Handle(enviroment);
        });
        AttachToProcessCommand = ReactiveCommand.Create(async () =>
        {
            Console.WriteLine(CurrentProcess);
        });
        // Loads all the running processes into the combo box.
        LoadRunningProcessesCommand = ReactiveCommand.Create(async () =>
        {
            await _processViewModel.LoadProcessesAsync();
            _processes = _processViewModel.Processes!;
            ProcessComboBoxSource!.Items = _processes;
        });
    }

    public bool IsChanged => CurrentProcess != _currentProcess;

    public Process CurrentProcess
    {
        get => _currentProcess; 
        set => this.RaiseAndSetIfChanged(ref _currentProcess, value);
    }
    public ICommand OpenFolderCommand { get; }
    public ICommand AttachToProcessCommand { get; }
    public ICommand LoadRunningProcessesCommand { get; }
    public Interaction<EnvironmentViewModel, ProjectViewModel?> ShowDialog { get; }
    public FlatTreeDataGridSource<MemoryRecord> MemorySource { get; }
    public ComboBox ProcessComboBoxSource { get; }
}