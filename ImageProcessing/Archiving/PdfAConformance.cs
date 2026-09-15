using System;

namespace ImageProcessing.Archiving
{
    /// <summary>Mức chuẩn PDF/A cần tạo/kiểm tra. Mặc định trong toàn bộ pipeline là PDF/A-2b.</summary>
    public enum PdfAConformance
    {
        /// <summary>PDF/A-1b (PDF 1.4). Không JPEG2000, không trong suốt, không layer.</summary>
        Level1B,

        /// <summary>PDF/A-2b (PDF 1.7). Mặc định. Cho phép JPEG2000 / JBIG2 / trong suốt.</summary>
        Level2B,

        /// <summary>PDF/A-2u. Giống 2b, nhưng mọi glyph đều phải có ánh xạ Unicode (ToUnicode).</summary>
        Level2U,

        /// <summary>PDF/A-3b. Giống 2b, nhưng cho phép nhúng kèm file bất kỳ (associated files).</summary>
        Level3B
    }

    /// <summary>Quy đổi <see cref="PdfAConformance"/> sang các giá trị metadata chuẩn ISO dùng cho bước OutputIntent/XMP và cho công cụ dòng lệnh veraPDF.</summary>
    public static class PdfAConformanceInfo
    {
        /// <summary>Số phần PDF/A (1, 2 hoặc 3) cho <c>pdfaid:part</c>.</summary>
        public static int Part(this PdfAConformance level)
        {
            switch (level)
            {
                case PdfAConformance.Level1B: return 1;
                case PdfAConformance.Level2B: return 2;
                case PdfAConformance.Level2U: return 2;
                case PdfAConformance.Level3B: return 3;
                default: throw new ArgumentOutOfRangeException(nameof(level));
            }
        }

        /// <summary>Ký tự mức chuẩn ("B" hoặc "U") cho <c>pdfaid:conformance</c>.</summary>
        public static string ConformanceLetter(this PdfAConformance level)
        {
            return level == PdfAConformance.Level2U ? "U" : "B";
        }

        /// <summary>True khi mọi glyph bắt buộc phải có ánh xạ ToUnicode.</summary>
        public static bool RequiresUnicodeMapping(this PdfAConformance level)
        {
            return level == PdfAConformance.Level2U;
        }

        /// <summary>True khi mức này cấm JPEG2000 và trong suốt (chỉ áp dụng cho PDF/A-1).</summary>
        public static bool ForbidsJpeg2000(this PdfAConformance level)
        {
            return level == PdfAConformance.Level1B;
        }

        /// <summary>Nhãn hiển thị/dùng cho veraPDF, ví dụ "2b" (giá trị tham số <c>--flavour</c> của veraPDF), hoặc "PDF/A-2b" qua <see cref="DisplayName"/>.</summary>
        public static string Flavour(this PdfAConformance level)
        {
            return level.Part() + level.ConformanceLetter().ToLowerInvariant();
        }

        public static string DisplayName(this PdfAConformance level)
        {
            return "PDF/A-" + level.Flavour();
        }
    }
}
