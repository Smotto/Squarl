using ReactiveUI;

namespace Squarl.ViewModels;

public class ViewModelBase : ReactiveObject
{
    int selectedIndex;
    public int SelectedIndex
    {
        get => selectedIndex;
        set => this.RaiseAndSetIfChanged(ref selectedIndex, value);
    }
}