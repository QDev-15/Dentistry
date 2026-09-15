using System;
using System.Drawing;
using ImageProcessing.Documents;
using ImageProcessing.Imaging;
using OpenCvSharp;

namespace ImageProcessing.Assembly
{
    /// <summary>The colour depth a scanned/processed page is stored as.</summary>
    public enum ScanColorMode
    {
        BlackAndWhite,
        Gray,
        Color
    }

    /// <summary>
    /// Writes a processed page to disk in the format its <see cref="ScanColorMode"/> dictates.
    /// Internal: works in terms of OpenCvSharp's <see cref="Mat"/>, kept out of the public surface
    /// so consumers never need an OpenCvSharp reference just to call this library.
    /// </summary>
    internal static class ImagePageWriter
    {
        /// <summary>Black &amp; white -> single-page CCITT Group 4 TIFF; Gray/Color -> JPEG.</summary>
        public static void Save(Mat image, string destPath, ScanColorMode colorMode, long jpegQuality = 75)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (string.IsNullOrEmpty(destPath)) throw new ArgumentException("destPath is required.", nameof(destPath));

            switch (colorMode)
            {
                case ScanColorMode.BlackAndWhite:
                    using (Mat bitonal = PixelOps.ToOtsuBitonal(image))
                        TiffCodec.SaveAsGroup4Tiff(bitonal, destPath);
                    break;

                case ScanColorMode.Gray:
                    using (Bitmap gray = PixelOps.ToGrayscaleBitmap(image))
                        JpegCodec.SaveToFile(gray, destPath, jpegQuality);
                    break;

                default: // Color
                    using (Bitmap bmp = PixelOps.ToManagedBitmap(image))
                        JpegCodec.SaveToFile(bmp, destPath, jpegQuality);
                    break;
            }
        }
    }
}
