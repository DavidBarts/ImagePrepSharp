namespace ImagePrepSharp.Data;

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

[JsonSerializable(typeof(Settings))]
internal partial class SourceGenerationContext : JsonSerializerContext { }

// This does not use async I/O, because a) it is easier, and b) the settings
// file is small and should load/save quickly. Hardware errors can violate
// these assumptions, but if those are happening, lots of things are going
// to get slow and freezy.
public class Settings
{
    public static readonly string FILE_NAME = Path.Join(Files.DIRECTORY, "settings.json");
    private static readonly Settings _instance = GetSettings();

    public string OutputSuffix { get; set; } = "_scaled";
    public string OutputTo { get; set; } = OperatingSystem.IsMacOS() || OperatingSystem.IsWindows() ? Path.Join(Files.HOME, "Documents") : Files.HOME;
    public OutputType OutputType { get; set; } = OutputType.JPEG;
    public int OutputQuality { get; set; } = 85;
    public bool OutputToInputDir { get; set; } = false;
    public int MaxDimension { get; set; } = 640;

    public static Settings Instance
    {
        get => _instance;
    }

    // XXX - should be private, but JsonSerializer.Deserialize wants this
    // to be public.
    public Settings()
    {
    }

    private static Settings GetSettings()
    {
        if (File.Exists(FILE_NAME))
        {
            using var stream = File.Open(FILE_NAME, FileMode.Open, FileAccess.Read);
            return JsonSerializer.Deserialize(stream, SourceGenerationContext.Default.Settings)!;
        }
        else
        {
            return new();
        }
    }

    public void Save()
    {
        if (!Directory.Exists(Files.DIRECTORY))
        {
            Directory.CreateDirectory(Files.DIRECTORY);
        }
        using var stream = File.Open(FILE_NAME, FileMode.Create, FileAccess.Write);
        JsonSerializer.Serialize(stream, this, SourceGenerationContext.Default.Settings);
    }
}

public enum OutputType
{
     JPEG,
     WEBP
}
