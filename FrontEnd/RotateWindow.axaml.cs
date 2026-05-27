using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ImagePrepSharp.BackEnd;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Serilog;

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

    public static async Task ShowForStorageItemAsync(IStorageItem item, int maxDim, Window parent)
    {
        BackEndImage? image = null;
        try
        {
            image = new BackEndImage();
            await image.LoadAsync(item.Path.AbsolutePath);
            var scaled = await image.ScaleMapColorAsync((int) maxDim);
            image.DisposeIfDifferentFrom(scaled);
            image = scaled;
        }
        catch (Exception e)
        {
            var message = $"Unable to load {item.Name}.";
            Log.Error(e, message);
            var errorDialog = MessageBoxManager.GetMessageBoxStandard("Error", message, ButtonEnum.Ok);
            await errorDialog.ShowWindowDialogAsync(parent);
            return;
        }
        new RotateWindow(image!).Show(parent);
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
