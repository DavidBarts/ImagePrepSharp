Why Do My Pictures Sometimes Show up Sideways?
==============================================

Executive Summary
-----------------

This happens because not all web browsers and web servers are aware of a
feature in image files from many digital cameras. The fix is to not post
image files with such features. This is what ImagePrepSharp enables you
to do, by converting images from your camera into a
lowest-common-denominator format that will work with any software.

The Details
-----------

Modern cameras contain sensors that tell their on-board computers which
way the camera is being held. When it captures an image, the camera
records which way it was oriented (portrait or landscape) in the
resulting file, but it always writes the image data itself in landscape
(larger dimension horizontal) format.

It is considered the responsibility of any program that displays images
to read the orientation information and use it to display the image
properly, by automatically rotating things if needed. Historically, many
web browsers in failed to read the orientation information; they simply
assumed that the horizontal dimension will always be horizontal
(because, prior to the new feature, it was).

Even if your browser displays such images properly, the web site the
image is uploaded to can also come into play. Many of these sites
postprocess the images you upload to them, and the postprocessing they
run is not always aware of the image orientation information. So even
a file that you can load and display correctly locally in your web
browser can suddenly appear sideways or upside-down after you upload
it!

The Solution
------------

The solution is to rotate the pixels themselves, and not to rely on
image orientation metadata being properly honoured. When you save an
image in ImagePrepSharp, it will be written in the same orientation that
it appears on the screen, even if the original contains image
orientation metadata.
