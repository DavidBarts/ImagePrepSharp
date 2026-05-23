namespace ImagePrepSharp.Data;

using System;
using System.IO;

public static class Files
{
    public const string MYNAME = "ImagePrepSharp";
    public static readonly string HOME = OperatingSystem.IsWindows() ? Environment.GetEnvironmentVariable("USERPROFILE")! : Environment.GetEnvironmentVariable("HOME")!;
    public static readonly string DIRECTORY =
        OperatingSystem.IsMacOS() ? Path.Join(Environment.GetEnvironmentVariable("HOME"), Path.Join("Library", Path.Join("Application Support", MYNAME))) :
        OperatingSystem.IsWindows() ? Path.Join(Environment.GetEnvironmentVariable("APPDATA"), MYNAME) :
        Path.Join(Environment.GetEnvironmentVariable("HOME"), "." + MYNAME);

}
