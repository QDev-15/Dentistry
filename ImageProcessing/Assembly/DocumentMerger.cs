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
    /// <summary>Gộp nhiều file ảnh thành 1 PDF thường hoặc 1 file TIFF nhiều trang.</summary>
    public static class DocumentMerger
    {
        /// <summary>
        /// Tạo 1 PDF thường (không phải PDF/A), mỗi ảnh/khung hình nguồn chiếm trọn 1 trang. Muốn
        /// xuất PDF/A thì dùng <c>ImageProcessing.Archiving.PdfADocumentBuilder</c> thay cho hàm
        /// này.
        /// </summary>
        public static void MergeToPlainPdf(IEnumerable<string> sourceFiles, string destFile, DocumentMetadata metadata = null)
        {
            List<string> files = RequireFiles(sourceFiles);
            PdfRaster.CreateImagePdf(files, destFile, metadata);
        }

        /// <summary>
        /// Tạo 1 file TIFF nhiều trang. Khi <paramref name="convertTo1Bpp"/> là true, mỗi trang sẽ
        /// được nhị phân hoá (thuật toán Otsu) và ghi ra dưới dạng CCITT Group 4; ngược lại mỗi
        /// trang giữ nguyên độ sâu màu gốc qua 1 file TIFF nhiều khung hình thông thường.
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
