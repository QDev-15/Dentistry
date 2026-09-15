using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageProcessing.Documents;
using ImageProcessing.Imaging;
using ImageProcessing.Ocr;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Chuyển file ảnh đã scan thành các <see cref="ArchivePage"/> để đưa vào bộ đóng gói PDF/A,
    /// cố gắng giữ nguyên kiểu mã hoá gốc khi có thể: JPEG được nhúng nguyên trạng, TIFF CCITT
    /// Group 4 dạng single-strip được nhúng nguyên dòng dữ liệu gốc, ảnh đen-trắng nguồn khác sẽ
    /// được mã hoá lại sang Group 4 (không mất dữ liệu), còn lại đều mã hoá sang JPEG. File TIFF
    /// nhiều trang sẽ tách thành nhiều <see cref="ArchivePage"/>, mỗi khung hình 1 trang. Khi có
    /// truyền vào <see cref="OcrWordExtractor"/>, các từ nhận diện được của từng trang sẽ được
    /// gắn kèm để tạo lớp text vô hình.
    /// </summary>
    public static class ArchivePageBuilder
    {
        public static IEnumerable<ArchivePage> FromFile(string path, OcrWordExtractor ocr)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("path is required.", nameof(path));
            if (!File.Exists(path)) throw new FileNotFoundException("Image file not found.", path);

            string ext = (Path.GetExtension(path) ?? string.Empty).ToLowerInvariant();
            if (ext == ".jpg" || ext == ".jpeg")
                return FromJpeg(path, ocr);
            if (TiffCodec.IsReadableTiff(path))
                return FromTiff(path, ocr);
            return FromOther(path, ocr);
        }

        private static IEnumerable<ArchivePage> FromJpeg(string path, OcrWordExtractor ocr)
        {
            byte[] bytes = File.ReadAllBytes(path);
            int w, h, components, dpiX, dpiY;
            if (!JpegHeaderReader.TryParse(bytes, out w, out h, out components, out dpiX, out dpiY))
            {
                using (Bitmap bmp = new Bitmap(path))
                    return new[] { EncodeAsJpeg(bmp, ocr) };
            }

            PageColorSpace cs = components == 1 ? PageColorSpace.Gray
                              : components == 4 ? PageColorSpace.Cmyk
                              : PageColorSpace.Rgb;

            IReadOnlyList<OcrWord> words = null;
            if (ocr != null)
                using (Bitmap bmp = new Bitmap(path))
                    words = ocr.Extract(bmp);

            return new[] { new ArchivePage(bytes, PageCodec.Jpeg, w, h, cs, dpiX, dpiY, words) };
        }

        private static IEnumerable<ArchivePage> FromTiff(string path, OcrWordExtractor ocr)
        {
            List<ArchivePage> pages = new List<ArchivePage>();
            int frameCount = TiffCodec.GetPageCount(path);

            for (int frame = 0; frame < frameCount; frame++)
            {
                byte[] g4; int w, h, dpiX, dpiY;
                if (TiffCodec.TryGetRawGroup4(path, frame, out g4, out w, out h, out dpiX, out dpiY))
                {
                    IReadOnlyList<OcrWord> words = null;
                    if (ocr != null)
                        using (Bitmap bmp = TiffCodec.ReadPage(path, frame))
                            words = ocr.Extract(bmp);

                    pages.Add(new ArchivePage(g4, PageCodec.CcittGroup4, w, h, PageColorSpace.Bitonal, dpiX, dpiY, words));
                }
                else
                {
                    using (Bitmap bmp = TiffCodec.ReadPage(path, frame))
                        pages.Add(FromBitmap(bmp, ocr));
                }
            }
            return pages;
        }

        private static IEnumerable<ArchivePage> FromOther(string path, OcrWordExtractor ocr)
        {
            using (Bitmap bmp = new Bitmap(path))
                return new[] { FromBitmap(bmp, ocr) };
        }

        /// <summary>Ảnh đen-trắng (bitonal) sẽ chuyển sang Group 4; còn lại chuyển sang JPEG.</summary>
        private static ArchivePage FromBitmap(Bitmap bmp, OcrWordExtractor ocr)
        {
            IReadOnlyList<OcrWord> words = ocr != null ? ocr.Extract(bmp) : null;
            int dpiX = ResolveDpi(bmp.HorizontalResolution);
            int dpiY = ResolveDpi(bmp.VerticalResolution);

            if (bmp.PixelFormat == PixelFormat.Format1bppIndexed)
            {
                byte[] g4 = TiffCodec.EncodeGroup4(bmp);
                return new ArchivePage(g4, PageCodec.CcittGroup4, bmp.Width, bmp.Height, PageColorSpace.Bitonal, dpiX, dpiY, words);
            }

            return EncodeAsJpeg(bmp, ocr, words, dpiX, dpiY);
        }

        private static ArchivePage EncodeAsJpeg(Bitmap bmp, OcrWordExtractor ocr, IReadOnlyList<OcrWord> words = null, int dpiX = 0, int dpiY = 0)
        {
            if (words == null && ocr != null) words = ocr.Extract(bmp);
            if (dpiX == 0) dpiX = ResolveDpi(bmp.HorizontalResolution);
            if (dpiY == 0) dpiY = ResolveDpi(bmp.VerticalResolution);

            byte[] jpeg = EncodeJpegBytes(bmp, 80L);
            // GDI mã hoá bề mặt 24bpp dưới dạng YCbCr (3 thành phần) -> DeviceRGB.
            return new ArchivePage(jpeg, PageCodec.Jpeg, bmp.Width, bmp.Height, PageColorSpace.Rgb, dpiX, dpiY, words);
        }

        private static byte[] EncodeJpegBytes(Bitmap bmp, long quality)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb)
                return JpegCodec.EncodeToBytes(bmp, quality);

            // Đảm bảo bề mặt ở dạng 24bpp để JPEG ra đúng 3 thành phần DeviceRGB.
            using (Bitmap rgb = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format24bppRgb))
            {
                rgb.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
                using (Graphics g = Graphics.FromImage(rgb))
                    g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                return JpegCodec.EncodeToBytes(rgb, quality);
            }
        }

        private static int ResolveDpi(float resolution)
        {
            int dpi = (int)Math.Round(resolution);
            return dpi > 0 ? dpi : 200;
        }
    }

    /// <summary>Đọc header JPEG tối giản: kích thước ảnh, số thành phần màu và mật độ điểm ảnh JFIF - tránh phải giải mã toàn bộ ảnh chỉ để lấy thông số hình học.</summary>
    internal static class JpegHeaderReader
    {
        public static bool TryParse(byte[] data, out int width, out int height, out int components, out int dpiX, out int dpiY)
        {
            width = 0; height = 0; components = 3; dpiX = 200; dpiY = 200;
            if (data == null || data.Length < 4 || data[0] != 0xFF || data[1] != 0xD8)
                return false;

            bool haveDensity = false;
            int i = 2;
            while (i + 4 <= data.Length)
            {
                if (data[i] != 0xFF) { i++; continue; }
                byte marker = data[i + 1];
                if (marker == 0xD9 || marker == 0xDA) break; // EOI / bắt đầu scan
                if (marker == 0xFF) { i++; continue; }
                if (marker >= 0xD0 && marker <= 0xD7) { i += 2; continue; } // RSTn (không có length)

                int len = (data[i + 2] << 8) | data[i + 3];
                if (len < 2 || i + 2 + len > data.Length) break;
                int seg = i + 4;

                if (marker == 0xE0 && len >= 16 && data.Length >= seg + 12 &&
                    data[seg] == (byte)'J' && data[seg + 1] == (byte)'F' && data[seg + 2] == (byte)'I' && data[seg + 3] == (byte)'F')
                {
                    int units = data[seg + 7];
                    int xd = (data[seg + 8] << 8) | data[seg + 9];
                    int yd = (data[seg + 10] << 8) | data[seg + 11];
                    if (xd > 0 && yd > 0)
                    {
                        if (units == 1) { dpiX = xd; dpiY = yd; haveDensity = true; }
                        else if (units == 2) { dpiX = (int)Math.Round(xd * 2.54); dpiY = (int)Math.Round(yd * 2.54); haveDensity = true; }
                    }
                }

                bool isSof = (marker >= 0xC0 && marker <= 0xCF) && marker != 0xC4 && marker != 0xC8 && marker != 0xCC;
                if (isSof && len >= 8)
                {
                    height = (data[seg + 1] << 8) | data[seg + 2];
                    width = (data[seg + 3] << 8) | data[seg + 4];
                    components = data[seg + 5];
                    if (!haveDensity) { dpiX = 200; dpiY = 200; }
                    return width > 0 && height > 0;
                }

                i += 2 + len;
            }
            return false;
        }
    }
}
