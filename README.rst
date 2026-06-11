ImagePrepSharp
==============

Introduction
------------

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

And when you share it, the picture will always show right side up. No
more surprises with unexpected sideways or upside-down images that
looked just fine on your computer. (If you are curious why this sometimes
happens, click `here <sideways.rst>`_.)

Installing
----------

The easiest thing is to use
`Parcel <https://docs.avaloniaui.net/tools/parcel/setup>`_. It can make
a standard, clickable application for most desktop environments.

If you are installing on a Mac, there is an ``Info.plist`` file that
registers ImagePrepSharp as an editor for most image file formats; this
makes editing images quite a bit easier (you can, for example, request
that the standard Apple Photos app use ImagePrepSharp to edit an image).
There is also an ``Image.parcel`` file ready to use. As an alternative,
you can run the ``make-mac-app`` script to make a Mac application
bundle. If you choose the latter option, reading
`this <https://avaloniaui.net/blog/the-definitive-guide-to-building-and-deploying-avalonia-applications-for-macos>`_
should be helpful.

If you are installing on Linux, check out the ``package-files/linux``
directory for an icon and other Linux-related files. I currently lack
access to desktop systems other than this Mac, and contributions from
others for getting things more supported for Linux and Windows would
be most appreciated.

It should not be necessary to purchase the Plus or Pro versions of
Avalonia; this program is small enough to easily fit within the size
limitations of the Community version.

One thing of note is Magick.NET. It’s mostly written in C++ and compiled
to machine code. As such, the NuGet package you need depends on your
hardware and operating system. The
`GitHub README <https://github.com/dlemstra/Magick.NET>`_
for Magick.NET has a list of the packages available. Note that an 8-bit
(Q8) package without HDRI support is sufficient.

Installing on a Mac
^^^^^^^^^^^^^^^^^^^

Since I used a Mac to code and build this, I have included both arm64
and x64 app bundles in the ``dist`` subdirectory.

Running
-------

It should be mostly self-explanatory; this is not a complex program.

#. Open the image using File… Open & Scale. As this command’s name
   implies, it will also scale the file to a maximum of 640 (or whatever
   the defined maximum is) pixels in either dimension, preserving its
   aspect ratio.
#. Rotate the image as needed.
#. Either write out the file and close it using File… Save & Close, or
   discard your edits using File… Discard.

Settings
^^^^^^^^

Most of what this program does is controlled by various settings.

**Default maximum dimension.** This sets the default value for the maximum
dimension choice. When an image is loaded, both its width and its height
(in pixels) are compared to the maximum dimension. If either is greater,
the image is proportionally downsampled using the Lanczos 3 algorithm so
as to fit within this constraint.

**Output filename suffix.** When saving, a default filename will be
suggested. The suggested name will be the input name (sans extension),
plus this suffix, with an extension corresponding to the chosen default
output type (q.v.). If the suffix contains ``{0}`` or ``{1}``, those
sequences will be replaced by the width and the height respectively of
the output file. For example, for an output type of JPEG, an output
suffix of ``_{0}x{1}`` and an input file of ``IMG_1234.JPG``, the
suggested output filename would be something like
``IMG_1234_640x480.jpeg``.

**Default output quality.** Both JPEG and WebP images are stored in a
compressed form. The compression is lossy; i.e. a compressed image is
not 100% identical to the original (the compression is, however,
engineered so that the differences are typically hard to spot). Output
quality controls how much this compression is permitted to alter the
image, and runs from 1 (least faithful) to 100 (most faithful).

**Default output location.** Depending on the configuration, the file
chooser for saving will open up in either the directory containing the
input file, or in the specified output directory.

**Default output type.** This controls the default extension (and by
implication the default image file format) for the output file.

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
