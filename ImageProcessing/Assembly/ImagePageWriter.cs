using System;
using System.Drawing;
using ImageProcessing.Documents;
using ImageProcessing.Imaging;
using OpenCvSharp;

namespace ImageProcessing.Assembly
{
    /// <summary>Chế độ màu mà 1 trang scan/đã xử lý được lưu lại.</summary>
    public enum ScanColorMode
    {
        BlackAndWhite,
        Gray,
        Color
    }

    /// <summary>
    /// Ghi 1 trang đã xử lý ra đĩa theo đúng định dạng mà <see cref="ScanColorMode"/> của nó quy
    /// định. Nội bộ: làm việc trực tiếp trên <see cref="Mat"/> của OpenCvSharp, không đưa ra bề
    /// mặt public để bên dùng thư viện không bao giờ phải tham chiếu OpenCvSharp chỉ để gọi hàm
    /// này.
    /// </summary>
    internal static class ImagePageWriter
    {
        /// <summary>Đen trắng -> TIFF CCITT Group 4 1 trang; Xám/Màu -> JPEG.</summary>
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
