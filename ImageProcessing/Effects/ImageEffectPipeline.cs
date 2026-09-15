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
    /// Which cleanup passes <see cref="ImageEffectPipeline.ApplyPipeline"/> runs, and in what
    /// order (isolated-dot removal, then deskew, then despeckle, then black-border crop -
    /// matching the historical GdPicture/OpenImaging pipeline order).
    /// </summary>
    public sealed class EffectPipelineOptions
    {
        /// <summary>Drops isolated ink specks (2x2 or smaller). Applies only to non-colour pages.</summary>
        public bool RemoveIsolatedDots { get; set; }

        public bool Deskew { get; set; }

        public bool Despeckle { get; set; }

        public bool RemoveBlackBorder { get; set; }

        /// <summary>False = page is stored bitonal (CCITT G4 TIFF); true = stored as colour JPEG.</summary>
        public bool ColorImage { get; set; }

        public static EffectPipelineOptions None { get { return new EffectPipelineOptions(); } }
    }

    /// <summary>
    /// Cleanup/geometry operations on a scanned page. Every method here is pure (Bitmap in,
    /// Bitmap out, no I/O); see <see cref="ImageEffectFileOps"/> for the file-path convenience
    /// wrappers used by scan/import pipelines that hand off images as temp files.
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
        /// Runs the configured cleanup passes in the fixed order: isolated-dot removal (non-colour
        /// pages only), deskew, despeckle, black-border crop.
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
    /// File-path convenience wrappers over <see cref="ImageEffectPipeline"/>, for callers that
    /// receive scanned pages as temp files (TWAIN drivers, import folders) rather than in-memory
    /// bitmaps: each operation loads the file, runs the effect, saves back in the same depth it
    /// found (bitonal stays bitonal TIFF, everything else becomes JPEG) and returns a thumbnail.
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

        /// <summary>Runs <see cref="ImageEffectPipeline.ApplyPipeline(Bitmap,EffectPipelineOptions)"/> and saves back in place.</summary>
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
