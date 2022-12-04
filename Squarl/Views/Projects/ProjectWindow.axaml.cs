using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using Squarl.ViewModels;

namespace Squarl.Views;

public partial class ProjectWindow : ReactiveWindow<ProjectWindowViewModel>
{
    public ProjectWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.LoadProjectCommand.Subscribe(Close)));
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}