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

    /// <summary>Một cặp loại mã vạch + giá trị mong đợi mà bên gọi muốn tìm kiếm.</summary>
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

    /// <summary>Một mã vạch đã giải mã được.</summary>
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

    /// <summary>Đọc mã vạch 1D từ trang đầu tiên của 1 file ảnh (dùng ZXing.Net).</summary>
    public static class BarcodeScanner
    {
        /// <summary>Giải mã mọi mã vạch trên trang có định dạng khớp với 1 trong các <paramref name="kinds"/>.</summary>
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
        /// Quét lần lượt các <paramref name="targets"/> đã cấu hình theo đúng thứ tự, trả về mục
        /// đầu tiên vừa khớp cả loại mã vạch LẪN điều kiện so sánh giá trị. Trả về null nếu không
        /// có gì khớp.
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
        /// Tạo trực tiếp 1 nguồn luminance của ZXing từ Bitmap, không cần gói
        /// ZXing.Windows.Compatibility.
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
                            int si = x * 3; // GDI+ lưu theo thứ tự BGR
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

    /// <summary>Vẽ 1 mã vạch 1D và ghép nó vào 1 ảnh trang.</summary>
    public static class BarcodeStamp
    {
        /// <summary>
        /// Vẽ <paramref name="value"/> dưới dạng mã vạch <paramref name="kind"/> lấp đầy
        /// <paramref name="zone"/> rồi vẽ nó lên 1 bản sao của <paramref name="page"/>.
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

    /// <summary>Cầu nối tới bộ tải trang-đầu-tiên nội bộ mà không đưa nó ra bề mặt public.</summary>
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
