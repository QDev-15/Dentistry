using System.Drawing;
using ImageProcessing.Imaging;
using OpenCvSharp;

namespace ImageProcessing.Effects
{
    /// <summary>Tạo ảnh thumbnail có khung, độ rộng cố định, từ 1 file ảnh hoặc 1 bitmap trong bộ nhớ.</summary>
    public static class ThumbnailGenerator
    {
        public const int DefaultWidth = 184;

        public static Bitmap FromFile(string imagePath, int width = DefaultWidth)
        {
            using (Mat src = ImageEffectFileOps.LoadFirstPage(imagePath))
                return PixelOps.BuildFramedThumbnail(src, width);
        }

        public static Bitmap FromBitmap(Bitmap source, int width = DefaultWidth)
        {
            using (Mat src = OpenCvSharp.Extensions.BitmapConverter.ToMat(source))
                return PixelOps.BuildFramedThumbnail(src, width);
        }
    }
}
