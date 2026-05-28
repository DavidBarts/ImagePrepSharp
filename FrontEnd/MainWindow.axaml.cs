using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using ImagePrepSharp.Data;

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

    private async void OnDrop(object? sender, DragEventArgs e)
    {
        if (e.DataTransfer.Formats.Contains(DataFormat.File))
        {
            var files = e.DataTransfer.TryGetFiles();
            if (files != null && files.Length > 0)
            {
                var maxDim = (await new MaxDimDialog().ShowAsync(this)) ?? Settings.Instance.MaxDimension;
                foreach (var file in files)
                {
                    await RotateWindow.ShowForStorageItemAsync(file, maxDim, this);
                }
            }
        }
    }
}
