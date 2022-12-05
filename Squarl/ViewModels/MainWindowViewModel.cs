using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using ReactiveUI;
using Squarl.Engine;
using Squarl.Models;
using Squarl.ViewModels.Environments;

namespace Squarl.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ObservableCollection<MemoryRecord> _memoryRecords = new()
    {
        new MemoryRecord() { address = "0010455C", value = "1", previousValue = "previous value", label = "label" },
    };

    public MainWindowViewModel()
    {
        ShowDialog = new Interaction<EnvironmentViewModel, ProjectViewModel?>();

        OpenFolderCommand = ReactiveCommand.Create(async () =>
        {
            // Code Here Executed When Button Is Clicked.
            var enviroment = new EnvironmentViewModel();

            var result = await ShowDialog.Handle(enviroment);
        });

        SelectProcessCommand = ReactiveCommand.Create(() =>
        {
            // TODO: Select a process from the current list of processes
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
    }

    public ICommand OpenFolderCommand { get; }
    public ICommand SelectProcessCommand { get; }
    public Interaction<EnvironmentViewModel, ProjectViewModel?> ShowDialog { get; }
    public FlatTreeDataGridSource<MemoryRecord> MemorySource { get; }

}