using System;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Serilog;

using ImagePrepSharp.Data;
using System.IO;

namespace ImagePrepSharp.FrontEnd;

// This can't be a ViewModel because some menu operations (e.g. those
// that pop up dialogs) need access to view objects, and the strict
// M-V-VM pattern Avalonia pushes forbids that.
static class MenuOperations
{
    private static readonly SettingsDialog settingsDialog = new();
    private static readonly AboutDialog aboutDialog = new();
    private static readonly HelpDialog helpDialog = new();

    public static async Task About(object? sender, EventArgs eventArgs, Window? parent)
    {
        aboutDialog.Show();
    }

    public static async Task Discard(object? sender, EventArgs eventArgs, Window? parent)
    {
        if (parent is not RotateWindow rotateWindow)
        {
            throw new ArgumentException("Unexpected parent type.");
        }
        rotateWindow.Close();
    }

    public static async Task Help(object? sender, EventArgs eventArgs, Window? parent)
    {
        helpDialog.Show();
    }

    public static async Task OpenScale(object? sender, EventArgs eventArgs, Window? parent)
    {
        var storageProvider = TopLevel.GetTopLevel(parent)?.StorageProvider!;
        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions {
            Title = "Open Image File",
            AllowMultiple = true,
            FileTypeFilter =
            [
                new FilePickerFileType("Image Files") {
                    Patterns = ["*.bmp", "*.gif", "*.heic", "*.heif", "*.jpeg", "*.jpg", "*.png", "*.tif", "*.tiff", "*.webp"],
                    AppleUniformTypeIdentifiers = ["public.image"]
                }
            ]
        });
        if (files.Count <= 0)
        {
            return;  // nothing selected by user
        }
        var maxDim = await new MaxDimDialog().ShowAsync(parent!);
        if (maxDim == null)
        {
            return;  // user cancelled it
        }
        foreach (var file in files)
        {
            await RotateWindow.ShowForStorageItemAsync(file, (int) maxDim, parent!);
        }
    }

    public static async Task Preferences(object? sender, EventArgs eventArgs, Window? parent)
    {
        settingsDialog.Show();
    }

    public static async Task Quit(object? sender, EventArgs eventArgs, Window? parent)
    {
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

    public static async Task SaveClose(object? sender, EventArgs eventArgs, Window? parent)
    {
        if (parent is not RotateWindow rotateWindow)
        {
            throw new ArgumentException("Unexpected parent type.");
        }
        var storageProvider = TopLevel.GetTopLevel(parent)?.StorageProvider!;
        var settings = Settings.Instance;
        var defaultExtension = settings.OutputType.ToString().ToLowerInvariant();
        var outputSuffix = settings.OutputSuffix.Contains('{') ? String.Format(settings.OutputSuffix, rotateWindow.Image.Width, rotateWindow.Image.Height) : settings.OutputSuffix;
        var defaultDirectory = settings.OutputToInputDir ? Path.GetDirectoryName(rotateWindow.Image.Path) ?? Path.DirectorySeparatorChar.ToString() : settings.OutputTo;
        var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            DefaultExtension = defaultExtension,
            SuggestedFileName = Path.GetFileNameWithoutExtension(rotateWindow.Image.Path) + outputSuffix + "." + defaultExtension,
            SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(defaultDirectory)
        });
        if (file == null)
        {
            return;  // user cancelled
        }
        var quality = await new OutQualDialog().ShowAsync(parent!);
        if (quality == null)
        {
            return; // user cancelled
        }
        try
        {
            await rotateWindow.Image.SaveAsync(file.Path.GetComponents(UriComponents.Path, UriFormat.Unescaped), (int) quality);
            rotateWindow.Close();
        }
        catch (Exception e)
        {
            var message = $"Unable to save as {file.Name}.";
            Log.Error(e, message);
            var errorDialog = MessageBoxManager.GetMessageBoxStandard("Error", message, ButtonEnum.Ok);
            await errorDialog.ShowWindowDialogAsync(parent);
        }
    }
}
