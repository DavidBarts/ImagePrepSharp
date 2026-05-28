using System.Threading.Tasks;
using Avalonia.Controls;

namespace ImagePrepSharp.FrontEnd;
public partial class MaxDimDialog : Window
{
    public bool AllowCancel
    {
        get => CancelButton.IsVisible;
        set { CancelButton.IsVisible = value; }
    }

    public MaxDimDialog()
    {
        InitializeComponent();
    }

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(null);
    }

    private void OK_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(MaxDimSpinner.Value);
    }

    public async Task<int?> ShowAsync(Window parent)
    {
        return await ShowDialog<int?>(parent);
    }
}
