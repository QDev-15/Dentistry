using System;

namespace ImageProcessing.Assembly
{
    /// <summary>
    /// Metadata tài liệu, dùng chung cho cả bộ tạo PDF thường (<see cref="Documents.PdfRaster"/>)
    /// lẫn bộ đóng gói PDF/A (<c>ImageProcessing.Archiving.PdfADocumentBuilder</c>). Với PDF/A,
    /// việc có 1 nguồn dữ liệu duy nhất rất quan trọng: Title/Author/ngày tháng lệch nhau giữa
    /// dictionary Info và gói XMP là lỗi phổ biến khiến veraPDF báo không đạt chuẩn, nên cả 2 nơi
    /// dùng đều lấy từ đúng cùng 1 bộ giá trị này.
    /// </summary>
    public sealed class DocumentMetadata
    {
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Keywords { get; set; } = string.Empty;

        public string Creator { get; set; } = "ImageProcessing";

        public string Producer { get; set; } = "ImageProcessing";

        /// <summary>Null = dùng thời điểm hiện tại lúc tạo tài liệu.</summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>Null = dùng thời điểm hiện tại lúc tạo tài liệu.</summary>
        public DateTime? ModifyDate { get; set; }

        public DocumentMetadata WithResolvedDates(DateTime now)
        {
            return new DocumentMetadata
            {
                Title = Title,
                Author = Author,
                Subject = Subject,
                Keywords = Keywords,
                Creator = Creator,
                Producer = Producer,
                CreateDate = CreateDate ?? now,
                ModifyDate = ModifyDate ?? now
            };
        }
    }
}
