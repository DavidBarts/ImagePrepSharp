using Avalonia.Controls;

namespace ImagePrepSharp.FrontEnd;

public class StandardWindow : Window
{
    // These just pass the buck to MenuOperations because some things are
    // in common between here and the Apple-specific stuff in App.axaml.cs
    protected void About_OnClick(object? sender, System.EventArgs e)
    {
        MenuOperations.About(sender, e, this);
    }

    protected void Discard_OnClick(object? sender, System.EventArgs e)
    {
        MenuOperations.Discard(sender, e, this);
    }

    protected void Help_OnClick(object? sender, System.EventArgs e)
    {
        MenuOperations.Help(sender, e, this);
    }

    protected void OpenScale_OnClick(object? sender, System.EventArgs e)
    {
        MenuOperations.OpenScale(sender, e, this);
    }

    protected void Preferences_OnClick(object? sender, System.EventArgs e)
    {
        MenuOperations.Preferences(sender, e, this);
    }

    protected void Quit_OnClick(object? sender, System.EventArgs e)
    {
        MenuOperations.Quit(sender, e, this);
    }

    protected void SaveClose_OnClick(object? sender, System.EventArgs e)
    {
        MenuOperations.SaveClose(sender, e, this);
    }
}
