using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace Squarl.ViewModels;

public class ProjectWindowViewModel : ViewModelBase
{
    private bool _isBusy;
    private string? _searchText;
    private ProjectViewModel? _selectedProject;
    
    public ProjectWindowViewModel()
    {
        LoadProjectCommand = ReactiveCommand.Create(() =>
        {
            return SelectedProject;
        });
        
        SearchResults.Add(new ProjectViewModel());
        SearchResults.Add(new ProjectViewModel());
        SearchResults.Add(new ProjectViewModel());
    }

    public ObservableCollection<ProjectViewModel> SearchResults { get; } = new();
    public ReactiveCommand<Unit, ProjectViewModel?> LoadProjectCommand { get; }

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