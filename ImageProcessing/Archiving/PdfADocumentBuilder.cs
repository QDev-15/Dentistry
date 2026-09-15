using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageProcessing.Assembly;
using ImageProcessing.Ocr;

namespace ImageProcessing.Archiving
{
    /// <summary>Tuning for <see cref="PdfADocumentBuilder.Build"/>.</summary>
    public sealed class PdfADocumentOptions
    {
        public PdfAConformance Conformance { get; set; } = PdfAConformance.Level2B;

        /// <summary>Null/empty = no OCR text layer.</summary>
        public string OcrDataPath { get; set; }

        /// <summary>Tesseract language codes, e.g. "eng+vie". Null = <see cref="OcrWordExtractor"/>'s own default.</summary>
        public string OcrLanguages { get; set; }

        /// <summary>Run the content-preserving structural optimizer (qpdf) after archiving. Best-effort: failures keep the un-optimized, still-valid PDF/A.</summary>
        public bool Optimize { get; set; }

        /// <summary>Defaults to <see cref="PdfArchiver"/>.</summary>
        public IPdfArchiver Archiver { get; set; }

        /// <summary>Defaults to <see cref="QpdfStructuralOptimizer"/>.</summary>
        public IPdfOptimizer Optimizer { get; set; }
    }

    /// <summary>
    /// High-level "merge these image files into a PDF/A" entry point: prepares codec-preserving
    /// pages (<see cref="ArchivePageBuilder"/>), optionally runs OCR (<see cref="OcrWordExtractor"/>),
    /// archives via <see cref="IPdfArchiver"/>, and optionally structurally optimizes the result.
    /// </summary>
    public static class PdfADocumentBuilder
    {
        public static void Build(IEnumerable<string> sourceFiles, string destFile, DocumentMetadata metadata, PdfADocumentOptions options = null)
        {
            options = options ?? new PdfADocumentOptions();
            IPdfArchiver archiver = options.Archiver ?? new PdfArchiver();

            OcrWordExtractor ocr = null;
            try
            {
                if (!string.IsNullOrEmpty(options.OcrDataPath))
                {
                    ocr = string.IsNullOrEmpty(options.OcrLanguages)
                        ? new OcrWordExtractor(options.OcrDataPath)
                        : new OcrWordExtractor(options.OcrDataPath, options.OcrLanguages);
                }

                List<ArchivePage> pages = new List<ArchivePage>();
                foreach (string file in sourceFiles)
                    pages.AddRange(ArchivePageBuilder.FromFile(file, ocr));

                byte[] pdfBytes;
                using (MemoryStream built = new MemoryStream())
                {
                    archiver.CreatePdfA(pages, options.Conformance, metadata, built);
                    pdfBytes = built.ToArray();
                }

                if (options.Optimize)
                    pdfBytes = TryOptimize(pdfBytes, options.Optimizer ?? new QpdfStructuralOptimizer());

                File.WriteAllBytes(destFile, pdfBytes);
            }
            finally
            {
                if (ocr != null) ocr.Dispose();
            }
        }

        /// <summary>
        /// Runs the optimizer over the built PDF/A bytes. Best-effort: on any failure (optimizer
        /// missing or erroring) the original, already-valid PDF/A bytes are returned unchanged so
        /// the export never fails because of an optional optimization step.
        /// </summary>
        private static byte[] TryOptimize(byte[] pdfBytes, IPdfOptimizer optimizer)
        {
            try
            {
                using (MemoryStream input = new MemoryStream(pdfBytes, false))
                using (MemoryStream output = new MemoryStream())
                {
                    OptimizeResult result = optimizer.Optimize(input, OptimizeOptions.PdfASafe(), output);
                    if (result.Success && output.Length > 0)
                        return output.ToArray();
                }
            }
            catch
            {
                // Keep the un-optimized (still valid) PDF/A.
            }
            return pdfBytes;
        }
    }
}
