namespace ImagePrepSharp.FrontEnd;

using System;
using Avalonia.Controls;
using ImagePrepSharp.Data;

public class MaxDimSpinner : ButtonSpinner
{
    private static readonly int[] DIMENSIONS = [320, 400, 512, 640, 800, 1024, 1280, 1600];

    private int index;

    protected override Type StyleKeyOverride => typeof(ButtonSpinner);

    public int Value {
        get => DIMENSIONS[index];
        set {
            var _index = Array.IndexOf(DIMENSIONS, value);
            if (_index < 0)
            {
                throw new ArgumentException($"Invalid maximum dimension: {value}.");
            }
            index = _index;
            ((TextBlock) Content!).Text = value.ToString();
        }
    }
    
    public MaxDimSpinner() : base()
    {
        Content = new TextBlock();
        Value = Settings.Instance.MaxDimension;
        Spin += OnSpin;
    }

    private void OnSpin(object? sender, SpinEventArgs e)
    {
        index += e.Direction == SpinDirection.Increase ? 1 : -1;
        if (index < 0)
        {
            index = DIMENSIONS.Length - 1;
        }
        else if (index >= DIMENSIONS.Length)
        {
            index = 0;
        }
        ((TextBlock) Content!).Text = Value.ToString();
    }
}
