using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using ImageProcessing.Documents;
using ImageProcessing.Imaging;
using OpenCvSharp;

namespace ImageProcessing.Assembly
{
    /// <summary>Combines multiple image files into one plain PDF or one multi-page TIFF.</summary>
    public static class DocumentMerger
    {
        /// <summary>
        /// Builds a plain (non-archival) PDF, one full page per source image/frame. For
        /// PDF/A output use <c>ImageProcessing.Archiving.PdfADocumentBuilder</c> instead.
        /// </summary>
        public static void MergeToPlainPdf(IEnumerable<string> sourceFiles, string destFile, DocumentMetadata metadata = null)
        {
            List<string> files = RequireFiles(sourceFiles);
            PdfRaster.CreateImagePdf(files, destFile, metadata);
        }

        /// <summary>
        /// Builds a multi-page TIFF. When <paramref name="convertTo1Bpp"/> is true every page is
        /// thresholded (Otsu) and written as CCITT Group 4; otherwise each source's native depth
        /// is preserved via a plain multi-frame TIFF.
        /// </summary>
        public static void MergeToTiff(IEnumerable<string> sourceFiles, string destFile, bool convertTo1Bpp)
        {
            List<string> files = RequireFiles(sourceFiles);

            if (convertTo1Bpp)
                MergeAsGroup4(files, destFile);
            else
                MergeAtNativeDepth(files, destFile);
        }

        private static void MergeAsGroup4(List<string> files, string destFile)
        {
            List<Mat> pages = new List<Mat>();
            try
            {
                foreach (string file in files)
                    foreach (Bitmap frame in RasterFrameSource.EnumerateFrames(file))
                        using (frame)
                        using (Mat mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(frame))
                            pages.Add(PixelOps.ToOtsuBitonal(mat));

                TiffCodec.SaveMultiPageGroup4Tiff(pages, destFile);
            }
            finally
            {
                foreach (Mat p in pages) p.Dispose();
            }
        }

        private static void MergeAtNativeDepth(List<string> files, string destFile)
        {
            List<Bitmap> frames = new List<Bitmap>();
            try
            {
                foreach (string file in files)
                    frames.AddRange(RasterFrameSource.EnumerateFrames(file));
                if (frames.Count == 0)
                    throw new InvalidOperationException("No pages to merge.");

                ImageCodecInfo tiffEncoder = ImageCodecInfo.GetImageEncoders()
                    .First(c => c.FormatID == ImageFormat.Tiff.Guid);

                using (EncoderParameters openParams = new EncoderParameters(1))
                {
                    openParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                    frames[0].Save(destFile, tiffEncoder, openParams);
                }

                for (int i = 1; i < frames.Count; i++)
                {
                    using (EncoderParameters addParams = new EncoderParameters(1))
                    {
                        addParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                        frames[0].SaveAdd(frames[i], addParams);
                    }
                }

                using (EncoderParameters closeParams = new EncoderParameters(1))
                {
                    closeParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.Flush);
                    frames[0].SaveAdd(closeParams);
                }
            }
            finally
            {
                foreach (Bitmap f in frames) f.Dispose();
            }
        }

        private static List<string> RequireFiles(IEnumerable<string> sourceFiles)
        {
            List<string> files = sourceFiles?.ToList() ?? new List<string>();
            if (files.Count == 0)
                throw new ArgumentException("At least one source file is required.", nameof(sourceFiles));
            return files;
        }
    }
}
