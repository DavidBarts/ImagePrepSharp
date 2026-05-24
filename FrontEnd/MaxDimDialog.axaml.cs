using System;
using System.Threading.Tasks;
using Avalonia.Controls;

using ImagePrepSharp.Data;

namespace ImagePrepSharp.FrontEnd;
public partial class MaxDimDialog : Window
{
    public int Value {
        get => MaxDimSpinner.Value;
        set { MaxDimSpinner.Value = value; }
    }

    public MaxDimDialog()
    {
        InitializeComponent();
    }

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(null);
    }

    private void OK_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(Value);
    }

    public async Task<int?> ShowModalAsync(Window parent)
    {
        return await ShowDialog<int?>(parent);
    }
}
