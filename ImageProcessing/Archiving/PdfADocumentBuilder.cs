using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageProcessing.Assembly;
using ImageProcessing.Ocr;

namespace ImageProcessing.Archiving
{
    /// <summary>Tuỳ chỉnh cho <see cref="PdfADocumentBuilder.Build"/>.</summary>
    public sealed class PdfADocumentOptions
    {
        public PdfAConformance Conformance { get; set; } = PdfAConformance.Level2B;

        /// <summary>Null/rỗng = không tạo lớp text OCR.</summary>
        public string OcrDataPath { get; set; }

        /// <summary>Mã ngôn ngữ Tesseract, ví dụ "eng+vie". Null = dùng mặc định của <see cref="OcrWordExtractor"/>.</summary>
        public string OcrLanguages { get; set; }

        /// <summary>Chạy bước tối ưu cấu trúc không đổi nội dung (qpdf) sau khi đóng gói. Cố gắng hết sức: nếu lỗi thì vẫn giữ nguyên PDF/A chưa tối ưu (vẫn hợp lệ).</summary>
        public bool Optimize { get; set; }

        /// <summary>Mặc định là <see cref="PdfArchiver"/>.</summary>
        public IPdfArchiver Archiver { get; set; }

        /// <summary>Mặc định là <see cref="QpdfStructuralOptimizer"/>.</summary>
        public IPdfOptimizer Optimizer { get; set; }
    }

    /// <summary>
    /// Điểm vào cấp cao "gộp các file ảnh này thành 1 PDF/A": chuẩn bị các trang giữ nguyên kiểu
    /// mã hoá gốc (<see cref="ArchivePageBuilder"/>), tuỳ chọn chạy OCR
    /// (<see cref="OcrWordExtractor"/>), đóng gói qua <see cref="IPdfArchiver"/>, và tuỳ chọn tối
    /// ưu cấu trúc kết quả.
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
        /// Chạy bộ tối ưu trên dữ liệu PDF/A đã tạo. Cố gắng hết sức: nếu có lỗi bất kỳ (thiếu
        /// công cụ tối ưu hoặc lỗi khi chạy) thì trả về nguyên dữ liệu PDF/A gốc (đã hợp lệ sẵn),
        /// không đổi gì - để bước tối ưu (vốn chỉ là tuỳ chọn) không bao giờ làm hỏng cả quá trình
        /// xuất file.
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
                // Giữ nguyên PDF/A chưa tối ưu (vẫn hợp lệ).
            }
            return pdfBytes;
        }
    }
}
