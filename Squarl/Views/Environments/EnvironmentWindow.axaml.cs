using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using Squarl.ViewModels.Environments;

namespace Squarl.Views.Environments;

public partial class EnvironmentWindow : ReactiveWindow<EnvironmentViewModel>
{
    public EnvironmentWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d => d(ViewModel!.LoadProjectCommand.Subscribe(Close!)));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}