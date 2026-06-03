using Avalonia;
using System;
using Serilog;

using ImagePrepSharp.Data;
using Avalonia.Media;

namespace ImagePrepSharp;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args){
        Logging.Start();
        Log.Information("Execution begins.");
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .WithSystemFonts()
            .UsePlatformDetect()
            .LogToTrace();
}

public static class AppBuilderExtensions
{
    extension(AppBuilder builder)
    {
        public AppBuilder WithSystemFonts()
        {
            if (OperatingSystem.IsMacOS())
            {
                return builder.With(new FontManagerOptions { DefaultFamilyName = ".AppleSystemUIFont" });
            }
            if (OperatingSystem.IsWindows())
            {
                return builder.With(new FontManagerOptions { DefaultFamilyName = "Segoe UI" });
            }
            return builder;
        }
    }
}
