using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ImagePrepSharp.Data;
using ImagePrepSharp.FrontEnd;

namespace ImagePrepSharp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.ShutdownRequested += OnShutdown;
            if (desktop.Args?.Length > 0)
            {
                OpenFilesFromArgs(desktop.MainWindow!, desktop.Args);
            }
            if (Current?.TryGetFeature<IActivatableLifetime>() is { } activatableLifetime)
            {
                activatableLifetime.Activated += async (_, e) => { HandleOpenRequest(desktop.MainWindow!, e); };
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    // See note in FrontEnd/StandardWindow.cs
    private async void About_OnClick(object? sender, System.EventArgs e)
    {
        await MenuOperations.About(sender, e, null);
    }

    private async void Preferences_OnClick(object? sender, System.EventArgs e)
    {
        await MenuOperations.Preferences(sender, e, null);
    }

    protected virtual void OnShutdown(object? sender, ShutdownRequestedEventArgs e)
    {
        // XXX - For some reason it sits there w/o exiting sometimes unless
        // this is here. Probably an ugly fix.
        Environment.Exit(0);
    }

    private async void OpenFilesFromArgs(Window parent, string[] fileNames)
    {
        var storageProvider = TopLevel.GetTopLevel(parent)?.StorageProvider!;
        var maxDim = (await new MaxDimDialog().ShowAsync(parent)) ?? Settings.Instance.MaxDimension;
        foreach (var file in fileNames)
        {
            var fileItem = await storageProvider.TryGetFileFromPathAsync(
                new UriBuilder { Scheme = Uri.UriSchemeFile, Host = "", Path = Path.GetFullPath(file) }.Uri);
            await RotateWindow.ShowForStorageItemAsync(fileItem!, maxDim, parent);
        }
    }

    private async void HandleOpenRequest(Window main, ActivatedEventArgs e)
    {
        if (e is not FileActivatedEventArgs fileArgs || fileArgs.Files.Count < 1)
        {
            return;
        }
        var maxDim = (await new MaxDimDialog().ShowAsync(main)) ?? Settings.Instance.MaxDimension;
        foreach (var file in fileArgs.Files)
        {
            await RotateWindow.ShowForStorageItemAsync(file, maxDim, main);
        }
    }
}
