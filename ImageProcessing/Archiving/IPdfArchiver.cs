using System.Collections.Generic;
using System.IO;
using ImageProcessing.Assembly;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Ranh giới trừu tượng cho việc tạo PDF/A - phần còn lại của thư viện chỉ phụ thuộc vào
    /// interface này, không bao giờ phụ thuộc trực tiếp vào một bộ máy PDF cụ thể, nên đổi bộ
    /// máy chỉ cần sửa đúng 1 adapter.
    /// </summary>
    public interface IPdfArchiver
    {
        /// <param name="pages">Các trang đã chuẩn bị sẵn, theo đúng thứ tự trong tài liệu.</param>
        /// <param name="conformance">Mức chuẩn PDF/A cần đạt.</param>
        /// <param name="metadata">Metadata của tài liệu (Info + XMP, được thiết lập nhất quán).</param>
        /// <param name="output">Stream đích; bộ đóng gói chỉ ghi PDF vào đây, không tự đóng stream.</param>
        PdfArchiveResult CreatePdfA(IEnumerable<ArchivePage> pages, PdfAConformance conformance, DocumentMetadata metadata, Stream output);
    }

    /// <summary>Kết quả trả về của <see cref="IPdfArchiver.CreatePdfA"/>.</summary>
    public sealed class PdfArchiveResult
    {
        public PdfArchiveResult(bool success, int pageCount, PdfAConformance conformance, string message = null)
        {
            Success = success;
            PageCount = pageCount;
            Conformance = conformance;
            Message = message ?? string.Empty;
        }

        /// <summary>True khi đã tạo ra được tài liệu. (Việc có thực sự đạt chuẩn hay không do một validator riêng kiểm chứng.)</summary>
        public bool Success { get; }

        public int PageCount { get; }

        public PdfAConformance Conformance { get; }

        public string Message { get; }

        public static PdfArchiveResult Ok(int pageCount, PdfAConformance conformance)
        {
            return new PdfArchiveResult(true, pageCount, conformance);
        }
    }
}
