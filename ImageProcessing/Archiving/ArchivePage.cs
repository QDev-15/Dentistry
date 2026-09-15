using System;
using System.Collections.Generic;
using System.Drawing;

namespace ImageProcessing.Archiving
{
    /// <summary>Kiểu nén ảnh của phần dữ liệu đã mã hoá trong <see cref="ArchivePage"/>.</summary>
    public enum PageCodec
    {
        /// <summary>JPEG chuẩn (baseline), nhúng nguyên trạng qua /DCTDecode.</summary>
        Jpeg,

        /// <summary>Nhị phân đen-trắng CCITT Group 4 (chuẩn fax), nhúng qua /CCITTFaxDecode.</summary>
        CcittGroup4,

        /// <summary>JPEG 2000 (JP2/J2K), nhúng qua /JPXDecode. Yêu cầu PDF/A-2 trở lên.</summary>
        Jpeg2000,

        /// <summary>Dữ liệu thô nén bằng Zlib/deflate, nhúng qua /FlateDecode.</summary>
        Flate
    }

    /// <summary>Cách biểu diễn màu của dữ liệu điểm ảnh trong <see cref="ArchivePage"/>.</summary>
    public enum PageColorSpace
    {
        /// <summary>Đen/trắng 1-bit (DeviceGray, 1 bpc).</summary>
        Bitonal,

        /// <summary>Thang xám 8-bit (DeviceGray).</summary>
        Gray,

        /// <summary>Màu 24-bit (DeviceRGB).</summary>
        Rgb,

        /// <summary>Màu 32-bit (DeviceCMYK). Cần có OutputIntent/ICC dạng CMYK.</summary>
        Cmyk
    }

    /// <summary>Một từ được OCR nhận diện, sẽ được vẽ dưới dạng text vô hình đè lên đúng vị trí in trên trang.</summary>
    public sealed class OcrWord
    {
        /// <param name="text">Nội dung chữ đã nhận diện.</param>
        /// <param name="boundingBox">
        /// Khung chứa từ theo toạ độ pixel của ảnh, gốc toạ độ ở góc trên-trái (X sang phải,
        /// Y xuống dưới) - đúng hệ toạ độ gốc của Tesseract/hOCR. Bộ đóng gói PDF sẽ tự quy đổi
        /// sang hệ toạ độ PDF (gốc dưới-trái, đơn vị point) dựa theo DPI của trang.
        /// </param>
        /// <param name="baseline">Toạ độ Y của đường baseline (pixel ảnh, gốc trên-trái), có thể để trống; null = ước lượng từ đáy khung chứa.</param>
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
    /// Một trang đã chuẩn bị sẵn để đưa vào <see cref="IPdfArchiver"/>: dữ liệu ảnh đã mã hoá sẵn
    /// (giữ nguyên trạng để bộ đóng gói nhúng thẳng, không phải mã hoá lại) cùng thông số kích
    /// thước và (nếu có) các khung từ OCR để tạo lớp text vô hình.
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
