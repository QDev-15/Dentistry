using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using ImageProcessing.Documents;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace ImageProcessing.Barcodes
{
    public enum BarcodeValueComparison
    {
        StartsWith,
        EndsWith,
        Contains,
        Exact
    }

    /// <summary>One barcode symbology + expected value the caller wants to search for.</summary>
    public sealed class BarcodeSearchTarget
    {
        public BarcodeSearchTarget(BarcodeKind kind, BarcodeValueComparison comparison, string compareValue)
        {
            Kind = kind;
            Comparison = comparison;
            CompareValue = compareValue ?? string.Empty;
        }

        public BarcodeKind Kind { get; }

        public BarcodeValueComparison Comparison { get; }

        public string CompareValue { get; }

        public bool Accepts(string decodedValue)
        {
            string value = decodedValue ?? string.Empty;
            switch (Comparison)
            {
                case BarcodeValueComparison.StartsWith: return value.StartsWith(CompareValue);
                case BarcodeValueComparison.EndsWith: return value.EndsWith(CompareValue);
                case BarcodeValueComparison.Contains: return value.Contains(CompareValue);
                case BarcodeValueComparison.Exact: return value == CompareValue;
                default: return false;
            }
        }
    }

    /// <summary>A decoded barcode.</summary>
    public sealed class BarcodeMatch
    {
        public BarcodeMatch(BarcodeKind kind, string value)
        {
            Kind = kind;
            Value = value ?? string.Empty;
        }

        public BarcodeKind Kind { get; }

        public string Value { get; }
    }

    /// <summary>Reads 1D barcodes from the first page of an image file (ZXing.Net).</summary>
    public static class BarcodeScanner
    {
        /// <summary>Decodes every barcode on the page whose format matches one of <paramref name="kinds"/>.</summary>
        public static IReadOnlyList<BarcodeMatch> ReadAll(string imagePath, IEnumerable<BarcodeKind> kinds, bool tryHarder = false)
        {
            List<BarcodeKind> kindList = kinds?.ToList() ?? new List<BarcodeKind>();
            Result[] results = Decode(imagePath, kindList, tryHarder);
            if (results == null || results.Length == 0)
                return Array.Empty<BarcodeMatch>();

            List<BarcodeMatch> matches = new List<BarcodeMatch>();
            foreach (Result r in results)
            {
                foreach (BarcodeKind kind in kindList)
                {
                    if (BarcodeCodecMap.Matches(kind, r.BarcodeFormat))
                    {
                        matches.Add(new BarcodeMatch(kind, r.Text));
                        break;
                    }
                }
            }
            return matches;
        }

        /// <summary>
        /// Scans for the configured <paramref name="targets"/> in order and returns the first
        /// whose barcode kind AND value comparison both match. Returns null when nothing matches.
        /// </summary>
        public static BarcodeMatch FindFirstMatch(string imagePath, IEnumerable<BarcodeSearchTarget> targets, bool tryHarder = false)
        {
            List<BarcodeSearchTarget> targetList = targets?.ToList() ?? new List<BarcodeSearchTarget>();
            if (targetList.Count == 0) return null;

            Result[] results = Decode(imagePath, targetList.Select(t => t.Kind), tryHarder);
            if (results == null || results.Length == 0)
                return null;

            foreach (BarcodeSearchTarget target in targetList)
            {
                foreach (Result r in results)
                {
                    if (!BarcodeCodecMap.Matches(target.Kind, r.BarcodeFormat))
                        continue;
                    string value = r.Text ?? string.Empty;
                    if (target.Accepts(value))
                        return new BarcodeMatch(target.Kind, value);
                }
            }
            return null;
        }

        private static Result[] Decode(string imagePath, IEnumerable<BarcodeKind> kinds, bool tryHarder)
        {
            using (Bitmap page = ImageEffectFileOpsBridge.LoadFirstPageBitmap(imagePath))
            {
                BarcodeReaderGeneric reader = new BarcodeReaderGeneric
                {
                    AutoRotate = true,
                    Options = new DecodingOptions
                    {
                        TryHarder = tryHarder,
                        PossibleFormats = BarcodeCodecMap.DistinctReadFormats(kinds)
                    }
                };
                LuminanceSource source = ToLuminanceSource(page);
                return reader.DecodeMultiple(source);
            }
        }

        /// <summary>
        /// Builds a ZXing luminance source directly from a Bitmap, without the
        /// ZXing.Windows.Compatibility package.
        /// </summary>
        private static LuminanceSource ToLuminanceSource(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            byte[] rgb = new byte[width * height * 3];

            using (Bitmap rgbBmp = bmp.PixelFormat == PixelFormat.Format24bppRgb
                ? bmp
                : new Bitmap(bmp).Clone(new Rectangle(0, 0, width, height), PixelFormat.Format24bppRgb))
            {
                BitmapData data = rgbBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                try
                {
                    int stride = data.Stride;
                    byte[] line = new byte[stride];
                    for (int y = 0; y < height; y++)
                    {
                        Marshal.Copy(data.Scan0 + y * stride, line, 0, stride);
                        for (int x = 0; x < width; x++)
                        {
                            int si = x * 3; // BGR in GDI+
                            int di = (y * width + x) * 3;
                            rgb[di] = line[si + 2];
                            rgb[di + 1] = line[si + 1];
                            rgb[di + 2] = line[si];
                        }
                    }
                }
                finally
                {
                    rgbBmp.UnlockBits(data);
                }
            }

            return new RGBLuminanceSource(rgb, width, height, RGBLuminanceSource.BitmapFormat.RGB24);
        }
    }

    /// <summary>Renders a 1D barcode and composes it onto a page image.</summary>
    public static class BarcodeStamp
    {
        /// <summary>
        /// Renders <paramref name="value"/> as a <paramref name="kind"/> barcode filling
        /// <paramref name="zone"/> and draws it onto a copy of <paramref name="page"/>.
        /// </summary>
        public static Bitmap Compose(Bitmap page, BarcodeKind kind, string value, Rectangle zone)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Barcode value is empty.", nameof(value));
            if (zone.Width <= 0 || zone.Height <= 0) throw new ArgumentException("Barcode zone has no area.", nameof(zone));

            BarcodeWriterPixelData writer = new BarcodeWriterPixelData
            {
                Format = BarcodeCodecMap.ToWriteFormat(kind),
                Options = new EncodingOptions
                {
                    Width = zone.Width,
                    Height = zone.Height,
                    Margin = 0,
                    PureBarcode = false
                }
            };

            PixelData pixelData = writer.Write(value);

            Bitmap result = new Bitmap(page);
            result.SetResolution(page.HorizontalResolution, page.VerticalResolution);
            using (Bitmap barcodeBmp = PixelDataToBitmap(pixelData))
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImageUnscaled(barcodeBmp, zone.Left, zone.Top);
            return result;
        }

        private static Bitmap PixelDataToBitmap(PixelData pixelData)
        {
            Bitmap bmp = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            try
            {
                Marshal.Copy(pixelData.Pixels, 0, data.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bmp.UnlockBits(data);
            }
            return bmp;
        }
    }

    /// <summary>Bridges to the internal first-page loader without exposing it publicly.</summary>
    internal static class ImageEffectFileOpsBridge
    {
        public static Bitmap LoadFirstPageBitmap(string path)
        {
            if (TiffCodec.IsReadableTiff(path) && TiffCodec.IsMultiPage(path))
                return TiffCodec.ReadPage(path, 0);
            return new Bitmap(path);
        }
    }
}
