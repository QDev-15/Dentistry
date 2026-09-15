using System;
using System.IO;
using System.Reflection;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace ImageProcessing.Archiving
{
    /// <summary>Phục vụ đúng 1 font OCR nhúng sẵn (DejaVu Sans - hỗ trợ tiếng Việt) cho mọi yêu cầu.</summary>
    internal sealed class OcrFontResolver : IFontResolver
    {
        public const string FamilyName = "ImageProcessing OCR Text";

        private const string FaceName = "ImageProcessing-OCR-Text";
        private const string FontResourceSuffix = ".DejaVuSans.ttf";
        private static readonly byte[] FontData = LoadFont();

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo(FaceName);
        }

        public byte[] GetFont(string faceName)
        {
            return FontData;
        }

        private static byte[] LoadFont()
        {
            System.Reflection.Assembly asm = typeof(OcrFontResolver).Assembly;
            string name = null;
            foreach (string candidate in asm.GetManifestResourceNames())
            {
                if (candidate.EndsWith(FontResourceSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    name = candidate;
                    break;
                }
            }
            if (name == null)
                throw new InvalidOperationException("Embedded OCR font not found (expected a resource ending '" + FontResourceSuffix + "').");

            using (Stream s = asm.GetManifestResourceStream(name))
            using (MemoryStream ms = new MemoryStream())
            {
                s.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }

    /// <summary>
    /// Đặt lớp text OCR vô hình (nhưng vẫn tìm kiếm/chọn được) đè lên 1 trang trong bộ đóng gói.
    /// PdfSharp không có API cho text-render-mode (Tr 3), và bộ vẽ của nó cũng không xử lý được
    /// alpha-0 (nó coi alpha 0 là không làm gì cả, khiến chữ vẫn hiện rõ), nên phải làm ẩn bằng
    /// cách khác: vẽ CHỮ OCR TRƯỚC, sau đó vẽ ảnh trang (không trong suốt) đè LÊN TRÊN để che đi.
    /// Chữ vẫn nằm trong nội dung PDF (chọn được, tìm kiếm được) nhưng không bao giờ hiển thị ra
    /// mắt.
    /// </summary>
    internal static class OcrTextOverlay
    {
        private static readonly object Gate = new object();
        private static bool _resolverReady;

        /// <summary>Vẽ các từ OCR của <paramref name="page"/> lên <paramref name="pdfPage"/>. Phải gọi TRƯỚC khi thêm nội dung ảnh của trang.</summary>
        public static void Draw(PdfPage pdfPage, ArchivePage page)
        {
            if (pdfPage == null) throw new ArgumentNullException(nameof(pdfPage));
            if (page == null || !page.HasText) return;

            EnsureFontResolver();

            XBrush brush = XBrushes.Black; // màu không quan trọng - sẽ bị ảnh vẽ đè lên che mất.
            using (XGraphics gfx = XGraphics.FromPdfPage(pdfPage, XGraphicsPdfPageOptions.Append))
            {
                foreach (OcrWord word in page.OcrWords)
                {
                    if (word == null || string.IsNullOrEmpty(word.Text))
                        continue;

                    double heightPt = word.BoundingBox.Height / page.DpiY * 72.0;
                    if (heightPt <= 0)
                        continue;

                    double sizePt = Math.Max(1.0, heightPt * 0.8); // chiều cao chữ ~= 0.7 em
                    double xPt = word.BoundingBox.Left / page.DpiX * 72.0;

                    double baselinePx = word.Baseline ?? (word.BoundingBox.Top + word.BoundingBox.Height);
                    double baselinePt = baselinePx / page.DpiY * 72.0;

                    XFont font = new XFont(OcrFontResolver.FamilyName, sizePt);
                    gfx.DrawString(word.Text, font, brush, new XPoint(xPt, baselinePt), XStringFormats.BaseLineLeft);
                }
            }
        }

        private static void EnsureFontResolver()
        {
            if (_resolverReady) return;
            lock (Gate)
            {
                if (_resolverReady) return;
                if (GlobalFontSettings.FontResolver == null)
                    GlobalFontSettings.FontResolver = new OcrFontResolver();
                _resolverReady = true;
            }
        }
    }
}
