namespace ImagePrepSharp.FrontEnd;

using System;
using Avalonia.Controls;
using ImagePrepSharp.Data;

public class OutQualSpinner : NumericUpDown
{
    protected override Type StyleKeyOverride => typeof(NumericUpDown);

    public new int Value {
        get => (int) base.Value!;
        set { base.Value = value; }
    }
    
    public OutQualSpinner() : base()
    {
        Value = Settings.Instance.OutputQuality;
        Minimum = 1.0m;
        Maximum = 100.0m;
        FormatString = "F0";
    }
}
