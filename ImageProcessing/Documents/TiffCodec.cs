using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using BitMiracle.LibTiff.Classic;
using ImageProcessing.Imaging;
using OpenCvSharp;

namespace ImageProcessing.Documents
{
    /// <summary>
    /// TIFF page counting, extraction and CCITT Group 4 (fax) encode/decode, built on
    /// BitMiracle.LibTiff.NET.
    /// </summary>
    internal static class TiffCodec
    {
        public static bool HasTiffExtension(string path)
        {
            string ext = Path.GetExtension(path);
            return string.Equals(ext, ".tif", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ext, ".tiff", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsReadableTiff(string path)
        {
            if (!HasTiffExtension(path))
                return false;
            using (Tiff t = Tiff.Open(path, "r"))
                return t != null;
        }

        public static bool IsMultiPage(string path)
        {
            return GetPageCount(path) > 1;
        }

        public static int GetPageCount(string path)
        {
            using (Tiff tiff = Tiff.Open(path, "r"))
            {
                if (tiff == null) return 0;
                int count = 0;
                do { count++; } while (tiff.ReadDirectory());
                return count;
            }
        }

        /// <summary>Reads a single 0-based frame of a (possibly multi-page) TIFF via GDI+.</summary>
        public static Bitmap ReadPage(string path, int zeroBasedPage)
        {
            using (Image img = Image.FromFile(path))
            {
                FrameDimension dim = new FrameDimension(img.FrameDimensionsList[0]);
                int total = img.GetFrameCount(dim);
                int index = Math.Max(0, Math.Min(zeroBasedPage, total - 1));
                img.SelectActiveFrame(dim, index);
                return new Bitmap(img);
            }
        }

        /// <summary>Extracts every page of a (multi-page) TIFF as individual bitmaps.</summary>
        public static Bitmap[] ReadAllPages(string path)
        {
            using (Image img = Image.FromFile(path))
            {
                FrameDimension dim = new FrameDimension(img.FrameDimensionsList[0]);
                int total = img.GetFrameCount(dim);
                Bitmap[] pages = new Bitmap[total];
                for (int i = 0; i < total; i++)
                {
                    img.SelectActiveFrame(dim, i);
                    pages[i] = new Bitmap(img);
                }
                return pages;
            }
        }

        /// <summary>Extracts a single 1-based page from a multi-page TIFF into its own file.</summary>
        public static void ExtractPageToFile(string sourceTiff, int pageNumber1Based, string destFile)
        {
            using (Bitmap page = ReadPage(sourceTiff, pageNumber1Based - 1))
            {
                if (page.PixelFormat == PixelFormat.Format1bppIndexed)
                    File.WriteAllBytes(destFile, EncodeGroup4(page));
                else
                    page.Save(destFile, ImageFormat.Tiff);
            }
        }

        /// <summary>
        /// Writes a bitonal <see cref="Mat"/> (as produced by <see cref="PixelOps.ToOtsuBitonal"/>:
        /// 0 = ink, 255 = paper) as a single-strip CCITT Group 4, MinIsWhite TIFF.
        /// </summary>
        public static void SaveAsGroup4Tiff(Mat bitonal, string destFile, int dpiX = 200, int dpiY = 200)
        {
            using (Tiff tiff = Tiff.Open(destFile, "w"))
            {
                if (tiff == null)
                    throw new IOException("Cannot open TIFF for writing: " + destFile);
                WriteGroup4Directory(tiff, bitonal, dpiX, dpiY);
            }
        }

        /// <summary>Writes several already-bitonal pages as one multi-page CCITT Group 4 TIFF.</summary>
        public static void SaveMultiPageGroup4Tiff(System.Collections.Generic.IEnumerable<Mat> bitonalPages, string destFile, int dpiX = 200, int dpiY = 200)
        {
            using (Tiff tiff = Tiff.Open(destFile, "w"))
            {
                if (tiff == null)
                    throw new IOException("Cannot open TIFF for writing: " + destFile);

                int count = 0;
                foreach (Mat page in bitonalPages)
                {
                    WriteGroup4Directory(tiff, page, dpiX, dpiY);
                    count++;
                }
                if (count == 0)
                    throw new InvalidOperationException("No pages supplied.");
            }
        }

        private static void WriteGroup4Directory(Tiff tiff, Mat bitonalMat, int dpiX, int dpiY)
        {
            int width = bitonalMat.Width;
            int height = bitonalMat.Height;
            ConfigureGroup4Tags(tiff, width, height, dpiX, dpiY);

            int rowBytes = (width + 7) / 8;
            byte[] row = new byte[rowBytes];
            long stride = bitonalMat.Step();
            IntPtr data = bitonalMat.Data;

            for (int y = 0; y < height; y++)
            {
                Array.Clear(row, 0, rowBytes);
                IntPtr rowPtr = IntPtr.Add(data, (int)(y * stride));
                for (int x = 0; x < width; x++)
                {
                    // Mat convention here: 0 = ink. MinIsWhite convention on the wire: 1 = ink.
                    if (Marshal.ReadByte(IntPtr.Add(rowPtr, x)) == 0)
                        row[x / 8] |= (byte)(0x80 >> (x % 8));
                }
                tiff.WriteScanline(row, y);
            }
            tiff.WriteDirectory();
        }

        private static void ConfigureGroup4Tags(Tiff tiff, int width, int height, int dpiX, int dpiY)
        {
            tiff.SetField(TiffTag.IMAGEWIDTH, width);
            tiff.SetField(TiffTag.IMAGELENGTH, height);
            tiff.SetField(TiffTag.BITSPERSAMPLE, 1);
            tiff.SetField(TiffTag.SAMPLESPERPIXEL, 1);
            tiff.SetField(TiffTag.ROWSPERSTRIP, height);
            tiff.SetField(TiffTag.XRESOLUTION, (float)dpiX);
            tiff.SetField(TiffTag.YRESOLUTION, (float)dpiY);
            tiff.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.INCH);
            tiff.SetField(TiffTag.COMPRESSION, Compression.CCITTFAX4);
            tiff.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISWHITE);
            tiff.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);
            tiff.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
        }

        /// <summary>
        /// If the given 0-based page is already a single-strip CCITT Group 4 image, returns its
        /// raw codestream verbatim (for embedding via /CCITTFaxDecode) plus geometry/DPI. Returns
        /// false for any other layout so the caller can re-encode via <see cref="EncodeGroup4"/>.
        /// </summary>
        public static bool TryGetRawGroup4(string path, int zeroBasedPage, out byte[] g4, out int width, out int height, out int dpiX, out int dpiY)
        {
            g4 = null; width = 0; height = 0; dpiX = 200; dpiY = 200;

            using (Tiff tiff = Tiff.Open(path, "r"))
            {
                if (tiff == null) return false;
                if (!tiff.SetDirectory((short)zeroBasedPage)) return false;

                FieldValue[] compression = tiff.GetField(TiffTag.COMPRESSION);
                if (compression == null || (Compression)compression[0].ToInt() != Compression.CCITTFAX4)
                    return false;
                if (tiff.NumberOfStrips() != 1)
                    return false;

                width = tiff.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                height = tiff.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                ReadResolution(tiff, out dpiX, out dpiY);

                int size = (int)tiff.RawStripSize(0);
                byte[] buffer = new byte[size];
                int read = tiff.ReadRawStrip(0, buffer, 0, size);
                if (read <= 0) return false;
                if (read != size) Array.Resize(ref buffer, read);
                g4 = buffer;
                return true;
            }
        }

        /// <summary>
        /// Encodes a 1bpp-indexed <see cref="Bitmap"/> (of unknown, per-file bit polarity - see
        /// <see cref="BitonalPolarityDetector"/>) to a raw single-strip CCITT Group 4 codestream
        /// (MinIsWhite semantics, ready for /CCITTFaxDecode with /BlackIs1 false).
        /// </summary>
        public static byte[] EncodeGroup4(Bitmap bitonal)
        {
            BitonalPolarity polarity = BitonalPolarityDetector.Detect(bitonal);
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".tif");
            try
            {
                using (Tiff tiff = Tiff.Open(tempFile, "w"))
                {
                    if (tiff == null) throw new IOException("Cannot open temp TIFF for Group 4 encode.");
                    WriteGroup4DirectoryFromIndexedBitmap(tiff, bitonal, polarity);
                    tiff.WriteDirectory();
                }
                using (Tiff tiff = Tiff.Open(tempFile, "r"))
                {
                    int size = (int)tiff.RawStripSize(0);
                    byte[] buffer = new byte[size];
                    int read = tiff.ReadRawStrip(0, buffer, 0, size);
                    if (read != size) Array.Resize(ref buffer, read);
                    return buffer;
                }
            }
            finally
            {
                try { if (File.Exists(tempFile)) File.Delete(tempFile); } catch { /* best effort */ }
            }
        }

        private static void WriteGroup4DirectoryFromIndexedBitmap(Tiff tiff, Bitmap bitonal, BitonalPolarity polarity)
        {
            int width = bitonal.Width;
            int height = bitonal.Height;
            int dpiX = ResolveDpi(bitonal.HorizontalResolution);
            int dpiY = ResolveDpi(bitonal.VerticalResolution);
            ConfigureGroup4Tags(tiff, width, height, dpiX, dpiY);

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData src = bitonal.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);
            try
            {
                int rowBytes = (width + 7) / 8;
                byte[] source = new byte[src.Stride];
                byte[] outRow = new byte[rowBytes];
                bool zeroIsInk = polarity == BitonalPolarity.ZeroIsInk;

                for (int y = 0; y < height; y++)
                {
                    Marshal.Copy(IntPtr.Add(src.Scan0, y * src.Stride), source, 0, src.Stride);
                    Array.Clear(outRow, 0, rowBytes);
                    for (int x = 0; x < width; x++)
                    {
                        bool bitSet = (source[x / 8] & (0x80 >> (x % 8))) != 0;
                        bool isInk = bitSet ? !zeroIsInk : zeroIsInk;
                        // MinIsWhite wire format: 1 = ink.
                        if (isInk)
                            outRow[x / 8] |= (byte)(0x80 >> (x % 8));
                    }
                    tiff.WriteScanline(outRow, y);
                }
            }
            finally
            {
                bitonal.UnlockBits(src);
            }
        }

        private static int ResolveDpi(float resolution)
        {
            int dpi = (int)Math.Round(resolution);
            return dpi > 0 ? dpi : 200;
        }

        private static void ReadResolution(Tiff tiff, out int dpiX, out int dpiY)
        {
            dpiX = 200;
            dpiY = 200;
            FieldValue[] xr = tiff.GetField(TiffTag.XRESOLUTION);
            FieldValue[] yr = tiff.GetField(TiffTag.YRESOLUTION);
            FieldValue[] ru = tiff.GetField(TiffTag.RESOLUTIONUNIT);
            bool inch = ru == null || (ResUnit)ru[0].ToInt() != ResUnit.CENTIMETER;
            if (xr != null && xr[0].ToFloat() > 0) dpiX = (int)Math.Round(inch ? xr[0].ToFloat() : xr[0].ToFloat() * 2.54f);
            if (yr != null && yr[0].ToFloat() > 0) dpiY = (int)Math.Round(inch ? yr[0].ToFloat() : yr[0].ToFloat() * 2.54f);
            if (dpiX <= 0) dpiX = 200;
            if (dpiY <= 0) dpiY = 200;
        }
    }
}
