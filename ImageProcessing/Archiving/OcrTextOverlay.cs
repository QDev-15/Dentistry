using System;
using System.IO;
using System.Reflection;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace ImageProcessing.Archiving
{
    /// <summary>Serves the single embedded OCR-layer font (DejaVu Sans - covers Vietnamese) for every request.</summary>
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
    /// Places an invisible, searchable OCR text layer over an archiver page. PdfSharp has no
    /// text-render-mode (Tr 3) API and its renderer will not emit an alpha-0 fill (it realizes
    /// alpha 0 as a no-op, leaving the text opaque), so invisibility is achieved by z-order
    /// instead: the OCR words are drawn FIRST, then the opaque page image is drawn on top,
    /// hiding them. The text stays in the content stream (selectable, searchable) but never
    /// renders visibly.
    /// </summary>
    internal static class OcrTextOverlay
    {
        private static readonly object Gate = new object();
        private static bool _resolverReady;

        /// <summary>Draws <paramref name="page"/>'s OCR words onto <paramref name="pdfPage"/>. Call BEFORE the image content is appended.</summary>
        public static void Draw(PdfPage pdfPage, ArchivePage page)
        {
            if (pdfPage == null) throw new ArgumentNullException(nameof(pdfPage));
            if (page == null || !page.HasText) return;

            EnsureFontResolver();

            XBrush brush = XBrushes.Black; // colour is irrelevant - the image paints over it.
            using (XGraphics gfx = XGraphics.FromPdfPage(pdfPage, XGraphicsPdfPageOptions.Append))
            {
                foreach (OcrWord word in page.OcrWords)
                {
                    if (word == null || string.IsNullOrEmpty(word.Text))
                        continue;

                    double heightPt = word.BoundingBox.Height / page.DpiY * 72.0;
                    if (heightPt <= 0)
                        continue;

                    double sizePt = Math.Max(1.0, heightPt * 0.8); // cap height ~= 0.7 em
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
