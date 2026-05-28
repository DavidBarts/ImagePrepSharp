using System;
using Avalonia.Controls;
using Serilog;
using ImagePrepSharp.Data;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Avalonia.Platform.Storage;

namespace ImagePrepSharp.FrontEnd;
public partial class SettingsDialog : Window
{
    public SettingsDialog()
    {
        InitializeComponent();
        ResetValues();
        Closing += OnClosing;
    }

    private void ResetValues()
    {
        var settings = Settings.Instance;
        MaxDimension.Value = settings.MaxDimension;
        OutputQuality.Value = settings.OutputQuality;
        OutputSuffix.Text = settings.OutputSuffix;
        OutputToInputDir.IsChecked = settings.OutputToInputDir;
        CreateOutputIn.IsChecked = !settings.OutputToInputDir;
        OutputTo.Text = settings.OutputTo;
        OutputTypeJpeg.IsChecked = settings.OutputType == OutputType.JPEG;
        OutputTypeWebp.IsChecked = settings.OutputType == OutputType.WEBP;
    }

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ResetValues();
        Hide();
    }

    private async void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var settings = Settings.Instance;
        settings.MaxDimension = MaxDimension.Value;
        settings.OutputQuality = OutputQuality.Value;
        if (OutputSuffix.Text != null)
            settings.OutputSuffix = OutputSuffix.Text;
        settings.OutputToInputDir = OutputToInputDir.IsChecked ?? false;
        if (OutputTo.Text != null)
            settings.OutputTo = OutputTo.Text;
        settings.OutputType = GetOutputType();
        try
        {
            settings.Save();
        }
        catch (Exception exc)
        {
            var message = "Unable to save settings.";
            Log.Error(exc, message);
            var errorDialog = MessageBoxManager.GetMessageBoxStandard("Error", message, ButtonEnum.Ok);
            await errorDialog.ShowWindowDialogAsync(this);
            return;
        }
        Hide();
    }

    private async void Change_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider!;
        var folders = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            AllowMultiple = false,
            SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(OutputTo.Text!)
        });
        if (folders.Count < 1)
        {
            return;  // user cancelled
        }
        OutputTo.Text = folders[0].Path.GetComponents(UriComponents.Path, UriFormat.Unescaped);
    }

    private async void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;  // never close, because we reuse this one
        if (Changed())
        {
            var confirmDialog = MessageBoxManager.GetMessageBoxStandard("Confirm Closing Settings", "Really close settings window? All unsaved changes will be discarded.", ButtonEnum.OkCancel);
            var answer = await confirmDialog.ShowWindowDialogAsync(this);
            if (answer == ButtonResult.Ok)
            {
                ResetValues();
                Hide();
            }
        }
        else
        {
            Hide();
        }
    }

    private OutputType GetOutputType()
    {
        if (OutputTypeJpeg.IsChecked ?? false)
            return OutputType.JPEG;
        if (OutputTypeWebp.IsChecked ?? false)
            return OutputType.WEBP;
        throw new InvalidOperationException("Neither button checked!");
    }

    private bool Changed()
    {
        var settings = Settings.Instance;
        return MaxDimension.Value != settings.MaxDimension ||
            OutputQuality.Value != settings.OutputQuality ||
            OutputSuffix.Text != settings.OutputSuffix ||
            OutputToInputDir.IsChecked != settings.OutputToInputDir ||
            OutputTo.Text != settings.OutputTo ||
            GetOutputType() != settings.OutputType;
    }
}
