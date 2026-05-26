using System.Threading.Tasks;
using Avalonia.Controls;

using ImagePrepSharp.Data;

namespace ImagePrepSharp.FrontEnd;
public partial class OutQualDialog : Window
{
    public OutQualDialog()
    {
        InitializeComponent();
        OutQualSpinner.Value = Settings.Instance.OutputQuality;
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
