using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
        System.Environment.Exit(0);
    }
}
