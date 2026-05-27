ImagePrepSharp
==============

Introduction
------------

**NOTE:** This project is very much a work in progress right now.

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
there is either geared towards strange specialty purposes, an expensive
proprietary product, limited in functionality, abandonware, or some
combination of the above. I know, I’ve looked.

I didn’t want to write it in C++, JavaScript, or TypeScript, I don’t
like PyQt/PySide, and wxPython is looking more and more like it will
soon be abandonware. So C# it was, and given that I want a GUI program I
can compile and run on all three of Windows, Mac, and Linux, Avalonia it
also was.

What This Program Does
----------------------

Executive summary: It prepares images for being shared electronically
by minimizing their size, stripping out unnecessary information, and
ensuring they will always display right side up.

It opens an image file (it can handle any format Image Magick can, which
is a whole lot of formats; this includes the HEIC files produced by
iPhones but does not include camera raw files). It prompts for a desired
maximum resolution (i.e. the resolution of the height or the width,
whichever is greater), and downsamples the image to that resolution if
needed. If the color space is not sRGB, it converts it to sRGB. Most
metadata (except for the color profile) is deleted; this includes
metadata revealing your camera model and serial number, and details of
the image you shot.

Then it displays the image and lets you rotate it so it is right side
up, if needed. After possibly rotating the image, the it can be saved in
JPEG or WebP format for sharing online.

Installing
----------

(This section is incomplete.)

One thing of note is Magick.NET. It’s mostly written in C++ and compiled
to machine code. As such, the NuGet package you need depends on your
hardware and operating system. The
`GitHub README <https://github.com/dlemstra/Magick.NET>`_
for Magick.NET has a list of the packages available. Note that an 8-bit
(Q8) package without HDRI support is sufficient.

Running
-------

It should be mostly self-explanatory; this is not a complex program. One
can open image files either from the menu at top, or via dragging them
onto the main window. Rotation, if needed, is accomplished by three
buttons in an editing window.  Saving is via the menu at top. Nothing is
done to stop you from exiting with unsaved image changes, because the
image editing options offered are so simple (limited to rotation) that
it did not seem worthwhile.

A number of settings control the default operation of this program;
these may be adjusted via a settings dialog brought up via the top menu.
Of particular note here is that the output suffix can contain ``{0}``
and ``{1}`` sequences, which will be replaced by the width and height
of the image respectively.

Why Not MVVM?
-------------

If you look at the source code, you will see that I did not use the MVVM
pattern.  It just didn’t make sense to me to use it. Sure, it increases
the fraction of code that can be unit tested, but only by a very modest
amount, and at a significant cost in complexity. I just didn’t find the
trade-off worth it. Even the inventor of the MVVM pattern has stated it
`“can be overkill” <https://learn.microsoft.com/en-us/archive/blogs/johngossman/advantages-and-disadvantages-of-m-v-vm>`_
in smaller projects.

Instead, I separated the code base into stuff that deals with user
interaction (``FrontEnd``), stuff that deals with back-end processing
(``BackEnd``), and data access code (``Data``). The result ended up,
perhaps expectedly, being very front-end heavy. There is actually a lot
of back end processing going on, but most of that happens in the
image-processing library, not in my code.
