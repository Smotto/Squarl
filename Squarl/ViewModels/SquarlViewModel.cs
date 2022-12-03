using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace Squarl.ViewModels;

public class SquarlViewModel : ViewModelBase
{
    private bool _isBusy;
    private string? _searchText;
    private ProcessViewModel? _selectedAlbum;
    
    public SquarlViewModel()
    {
        LoadProjectCommand = ReactiveCommand.Create(() =>
        {
            return SelectedAlbum;
        });
        
        SearchResults.Add(new ProcessViewModel());
        SearchResults.Add(new ProcessViewModel());
        SearchResults.Add(new ProcessViewModel());
    }

    public ObservableCollection<ProcessViewModel> SearchResults { get; } = new();
    public ReactiveCommand<Unit, ProcessViewModel?> LoadProjectCommand { get; }

    public ProcessViewModel? SelectedAlbum
    {
        get => _selectedAlbum;
        set => this.RaiseAndSetIfChanged(ref _selectedAlbum, value);
    }

    public string? SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }
    
    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }
}