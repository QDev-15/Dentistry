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
    /// <summary>Splits a multi-page TIFF or a PDF into individual page files, optionally dropping blank pages.</summary>
    public static class PageSplitter
    {
        /// <summary>
        /// Splits a multi-page TIFF into single-page files; a single-page TIFF (or any other
        /// image) is returned unchanged (as its own path) unless it is dropped as blank.
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
        /// Renders a PDF at <paramref name="resolutionDpi"/> and saves each page per
        /// <paramref name="colorMode"/>, optionally dropping blank pages. Throws
        /// <see cref="NotSupportedException"/> for a dynamic XFA form (see
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
            try { if (File.Exists(path)) File.Delete(path); } catch { /* best effort */ }
        }
    }
}
