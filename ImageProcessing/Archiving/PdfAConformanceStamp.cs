using System;
using System.Text;
using PdfSharp.Pdf;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Biến 1 tài liệu PdfSharp đã tạo sẵn thành tài liệu đạt chuẩn PDF/A.
    ///
    /// PdfSharp 6.2.x có hỗ trợ PDF/A nhưng không tài liệu hoá công khai, qua
    /// <see cref="PdfDocument.SetPdfA"/>: lúc lưu file nó tự chèn 1 OutputIntent sRGB kèm ICC
    /// profile nhúng sẵn và tự sinh gói XMP (từ dictionary Info) có kèm khối nhận dạng pdfaid -
    /// nhưng nó luôn ghi cứng khối đó là PDF/A-1a (phần 1, mức A). Vậy nên để PdfSharp làm phần
    /// việc nặng (OutputIntent, nhúng ICC, sinh XMP từ Info), rồi sau đó vá lại đúng nhãn mức
    /// chuẩn trên dữ liệu đã lưu (phần 1->2/3, A->B/U) theo cách giữ nguyên kích thước file:
    /// pdfaid part/conformance chỉ là vài ký tự đơn trong 1 luồng metadata XMP không nén, nên
    /// không cần đụng tới cấu trúc xref của file.
    ///
    /// Ghim ở bản PdfSharp 6.2.4: <see cref="Apply"/> sẽ ném lỗi nếu không tìm thấy đúng khối
    /// pdfaid mong đợi, để khi nâng cấp PdfSharp mà định dạng bị đổi thì báo lỗi ngay, thay vì âm
    /// thầm xuất ra file gắn sai nhãn.
    /// </summary>
    internal static class PdfAConformanceStamp
    {
        private const string PartFrom = "<pdfaid:part>1</pdfaid:part>";
        private const string ConformanceFrom = "<pdfaid:conformance>A</pdfaid:conformance>";

        /// <summary>Bật chế độ xuất PDF/A của PdfSharp cho tài liệu. Gọi đúng 1 lần, sau khi đã thiết lập xong trang/Info, trước khi lưu.</summary>
        public static void Enable(PdfDocument document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            document.SetPdfA();
        }

        /// <summary>Viết lại khối nhận dạng pdfaid của PDF đã lưu, từ mức PDF/A-1a mà PdfSharp ghi cứng sang <paramref name="target"/>. Giữ nguyên kích thước file.</summary>
        public static byte[] Apply(byte[] pdfBytes, PdfAConformance target)
        {
            if (pdfBytes == null) throw new ArgumentNullException(nameof(pdfBytes));

            string partTo = "<pdfaid:part>" + target.Part() + "</pdfaid:part>";
            string conformanceTo = "<pdfaid:conformance>" + target.ConformanceLetter() + "</pdfaid:conformance>";

            ReplaceAsciiOnce(pdfBytes, PartFrom, partTo, "pdfaid:part");
            ReplaceAsciiOnce(pdfBytes, ConformanceFrom, conformanceTo, "pdfaid:conformance");
            return pdfBytes;
        }

        private static void ReplaceAsciiOnce(byte[] buffer, string from, string to, string what)
        {
            byte[] fromBytes = Encoding.ASCII.GetBytes(from);
            byte[] toBytes = Encoding.ASCII.GetBytes(to);
            if (fromBytes.Length != toBytes.Length)
                throw new InvalidOperationException("Non size-preserving conformance patch for " + what + ".");

            int at = IndexOf(buffer, fromBytes);
            if (at < 0)
                throw new InvalidOperationException(
                    "Expected PdfSharp pdfaid block ('" + from + "') not found - PdfSharp's XMP format may have changed (" + what + ").");

            Buffer.BlockCopy(toBytes, 0, buffer, at, toBytes.Length);
        }

        private static int IndexOf(byte[] haystack, byte[] needle)
        {
            int limit = haystack.Length - needle.Length;
            for (int i = 0; i <= limit; i++)
            {
                int j = 0;
                while (j < needle.Length && haystack[i + j] == needle[j])
                    j++;
                if (j == needle.Length)
                    return i;
            }
            return -1;
        }
    }
}
