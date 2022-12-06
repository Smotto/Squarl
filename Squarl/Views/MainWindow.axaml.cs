using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Squarl.ViewModels;
using Squarl.ViewModels.Environments;
using Squarl.Views.Environments;

namespace Squarl.Views;

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }
    
    private async Task DoShowDialogAsync(InteractionContext<EnvironmentViewModel, ProjectViewModel?> interaction)
    {
        var dialog = new EnvironmentWindow();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<ProjectViewModel?>(this);
        interaction.SetOutput(result);
    }
}