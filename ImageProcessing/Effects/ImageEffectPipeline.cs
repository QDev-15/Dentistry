using System;
using System.Drawing;
using ImageProcessing.Assembly;
using ImageProcessing.Documents;
using ImageProcessing.Imaging;
using OpenCvSharp;

namespace ImageProcessing.Effects
{
    public enum RotationDirection
    {
        Clockwise,
        CounterClockwise
    }

    /// <summary>
    /// Các bước làm sạch mà <see cref="ImageEffectPipeline.ApplyPipeline"/> sẽ chạy, và theo thứ
    /// tự nào (xoá chấm mực lẻ tẻ, rồi chỉnh nghiêng, rồi khử nhiễu, rồi cắt viền đen - đúng theo
    /// thứ tự pipeline truyền thống của GdPicture/OpenImaging).
    /// </summary>
    public sealed class EffectPipelineOptions
    {
        /// <summary>Xoá các chấm mực lẻ tẻ (2x2 điểm ảnh trở xuống). Chỉ áp dụng cho trang không màu.</summary>
        public bool RemoveIsolatedDots { get; set; }

        public bool Deskew { get; set; }

        public bool Despeckle { get; set; }

        public bool RemoveBlackBorder { get; set; }

        /// <summary>False = trang được lưu ở dạng đen-trắng (TIFF CCITT G4); true = lưu dạng JPEG màu.</summary>
        public bool ColorImage { get; set; }

        public static EffectPipelineOptions None { get { return new EffectPipelineOptions(); } }
    }

    /// <summary>
    /// Các thao tác làm sạch/chỉnh hình học trên 1 trang đã scan. Mọi hàm ở đây đều là hàm thuần
    /// (nhận Bitmap, trả về Bitmap, không đụng tới I/O); xem <see cref="ImageEffectFileOps"/> để
    /// có các hàm bọc tiện dụng làm việc trực tiếp trên đường dẫn file, dùng cho các pipeline
    /// scan/nhập liệu vốn trao đổi ảnh qua file tạm.
    /// </summary>
    public static class ImageEffectPipeline
    {
        public static Bitmap Rotate(Bitmap source, RotationDirection direction)
        {
            RotateFlags flags = direction == RotationDirection.Clockwise
                ? RotateFlags.Rotate90Clockwise
                : RotateFlags.Rotate90Counterclockwise;

            using (Mat src = ToMat(source))
            using (Mat rotated = PixelOps.RotateQuadrant(src, flags))
                return PixelOps.ToManagedBitmap(rotated);
        }

        public static Bitmap Despeckle(Bitmap source)
        {
            using (Mat src = ToMat(source))
            using (Mat dst = PixelOps.MedianDespeckle(src))
                return PixelOps.ToManagedBitmap(dst);
        }

        public static Bitmap Deskew(Bitmap source)
        {
            using (Mat src = ToMat(source))
            using (Mat dst = PixelOps.EstimateSkewAndDeskew(src))
                return PixelOps.ToManagedBitmap(dst);
        }

        public static Bitmap RemoveIsolatedDots(Bitmap source)
        {
            using (Mat src = ToMat(source))
            using (Mat dst = PixelOps.OpenIsolatedInk(src))
                return PixelOps.ToManagedBitmap(dst);
        }

        public static Bitmap RemoveRuledLines(Bitmap source)
        {
            using (Mat src = ToMat(source))
            using (Mat dst = PixelOps.StripRuledLines(src))
                return PixelOps.ToManagedBitmap(dst);
        }

        public static Bitmap TrimDarkMargins(Bitmap source)
        {
            using (Mat src = ToMat(source))
            using (Mat dst = PixelOps.TrimDarkMargins(src))
                return PixelOps.ToManagedBitmap(dst);
        }

        /// <summary>
        /// Chạy các bước làm sạch đã bật theo đúng thứ tự cố định: xoá chấm mực lẻ tẻ (chỉ trang
        /// không màu), chỉnh nghiêng, khử nhiễu, cắt viền đen.
        /// </summary>
        public static Bitmap ApplyPipeline(Bitmap source, EffectPipelineOptions options)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            options = options ?? EffectPipelineOptions.None;

            using (Mat src = ToMat(source))
            {
                Mat current = src.Clone();
                try
                {
                    if (options.RemoveIsolatedDots && !options.ColorImage)
                        current = Swap(current, PixelOps.OpenIsolatedInk(current));

                    if (options.Deskew)
                        current = Swap(current, PixelOps.EstimateSkewAndDeskew(current));

                    if (options.Despeckle)
                        current = Swap(current, PixelOps.MedianDespeckle(current));

                    if (options.RemoveBlackBorder)
                        current = Swap(current, PixelOps.TrimDarkMargins(current));

                    return PixelOps.ToManagedBitmap(current);
                }
                finally
                {
                    current.Dispose();
                }
            }
        }

        internal static Mat ApplyPipeline(Mat source, EffectPipelineOptions options)
        {
            options = options ?? EffectPipelineOptions.None;
            Mat current = source.Clone();

            if (options.RemoveIsolatedDots && !options.ColorImage)
                current = Swap(current, PixelOps.OpenIsolatedInk(current));

            if (options.Deskew)
                current = Swap(current, PixelOps.EstimateSkewAndDeskew(current));

            if (options.Despeckle)
                current = Swap(current, PixelOps.MedianDespeckle(current));

            if (options.RemoveBlackBorder)
                current = Swap(current, PixelOps.TrimDarkMargins(current));

            return current;
        }

        private static Mat Swap(Mat previous, Mat next)
        {
            previous.Dispose();
            return next;
        }

        private static Mat ToMat(Bitmap source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return OpenCvSharp.Extensions.BitmapConverter.ToMat(source);
        }
    }

    /// <summary>
    /// Các hàm bọc tiện dụng làm việc trực tiếp trên đường dẫn file, đặt trên nền
    /// <see cref="ImageEffectPipeline"/>, dành cho bên gọi nhận trang scan dưới dạng file tạm
    /// (driver TWAIN, thư mục nhập liệu) thay vì bitmap trong bộ nhớ: mỗi thao tác tự tải file,
    /// chạy hiệu ứng, lưu lại đúng độ sâu màu đã đọc được (đen-trắng vẫn giữ TIFF đen-trắng, còn
    /// lại chuyển thành JPEG) và trả về 1 ảnh thumbnail.
    /// </summary>
    public static class ImageEffectFileOps
    {
        public static Bitmap RotateInPlace(string path, RotationDirection direction, int thumbnailWidth = 184)
        {
            RotateFlags flags = direction == RotationDirection.Clockwise
                ? RotateFlags.Rotate90Clockwise
                : RotateFlags.Rotate90Counterclockwise;

            return TransformInPlace(path, thumbnailWidth, src => PixelOps.RotateQuadrant(src, flags));
        }

        public static Bitmap DespeckleInPlace(string path, int thumbnailWidth = 184)
        {
            return TransformInPlace(path, thumbnailWidth, src => PixelOps.MedianDespeckle(src));
        }

        public static Bitmap DeskewInPlace(string path, int thumbnailWidth = 184)
        {
            return TransformInPlace(path, thumbnailWidth, PixelOps.EstimateSkewAndDeskew);
        }

        /// <summary>Chạy <see cref="ImageEffectPipeline.ApplyPipeline(Bitmap,EffectPipelineOptions)"/> và lưu lại đúng vị trí cũ.</summary>
        public static void ApplyPipelineInPlace(string path, EffectPipelineOptions options)
        {
            using (Mat src = LoadFirstPage(path))
            using (Mat result = ImageEffectPipeline.ApplyPipeline(src, options))
                ImagePageWriter.Save(result, path, options.ColorImage ? ScanColorMode.Color : ScanColorMode.BlackAndWhite);
        }

        private static Bitmap TransformInPlace(string path, int thumbnailWidth, Func<Mat, Mat> transform)
        {
            bool bitonal = PixelOps.IsBitonalFile(path);
            using (Mat src = LoadFirstPage(path))
            using (Mat result = transform(src))
            {
                ImagePageWriter.Save(result, path, bitonal ? ScanColorMode.BlackAndWhite : ScanColorMode.Color);
                return PixelOps.BuildFramedThumbnail(result, thumbnailWidth);
            }
        }

        internal static Mat LoadFirstPage(string path)
        {
            if (TiffCodec.IsReadableTiff(path) && TiffCodec.IsMultiPage(path))
            {
                using (Bitmap first = TiffCodec.ReadPage(path, 0))
                    return OpenCvSharp.Extensions.BitmapConverter.ToMat(first);
            }
            return PixelOps.LoadAsMat(path);
        }
    }
}
