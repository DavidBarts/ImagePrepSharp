namespace ImagePrepSharp.Data;

using System.IO;
using Serilog;

public static class Logging
{
    public static void Start()
    {
        if (!Directory.Exists(Files.DIRECTORY))
        {
            Directory.CreateDirectory(Files.DIRECTORY);
        }
        var logFile = Path.Join(Files.DIRECTORY, Files.MYNAME.ToLowerInvariant() + ".log");
        if (File.Exists(logFile))
        {
            File.Delete(logFile);
        }
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(logFile)
            .CreateLogger();
    }
}
