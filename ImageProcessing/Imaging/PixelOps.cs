using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using CvPoint = OpenCvSharp.Point;
using CvSize = OpenCvSharp.Size;

namespace ImageProcessing.Imaging
{
    /// <summary>
    /// Các thao tác pixel OpenCV ở tầng thấp, dùng chung cho các service ở tầng cao hơn. Mọi hàm
    /// trả về <see cref="Mat"/> đều chuyển quyền sở hữu cho bên gọi (phải tự dispose); mọi hàm
    /// nhận tham số đầu vào chỉ để đọc và không bao giờ dispose chúng.
    /// </summary>
    internal static class PixelOps
    {
        /// <summary>Giải mã 1 file raster mà GDI+ đọc được (khung hình đầu tiên) thành <see cref="Mat"/> dạng BGR/BGRA.</summary>
        public static Mat LoadAsMat(string path)
        {
            using (Bitmap decoded = (Bitmap)Image.FromFile(path))
            using (Bitmap detached = new Bitmap(decoded)) // tách khỏi handle file trước khi chuyển đổi
                return BitmapConverter.ToMat(detached);
        }

        /// <summary>True khi file giải mã ra dạng raster lập chỉ mục 1bpp (đen-trắng thuần).</summary>
        public static bool IsBitonalFile(string path)
        {
            using (Bitmap bmp = (Bitmap)Image.FromFile(path))
                return bmp.PixelFormat == PixelFormat.Format1bppIndexed;
        }

        public static Bitmap ToManagedBitmap(Mat mat)
        {
            return BitmapConverter.ToBitmap(mat);
        }

        /// <summary>View ảnh xám 1 kênh; clone nếu đã là ảnh xám sẵn để quy tắc sở hữu luôn nhất quán.</summary>
        public static Mat ToGrayscale(Mat src)
        {
            if (src.Channels() == 1)
                return src.Clone();

            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            return gray;
        }

        public static Bitmap ToGrayscaleBitmap(Mat src)
        {
            using (Mat gray = ToGrayscale(src))
                return ToManagedBitmap(gray);
        }

        /// <summary>
        /// Áp ngưỡng Otsu để tạo mặt nạ 1 kênh giá trị 0/255, trong đó 255 = giấy (trắng) và
        /// 0 = mực, giống cách 1 bản scan kiểu fax MinIsWhite thường được biểu diễn.
        /// </summary>
        public static Mat ToOtsuBitonal(Mat src)
        {
            using (Mat gray = ToGrayscale(src))
            {
                Mat bw = new Mat();
                Cv2.Threshold(gray, bw, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
                return bw;
            }
        }

        /// <summary>Mặt nạ mực theo ngưỡng Otsu (255 = mực, 0 = giấy) - ngược lại với <see cref="ToOtsuBitonal"/>.</summary>
        private static Mat ToOtsuInkMask(Mat src)
        {
            using (Mat gray = ToGrayscale(src))
            {
                Mat ink = new Mat();
                Cv2.Threshold(gray, ink, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
                return ink;
            }
        }

        /// <summary>Tỉ lệ (0..1) số pixel được ngưỡng Otsu xếp loại là mực.</summary>
        public static double ForegroundCoverage(Mat src)
        {
            using (Mat ink = ToOtsuInkMask(src))
            {
                double fg = Cv2.CountNonZero(ink);
                double total = (double)ink.Rows * ink.Cols;
                return total <= 0 ? 0 : fg / total;
            }
        }

        /// <summary>True khi ít nhất <paramref name="thresholdPercent"/>% trang là vùng trắng (không có mực).</summary>
        public static bool LooksBlank(Mat src, float thresholdPercent)
        {
            double blankRatio = 1.0 - ForegroundCoverage(src);
            return blankRatio * 100.0 >= thresholdPercent;
        }

        /// <summary>Khử nhiễu bằng median blur.</summary>
        public static Mat MedianDespeckle(Mat src, int kernelSize = 3)
        {
            Mat dst = new Mat();
            Cv2.MedianBlur(src, dst, kernelSize);
            return dst;
        }

        /// <summary>
        /// Phép mở hình thái học (2x2) trên mặt nạ mực để loại bỏ các chấm mực lẻ tẻ (1 điểm ảnh
        /// hoặc gần như vậy), sau đó tô lại kết quả thành ảnh nền trắng/mực đen sạch sẽ.
        /// </summary>
        public static Mat OpenIsolatedInk(Mat src)
        {
            using (Mat ink = ToOtsuInkMask(src))
            using (Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new CvSize(2, 2)))
            using (Mat opened = new Mat())
            {
                Cv2.MorphologyEx(ink, opened, MorphTypes.Open, kernel);
                return PaintInkOnWhite(src.Size(), opened);
            }
        }

        /// <summary>Phát hiện và loại bỏ các đường kẻ ngang/dọc dài khỏi mặt nạ mực.</summary>
        public static Mat StripRuledLines(Mat src)
        {
            using (Mat ink = ToOtsuInkMask(src))
            {
                int hLen = Math.Max(10, src.Cols / 15);
                int vLen = Math.Max(10, src.Rows / 15);

                using (Mat hKernel = Cv2.GetStructuringElement(MorphShapes.Rect, new CvSize(hLen, 1)))
                using (Mat vKernel = Cv2.GetStructuringElement(MorphShapes.Rect, new CvSize(1, vLen)))
                using (Mat hLines = new Mat())
                using (Mat vLines = new Mat())
                using (Mat anyLine = new Mat())
                {
                    Cv2.MorphologyEx(ink, hLines, MorphTypes.Open, hKernel);
                    Cv2.MorphologyEx(ink, vLines, MorphTypes.Open, vKernel);
                    Cv2.BitwiseOr(hLines, vLines, anyLine);
                    Cv2.Subtract(ink, anyLine, ink);
                    return PaintInkOnWhite(src.Size(), ink);
                }
            }
        }

        /// <summary>Cắt về đúng khung bao của nội dung không đen (bỏ viền đen quét thừa xung quanh).</summary>
        public static Mat TrimDarkMargins(Mat src)
        {
            using (Mat gray = ToGrayscale(src))
            using (Mat notBlack = new Mat())
            {
                Cv2.Threshold(gray, notBlack, 16, 255, ThresholdTypes.Binary);

                CvPoint[][] contours;
                HierarchyIndex[] hierarchy;
                Cv2.FindContours(notBlack, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                if (contours.Length == 0)
                    return src.Clone();

                Rect bounds = Cv2.BoundingRect(contours[0]);
                foreach (CvPoint[] c in contours)
                    bounds = bounds.Union(Cv2.BoundingRect(c));

                bounds = bounds.Intersect(new Rect(0, 0, src.Cols, src.Rows));
                if (bounds.Width <= 0 || bounds.Height <= 0)
                    return src.Clone();

                using (Mat roi = new Mat(src, bounds))
                    return roi.Clone();
            }
        }

        // Chi phí ước lượng góc nghiêng tỉ lệ theo số điểm ảnh mực thu thập được (mỗi điểm phải
        // copy từ vùng nhớ native qua 1 lệnh gọi marshal riêng lẻ), trong khi góc kết quả gần như
        // không đổi theo tỉ lệ ảnh. Ước lượng trên 1 bản copy đã giới hạn kích thước thay vì trên
        // toàn bộ trang gốc biến bước này từ O(số pixel trang) thành O(hằng số) - với 1 trang văn
        // bản A4@200dpi thông thường, riêng việc này đã giảm chi phí của Deskew khoảng 10 lần, mà
        // không đổi kết quả: phép warp thực tế vẫn chạy trên src gốc, giữ nguyên độ phân giải.
        private const int SkewEstimationMaxDimension = 800;

        /// <summary>
        /// Ước lượng góc nghiêng chủ đạo của các dòng chữ dựa trên hình chữ nhật diện tích nhỏ
        /// nhất bao quanh các điểm ảnh mực, rồi xoay trang để chỉnh lại. Độ nghiêng dưới 0.1 độ
        /// được giữ nguyên không xử lý.
        /// </summary>
        public static Mat EstimateSkewAndDeskew(Mat src)
        {
            double angle = EstimateSkewAngleDegrees(src);
            if (Math.Abs(angle) < 0.1)
                return src.Clone();

            Point2f center = new Point2f(src.Cols / 2f, src.Rows / 2f);
            using (Mat rotation = Cv2.GetRotationMatrix2D(center, angle, 1.0))
            {
                Mat dst = new Mat();
                Cv2.WarpAffine(src, dst, rotation, src.Size(), InterpolationFlags.Cubic, BorderTypes.Constant, Scalar.White);
                return dst;
            }
        }

        private static double EstimateSkewAngleDegrees(Mat src)
        {
            int longSide = Math.Max(src.Cols, src.Rows);
            bool downscale = longSide > SkewEstimationMaxDimension;
            Mat estimationSource = src;
            try
            {
                if (downscale)
                {
                    double scale = (double)SkewEstimationMaxDimension / longSide;
                    Mat resized = new Mat();
                    Cv2.Resize(src, resized, new CvSize(0, 0), scale, scale, InterpolationFlags.Area);
                    estimationSource = resized;
                }

                using (Mat ink = ToOtsuInkMask(estimationSource))
                {
                    CvPoint[] inkPoints;
                    using (Mat nonZero = new Mat())
                    {
                        Cv2.FindNonZero(ink, nonZero);
                        if (nonZero.Empty())
                            return 0;

                        int rows = nonZero.Rows;
                        inkPoints = new CvPoint[rows];
                        for (int i = 0; i < rows; i++)
                        {
                            Vec2i v = nonZero.Get<Vec2i>(i);
                            inkPoints[i] = new CvPoint(v.Item0, v.Item1);
                        }
                    }

                    RotatedRect box = Cv2.MinAreaRect(inkPoints);
                    double angle = box.Angle;
                    if (angle < -45) angle += 90;
                    if (angle > 45) angle -= 90;
                    return angle;
                }
            }
            finally
            {
                if (downscale) estimationSource.Dispose();
            }
        }

        public static Mat RotateQuadrant(Mat src, RotateFlags flags)
        {
            Mat dst = new Mat();
            Cv2.Rotate(src, dst, flags);
            return dst;
        }

        /// <summary>Thumbnail độ rộng cố định (giữ nguyên tỉ lệ khung hình), viền xám đậm 2px.</summary>
        public static Bitmap BuildFramedThumbnail(Mat src, int width)
        {
            int height = (int)((long)width * src.Height / src.Width) + 1;

            using (Mat resized = new Mat())
            {
                Cv2.Resize(src, resized, new CvSize(width, height), 0, 0, InterpolationFlags.Area);
                Cv2.Rectangle(resized, new Rect(0, 0, resized.Width, resized.Height),
                    new Scalar(Color.DarkGray.B, Color.DarkGray.G, Color.DarkGray.R), 2);
                return ToManagedBitmap(resized);
            }
        }

        private static Mat PaintInkOnWhite(CvSize size, Mat inkMask)
        {
            Mat dst = new Mat(size, MatType.CV_8UC1, new Scalar(255));
            dst.SetTo(new Scalar(0), inkMask);
            return dst;
        }
    }
}
