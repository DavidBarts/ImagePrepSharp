using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ImagePrepSharp.ViewModels;
using ImagePrepSharp.Views;

namespace ImagePrepSharp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var standardViewModel = new StandardViewModel();
        DataContext = standardViewModel;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = standardViewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
