using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Squarl.Engine;

namespace Squarl.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void ProcessComboBox_OnTapped(object? sender, TappedEventArgs e)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var source = sender as ComboBox;
            source!.Items = await ProcessEngine.GrabAllRunningApplications();
            e.Handled = true;
        });
    }
}