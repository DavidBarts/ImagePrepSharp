using Avalonia.Controls;

namespace ImagePrepSharp.FrontEnd;

public class StandardWindow : Window
{
    // These just pass the buck to MenuOperations because some things are
    // in common between here and the Apple-specific stuff in App.axaml.cs
    protected async void About_OnClick(object? sender, System.EventArgs e)
    {
        await MenuOperations.About(sender, e, this);
    }

    protected async void Discard_OnClick(object? sender, System.EventArgs e)
    {
        await MenuOperations.Discard(sender, e, this);
    }

    protected async void Help_OnClick(object? sender, System.EventArgs e)
    {
        await MenuOperations.Help(sender, e, this);
    }

    protected async void OpenScale_OnClick(object? sender, System.EventArgs e)
    {
        await MenuOperations.OpenScale(sender, e, this);
    }

    protected async void Preferences_OnClick(object? sender, System.EventArgs e)
    {
        await MenuOperations.Preferences(sender, e, this);
    }

    protected async void Quit_OnClick(object? sender, System.EventArgs e)
    {
        await MenuOperations.Quit(sender, e, this);
    }

    protected async void SaveClose_OnClick(object? sender, System.EventArgs e)
    {
        await MenuOperations.SaveClose(sender, e, this);
    }
}
