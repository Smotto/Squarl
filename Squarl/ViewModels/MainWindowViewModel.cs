using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using ReactiveUI;
using Squarl.Models;

namespace Squarl.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ObservableCollection<MemoryRecord> _memoryRecords = new()
    {
        new MemoryRecord() { address = "0010455C", value = "1", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010555C", value = "2", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010655C", value = "3", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010755C", value = "4", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "5", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "6", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "7", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "8", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "9", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "10", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "11", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "12", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "13", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "14", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "15", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "16", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "17", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "18", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "19", previousValue = "previous value", label = "label"},
        new MemoryRecord() { address = "0010855C", value = "20", previousValue = "previous value", label = "label"},
    };
    
    public MainWindowViewModel()
    {
        ShowDialog = new Interaction<SquarlViewModel, ProcessViewModel?>();
        
        OpenFolderCommand = ReactiveCommand.Create(async () =>
        {
            // Code Here Executed When Button Is Clicked.
            var store = new SquarlViewModel();

            var result = await ShowDialog.Handle(store);
        });
        
        Source = new FlatTreeDataGridSource<MemoryRecord>(_memoryRecords)
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
    public Interaction<SquarlViewModel, ProcessViewModel?> ShowDialog { get; }
    public FlatTreeDataGridSource<MemoryRecord> Source { get; }
}