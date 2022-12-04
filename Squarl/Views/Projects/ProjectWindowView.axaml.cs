using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Squarl.Views;

public partial class ProjectWindowView : UserControl
{
    public ProjectWindowView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}