using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ImageProcessing.Documents;
using ImageProcessing.Effects;
using ImageProcessing.Imaging;
using OpenCvSharp;

namespace ImageProcessing.Assembly
{
    /// <summary>Tách 1 file TIFF nhiều trang hoặc 1 PDF thành các file trang riêng lẻ, có thể tuỳ chọn bỏ qua trang trắng.</summary>
    public static class PageSplitter
    {
        /// <summary>
        /// Tách 1 file TIFF nhiều trang thành các file 1 trang; TIFF chỉ có 1 trang (hoặc bất kỳ
        /// ảnh nào khác) sẽ được trả về nguyên trạng (giữ nguyên đường dẫn của chính nó) trừ khi
        /// bị loại vì là trang trắng.
        /// </summary>
        public static List<string> SplitTiff(string sourceFile, bool removeBlankPages, string destFolder = null, BlankPageOptions blankOptions = null)
        {
            List<string> result = new List<string>();

            if (TiffCodec.IsReadableTiff(sourceFile) && TiffCodec.IsMultiPage(sourceFile))
            {
                int pages = TiffCodec.GetPageCount(sourceFile);
                for (int i = 1; i <= pages; i++)
                {
                    string pageFile = NewTempPath(destFolder, ".tif");
                    TiffCodec.ExtractPageToFile(sourceFile, i, pageFile);

                    if (!removeBlankPages || !BlankPageDetector.IsBlank(pageFile, blankOptions))
                        result.Add(pageFile);
                    else
                        TryDelete(pageFile);
                }
            }
            else if (!removeBlankPages || !BlankPageDetector.IsBlank(sourceFile, blankOptions))
            {
                result.Add(sourceFile);
            }

            return result;
        }

        /// <summary>
        /// Kết xuất (render) 1 file PDF ở độ phân giải <paramref name="resolutionDpi"/> và lưu
        /// mỗi trang theo đúng <paramref name="colorMode"/>, có thể tuỳ chọn bỏ qua trang trắng.
        /// Ném <see cref="NotSupportedException"/> nếu là form XFA động (xem
        /// <see cref="PdfRaster.IsDynamicXfaUnsupported"/>).
        /// </summary>
        public static List<string> SplitPdf(string pdfPath, int resolutionDpi, ScanColorMode colorMode, bool removeBlankPages, string destFolder = null, BlankPageOptions blankOptions = null)
        {
            if (PdfRaster.IsDynamicXfaUnsupported(pdfPath))
                throw new NotSupportedException(
                    "This PDF is a dynamic XFA form (Adobe LiveCycle) and cannot be imported. " +
                    "Its content exists only in the XFA form layer, which the image renderer does " +
                    "not support - importing it would only capture the placeholder page, not the " +
                    "actual form.\r\n\r\nFix: flatten it to a static PDF first (Adobe Acrobat -> " +
                    "Print to PDF, or Prepare Form -> Flatten), then import that file instead.");

            List<string> result = new List<string>();
            foreach (Bitmap page in PdfRaster.RenderPages(pdfPath, resolutionDpi))
            {
                using (page)
                using (Mat mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(page))
                {
                    if (removeBlankPages && BlankPageDetector.IsBlank(mat, blankOptions))
                        continue;

                    string extension = colorMode == ScanColorMode.BlackAndWhite ? ".tif" : ".jpg";
                    string pageFile = NewTempPath(destFolder, extension);
                    ImagePageWriter.Save(mat, pageFile, colorMode);
                    result.Add(pageFile);
                }
            }
            return result;
        }

        private static string NewTempPath(string destFolder, string extension)
        {
            return string.IsNullOrEmpty(destFolder)
                ? Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + extension)
                : Path.Combine(destFolder, Guid.NewGuid().ToString("N") + extension);
        }

        private static void TryDelete(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); } catch { /* cố gắng hết sức, bỏ qua nếu lỗi */ }
        }
    }
}
