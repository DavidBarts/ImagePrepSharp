using Avalonia.Controls;

namespace ImagePrepSharp.FrontEnd;

public partial class MainWindow : StandardWindow
{
    public MainWindow()
    {
        InitializeComponent();
        Closing += OnClosing;
    }

    private async void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;
    }
}
