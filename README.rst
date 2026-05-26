ImagePrepSharp
==============

This is a C#/Avalonia port of the ImagePrep utility I wrote in Kotlin in
2020, whose source code is self-hosted elsewhere (I will not link it
here because abusive crawling from AI bots has crashed my site more than
once, but you can find it by chasing the link from my Github home page).

Why port it? Because bitter experience has taught me the JVM is not the
best platform on which to do general-purpose image processing. The stock
``java.awt.image.BufferedImage`` and ``javax.imageio.ImageIO`` classes
are quite limited in the file formats they support (HEIC, used by
iPhones, is not supported at all), the operations they can do out of the
box, and in their capability to deal with large files.  And there is no
good alternative; there is nothing comparable to Python’s
`Pillow <https://pillow.readthedocs.io/en/stable/>`_ or .NET’s
`Magick.NET <https://github.com/dlemstra/Magick.NET>`_ for the JVM. What’s
there is either geared twoards strange specialty purposes, an expensive
proprietary product, limited in functionality, abandonware, or some
combination of the above. I know, I’ve looked.

I didn’t want to write it in C++, JavaScript, or TypeScript, I don’t
like PyQt/PySide, and wxPython is looking more and more like it will
soon be abandonware. So C# it was, and given that I want a GUI program I
can compile and run on all three of Windows, Mac, and Linux, Avalonia it
also was.

More information is hopefully coming soon.
