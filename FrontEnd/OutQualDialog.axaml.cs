using System.Threading.Tasks;
using Avalonia.Controls;

namespace ImagePrepSharp.FrontEnd;
public partial class OutQualDialog : Window
{
    public OutQualDialog()
    {
        InitializeComponent();
    }

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(null);
    }

    private void OK_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(OutQualSpinner.Value);
    }

    public async Task<int?> ShowAsync(Window parent)
    {
        return await ShowDialog<int?>(parent);
    }
}
