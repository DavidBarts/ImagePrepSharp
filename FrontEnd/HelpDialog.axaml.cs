using Avalonia.Controls;
using ImagePrepSharp.Data;

namespace ImagePrepSharp.FrontEnd;

public partial class HelpDialog : Window
{
    public HelpDialog()
    {
        InitializeComponent();
        Closing += OnClosing;
        ConfigFilePath.Text = Settings.FILE_NAME;
    }

    private async void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;  // never close, because we reuse this one
        Hide();
    }
}
