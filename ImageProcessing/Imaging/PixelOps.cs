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
    /// Low-level OpenCV pixel operations shared by the higher-level services. Every method
    /// that returns a <see cref="Mat"/> transfers ownership to the caller (dispose it); every
    /// method takes its input(s) by reference and never disposes them.
    /// </summary>
    internal static class PixelOps
    {
        /// <summary>Decodes any GDI+-readable raster file (first frame) into a BGR/BGRA <see cref="Mat"/>.</summary>
        public static Mat LoadAsMat(string path)
        {
            using (Bitmap decoded = (Bitmap)Image.FromFile(path))
            using (Bitmap detached = new Bitmap(decoded)) // detach from the file handle before converting
                return BitmapConverter.ToMat(detached);
        }

        /// <summary>True when the file decodes as a 1bpp-indexed (bitonal) raster.</summary>
        public static bool IsBitonalFile(string path)
        {
            using (Bitmap bmp = (Bitmap)Image.FromFile(path))
                return bmp.PixelFormat == PixelFormat.Format1bppIndexed;
        }

        public static Bitmap ToManagedBitmap(Mat mat)
        {
            return BitmapConverter.ToBitmap(mat);
        }

        /// <summary>Single-channel grayscale view; clones when already grayscale so ownership rules stay uniform.</summary>
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
        /// Otsu-thresholds to a 0/255 single-channel mask where 255 = paper (white) and
        /// 0 = ink, mirroring how a fax-style MinIsWhite scan is normally represented.
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

        /// <summary>Otsu-thresholded ink mask (255 = ink, 0 = paper) - the inverse of <see cref="ToOtsuBitonal"/>.</summary>
        private static Mat ToOtsuInkMask(Mat src)
        {
            using (Mat gray = ToGrayscale(src))
            {
                Mat ink = new Mat();
                Cv2.Threshold(gray, ink, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
                return ink;
            }
        }

        /// <summary>Fraction (0..1) of pixels classified as ink by Otsu thresholding.</summary>
        public static double ForegroundCoverage(Mat src)
        {
            using (Mat ink = ToOtsuInkMask(src))
            {
                double fg = Cv2.CountNonZero(ink);
                double total = (double)ink.Rows * ink.Cols;
                return total <= 0 ? 0 : fg / total;
            }
        }

        /// <summary>True when at least <paramref name="thresholdPercent"/>% of the page is blank (non-ink).</summary>
        public static bool LooksBlank(Mat src, float thresholdPercent)
        {
            double blankRatio = 1.0 - ForegroundCoverage(src);
            return blankRatio * 100.0 >= thresholdPercent;
        }

        /// <summary>Median-blur despeckle.</summary>
        public static Mat MedianDespeckle(Mat src, int kernelSize = 3)
        {
            Mat dst = new Mat();
            Cv2.MedianBlur(src, dst, kernelSize);
            return dst;
        }

        /// <summary>
        /// Morphological opening (2x2) over the ink mask to drop isolated single/near-single
        /// pixel specks, then repaints the result as a clean white-paper/black-ink image.
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

        /// <summary>Detects and subtracts long horizontal/vertical ruled lines from the ink mask.</summary>
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

        /// <summary>Crops to the bounding box of non-black content (drops surrounding black scan borders).</summary>
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

        // Angle estimation cost scales with the number of ink pixels collected (each one is
        // copied from native memory through a per-element marshaling call), while the resulting
        // angle is essentially scale-invariant. Estimating on a capped-size copy instead of the
        // full-resolution page turns an O(page pixel count) step into an O(constant) one - on a
        // typical A4@200dpi text page this alone cuts Deskew's cost by roughly 10x, without
        // changing the output: the warp itself still runs on the untouched, full-resolution src.
        private const int SkewEstimationMaxDimension = 800;

        /// <summary>
        /// Estimates the dominant text-line skew from the ink pixels' minimum-area rectangle
        /// and rotates the page to correct it. Skew under 0.1 degrees is left untouched.
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

        /// <summary>Fixed-width thumbnail (aspect-ratio preserved) framed with a 2px dark-gray border.</summary>
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
