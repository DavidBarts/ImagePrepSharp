using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

using ImagePrepSharp.Data;

namespace ImagePrepSharp.FrontEnd;

// This can't be a ViewModel because some menu operations (e.g. those
// that pop up dialogs) need access to view objects, and the strict
// M-V-VM pattern Avalonia pushes forbids that.
static class MenuOperations
{
    public static async Task About(object? sender, System.EventArgs eventArgs, Window? parent)
    {
        System.Console.WriteLine("About clicked.");
    }

    public static async Task Discard(object? sender, System.EventArgs eventArgs, Window? parent)
    {
        System.Console.WriteLine("Discard clicked.");
    }

    public static async Task Help(object? sender, System.EventArgs eventArgs, Window? parent)
    {
        System.Console.WriteLine("Help clicked.");
    }

    public static async Task OpenScale(object? sender, System.EventArgs eventArgs, Window? parent)
    {
        System.Console.WriteLine("OpenScale clicked.");
    }

    public static async Task Preferences(object? sender, System.EventArgs eventArgs, Window? parent)
    {
        System.Console.WriteLine("Preferences clicked.");
        var settings = Settings.Instance;
        System.Console.WriteLine($"Output suffix is {settings.OutputSuffix}.");
        // settings.Save();
    }

    public static async Task Quit(object? sender, System.EventArgs eventArgs, Window? parent)
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

    public static async Task SaveClose(object? sender, System.EventArgs eventArgs, Window? parent)
    {
        System.Console.WriteLine("SaveClose clicked.");
    }
}
