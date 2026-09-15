using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageProcessing.Imaging
{
    /// <summary>Hàm hỗ trợ mã hoá JPEG dùng chung qua GDI+ (tham số hoá chất lượng), được cả bộ
    /// ghi trang lẫn bộ dựng trang PDF/A dùng, để chỉ có duy nhất 1 nơi làm việc trực tiếp với
    /// <see cref="ImageCodecInfo"/>.</summary>
    internal static class JpegCodec
    {
        // Danh sách codec đã đăng ký của hệ thống không đổi trong suốt vòng đời tiến trình; gọi
        // GetImageEncoders() sẽ duyệt và cấp phát 1 mảng mới mỗi lần gọi, nên với bên gọi có
        // throughput cao (web) việc chỉ resolve 1 lần cho cả tiến trình tiết kiệm đáng kể so với
        // làm lại mỗi khi lưu 1 trang.
        private static readonly ImageCodecInfo JpegEncoderInfo = ResolveEncoder();

        public static void SaveToFile(Bitmap bmp, string destPath, long quality)
        {
            using (EncoderParameters encoderParams = BuildParams(quality))
                bmp.Save(destPath, JpegEncoderInfo, encoderParams);
        }

        public static byte[] EncodeToBytes(Bitmap bmp, long quality)
        {
            using (MemoryStream ms = new MemoryStream())
            using (EncoderParameters encoderParams = BuildParams(quality))
            {
                bmp.Save(ms, JpegEncoderInfo, encoderParams);
                return ms.ToArray();
            }
        }

        private static EncoderParameters BuildParams(long quality)
        {
            EncoderParameters p = new EncoderParameters(1);
            p.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            return p;
        }

        private static ImageCodecInfo ResolveEncoder()
        {
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()
                .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
            if (codec == null)
                throw new InvalidOperationException("No JPEG encoder registered on this machine.");
            return codec;
        }
    }
}
