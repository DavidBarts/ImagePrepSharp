using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;

namespace ImagePrepSharp.ViewModels;

public partial class StandardViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    [RelayCommand]
    public void OpenScale()
    {
        System.Console.WriteLine("OpenScale clicked.");
    }

    [RelayCommand]
    public void Preferences()
    {
        System.Console.WriteLine("Preferences clicked.");
    }

    [RelayCommand]
    public void Quit()
    {
        System.Console.WriteLine("Quit clicked.");
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
        {
            desktopApp.Shutdown();
        }
        else
        {
            /* shouldn't be needed, but Justin Case */
            System.Environment.Exit(0);
        }
    }

    [RelayCommand]
    public void About()
    {
        System.Console.WriteLine("About clicked.");
    }

    [RelayCommand]
    public void Help()
    {
        System.Console.WriteLine("Help clicked.");
    }
}
