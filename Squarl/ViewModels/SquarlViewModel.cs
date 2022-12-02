using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace Squarl.ViewModels;

public class SquarlViewModel : ViewModelBase
{
    private bool _isBusy;
    private string? _searchText;
    private AlbumViewModel? _selectedAlbum;
    
    public SquarlViewModel()
    {
        LoadProjectCommand = ReactiveCommand.Create(() =>
        {
            return SelectedAlbum;
        });
        
        SearchResults.Add(new AlbumViewModel());
        SearchResults.Add(new AlbumViewModel());
        SearchResults.Add(new AlbumViewModel());
    }

    public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();
    public ReactiveCommand<Unit, AlbumViewModel?> LoadProjectCommand { get; }

    public AlbumViewModel? SelectedAlbum
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