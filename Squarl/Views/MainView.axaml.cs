using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            IEnumerable<Process?> applications = (await ProcessEngine.GrabAllRunningApplications())!;
            var enumerable = applications.ToList();
            source!.Items = enumerable;
        });
        e.Handled = true;
    }
}