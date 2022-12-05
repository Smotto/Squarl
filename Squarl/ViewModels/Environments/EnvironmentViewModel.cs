using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace Squarl.ViewModels.Environments;

public class EnvironmentViewModel : ViewModelBase
{
    private bool _isBusy;
    private string? _searchText;
    private ProjectViewModel? _selectedProject;

    public EnvironmentViewModel()
    {
        LoadProjectCommand = ReactiveCommand.Create(() => { return SelectedProject; });
        SearchResults.Add(new ProjectViewModel());
        SearchResults.Add(new ProjectViewModel());
        SearchResults.Add(new ProjectViewModel());
    }

    public ReactiveCommand<Unit, ProjectViewModel?> LoadProjectCommand { get; }
    public ObservableCollection<ProjectViewModel> SearchResults { get; } = new();

    public ProjectViewModel? SelectedProject
    {
        get => _selectedProject;
        set => this.RaiseAndSetIfChanged(ref _selectedProject, value);
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