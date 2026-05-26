using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ImageMagick;
using Serilog;

namespace ImagePrepSharp.BackEnd;

// Tightly coupled to our needs; definitely not general-purpose.
public class BackEndImage : IDisposable
{
    private MagickImage? image;
    private string? path;

    public int Width { get => (int) image!.Width; }
    public int Height { get => (int) image!.Height; }
    public string Path {
        get {
            if (path == null)
            {
                throw new InvalidOperationException("Image has not been loaded.");
            }
            return path;
        }
    }

    public BackEndImage()
    {
        image = null;
        path = null;
    }

    public async Task LoadAsync(string fileName)
    {
        if (image != null)
        {
            throw new InvalidOperationException("Image has already been loaded.");
        }
        Log.Information("Opening {fileName}.", fileName);
        path = System.IO.Path.GetFullPath(fileName);
        image = await Task.Run(async delegate {
            return new MagickImage(fileName);
        });
    }

    private void AssertLoaded()
    {
        if (image == null)
        {
            throw new InvalidOperationException("Image has not been loaded.");
        }
    }

    // this one should also strip metadata, if needed to produce clean output
    public async Task<BackEndImage> ScaleMapColorAsync(int maxDimension)
    {
        AssertLoaded();
        await Task.Run(async delegate {
            var imageMaxDimension = uint.Max(image!.Width, image.Height);
            if (imageMaxDimension > maxDimension)
            {
                Log.Information("Resizing.");
                var ratio = (double) maxDimension / (double) imageMaxDimension;
                image.Resize((uint) (image.Width * ratio), (uint) (image.Height * ratio), FilterType.Lanczos);
            }
            var colorProfile = image.GetColorProfile();
            if (colorProfile == null)
            {
                Log.Information("No color profile, assuming sRGB.");
                image.SetProfile(ColorProfiles.SRGB);
            }
            else if (colorProfile != ColorProfiles.SRGB)
            {
                Log.Information("Transforming color space to sRGB.");
                image.TransformColorSpace(ColorProfiles.SRGB);
            }
            StripMetadata();
        });
        return this;
    }

    private void StripMetadata()
    {
        var exifProfile = image!.GetExifProfile();
        if (exifProfile != null)
            image.RemoveProfile(exifProfile);
        var iptcProfile = image.GetIptcProfile();
        if (iptcProfile != null)
            image.RemoveProfile(iptcProfile);
        var xmpProfile = image.GetXmpProfile();
        if (xmpProfile != null)
            image.RemoveProfile(xmpProfile);
    }

    public async Task<BackEndImage> RotateAsync(int degrees)
    {
        AssertLoaded();
        await Task.Run(async delegate {
            image!.Rotate(degrees);
        });
        return this;
    }

    public async Task SaveAsync(string fileName, int quality)
    {
        AssertLoaded();
        Log.Information("Saving as {fileName}.", fileName);
        await Task.Run(async delegate {
            image!.Quality = (uint) quality;
            image.Write(fileName);
        });
    }

    public void Dispose()
    {
        image?.Dispose();
    }

    public void DisposeIfDifferentFrom(BackEndImage other)
    {
        if (this != other)
        {
            Dispose();
        }
    }

    public Bitmap ToBitmap()
    {
        AssertLoaded();
        return image!.ToWriteableBitmap();
    }
}
