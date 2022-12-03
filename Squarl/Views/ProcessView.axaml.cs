using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Squarl.Views;

public partial class ProcessView : UserControl
{
    public ProcessView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}