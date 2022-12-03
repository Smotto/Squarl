using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Squarl.ViewModels;

namespace Squarl.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }
    
    private async Task DoShowDialogAsync(InteractionContext<SquarlViewModel, ProcessViewModel?> interaction)
    {
        var dialog = new ProjectWindow();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<ProcessViewModel?>(this);
        interaction.SetOutput(result);
    }
}