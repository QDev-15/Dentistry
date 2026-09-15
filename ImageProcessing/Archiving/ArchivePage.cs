using System;
using System.Collections.Generic;
using System.Drawing;

namespace ImageProcessing.Archiving
{
    /// <summary>The image compression of an <see cref="ArchivePage"/>'s encoded bytes.</summary>
    public enum PageCodec
    {
        /// <summary>Baseline JPEG, embedded as-is via /DCTDecode.</summary>
        Jpeg,

        /// <summary>CCITT Group 4 (fax) bitonal, embedded via /CCITTFaxDecode.</summary>
        CcittGroup4,

        /// <summary>JPEG 2000 (JP2/J2K), embedded via /JPXDecode. Requires PDF/A-2 or later.</summary>
        Jpeg2000,

        /// <summary>Zlib/deflate-compressed raw samples, embedded via /FlateDecode.</summary>
        Flate
    }

    /// <summary>The colour interpretation of an <see cref="ArchivePage"/>'s samples.</summary>
    public enum PageColorSpace
    {
        /// <summary>1-bit black/white (DeviceGray, 1 bpc).</summary>
        Bitonal,

        /// <summary>8-bit grayscale (DeviceGray).</summary>
        Gray,

        /// <summary>24-bit colour (DeviceRGB).</summary>
        Rgb,

        /// <summary>32-bit colour (DeviceCMYK). Needs a CMYK OutputIntent/ICC.</summary>
        Cmyk
    }

    /// <summary>One recognized OCR word, to be drawn as invisible text over its printed position.</summary>
    public sealed class OcrWord
    {
        /// <param name="text">Recognized word text.</param>
        /// <param name="boundingBox">
        /// Word box in image pixel coordinates, top-left origin (X right, Y down) - the native
        /// Tesseract/hOCR frame. The archiver converts to PDF (bottom-left, points) using the
        /// page DPI.
        /// </param>
        /// <param name="baseline">Optional baseline Y (image pixels, top-left origin); null = approximate from the box bottom.</param>
        public OcrWord(string text, RectangleF boundingBox, float? baseline = null)
        {
            Text = text ?? string.Empty;
            BoundingBox = boundingBox;
            Baseline = baseline;
        }

        public string Text { get; }

        public RectangleF BoundingBox { get; }

        public float? Baseline { get; }
    }

    /// <summary>
    /// One prepared page for <see cref="IPdfArchiver"/>: already-encoded image bytes (kept
    /// verbatim so the archiver embeds them without re-encoding) plus geometry and, optionally,
    /// OCR word boxes for the invisible text layer.
    /// </summary>
    public sealed class ArchivePage
    {
        public ArchivePage(
            byte[] imageData,
            PageCodec codec,
            int widthPx,
            int heightPx,
            PageColorSpace colorSpace,
            double dpiX = 200,
            double dpiY = 200,
            IReadOnlyList<OcrWord> ocrWords = null)
        {
            if (imageData == null) throw new ArgumentNullException(nameof(imageData));
            if (widthPx <= 0) throw new ArgumentOutOfRangeException(nameof(widthPx));
            if (heightPx <= 0) throw new ArgumentOutOfRangeException(nameof(heightPx));

            ImageData = imageData;
            Codec = codec;
            WidthPx = widthPx;
            HeightPx = heightPx;
            ColorSpace = colorSpace;
            DpiX = dpiX > 0 ? dpiX : 200;
            DpiY = dpiY > 0 ? dpiY : 200;
            OcrWords = ocrWords ?? Array.Empty<OcrWord>();
        }

        public byte[] ImageData { get; }

        public PageCodec Codec { get; }

        public int WidthPx { get; }

        public int HeightPx { get; }

        public PageColorSpace ColorSpace { get; }

        public double DpiX { get; }

        public double DpiY { get; }

        public IReadOnlyList<OcrWord> OcrWords { get; }

        public bool HasText { get { return OcrWords.Count > 0; } }

        public double WidthPt { get { return WidthPx / DpiX * 72.0; } }

        public double HeightPt { get { return HeightPx / DpiY * 72.0; } }
    }
}
