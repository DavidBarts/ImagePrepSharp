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
        }

        base.OnFrameworkInitializationCompleted();
    }

    // See note in FrontEnd/StandardWindow.cs
    private void About_OnClick(object? sender, System.EventArgs e)
    {
        MenuOperations.About(sender, e, null);
    }

    private void Preferences_OnClick(object? sender, System.EventArgs e)
    {
        MenuOperations.Preferences(sender, e, null);
    }
}
