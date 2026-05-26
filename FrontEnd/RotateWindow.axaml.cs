using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ImagePrepSharp.BackEnd;

namespace ImagePrepSharp.FrontEnd;

public partial class RotateWindow : StandardWindow
{
    public BackEndImage Image {get; private set;}

    public RotateWindow(BackEndImage image)
    {
        InitializeComponent();
        Image = image;
        Title = System.IO.Path.GetFileName(image.Path);
        ImageScroller.MaxWidth = (Screens.Primary?.WorkingArea.Width ?? 1024) * 3 / 4;
        ImageScroller.MaxHeight = (Screens.Primary?.WorkingArea.Height ?? 768) * 3 / 4;
        ImageDisplay.Source = image.ToBitmap();
    }

    private async void Rotate90CW_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await DoRotate(90);
    }

    private async void Rotate180_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await DoRotate(180);
    }

    private async void Rotate90CCW_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await DoRotate(270);
    }

    private async Task DoRotate(int degrees)
    {
        var rotated = await Image.RotateAsync(degrees);
        var oldDisplayed = ImageDisplay.Source;
        ImageDisplay.Source = rotated.ToBitmap();
        if (oldDisplayed is IDisposable disp)
        {
            disp.Dispose();
        }
        Image.DisposeIfDifferentFrom(rotated);
        Image = rotated;
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);
        Image.Dispose();
    }
}
