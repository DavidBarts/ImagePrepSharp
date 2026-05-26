using Avalonia.Controls;

namespace ImagePrepSharp.FrontEnd;

public partial class AboutDialog : Window
{
    public AboutDialog()
    {
        InitializeComponent();
        Closing += OnClosing;
    }

    private async void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;  // never close, because we reuse this one
        Hide();
    }
}
