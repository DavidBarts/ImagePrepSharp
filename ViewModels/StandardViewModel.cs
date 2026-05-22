using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;

namespace ImagePrepSharp.ViewModels;

public partial class StandardViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    [RelayCommand]
    private void OpenScale()
    {
        System.Console.WriteLine("OpenScale clicked.");
    }

    [RelayCommand]
    private void Preferences()
    {
        System.Console.WriteLine("Preferences clicked.");
    }

    [RelayCommand]
    private void Quit()
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
    private void About()
    {
        System.Console.WriteLine("About clicked.");
    }

    [RelayCommand]
    private void Help()
    {
        System.Console.WriteLine("Help clicked.");
    }
}
