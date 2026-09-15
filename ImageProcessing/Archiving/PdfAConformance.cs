using System;

namespace ImageProcessing.Archiving
{
    /// <summary>PDF/A conformance level to build/validate against. Default across the pipeline is PDF/A-2b.</summary>
    public enum PdfAConformance
    {
        /// <summary>PDF/A-1b (PDF 1.4). No JPEG2000, no transparency, no layers.</summary>
        Level1B,

        /// <summary>PDF/A-2b (PDF 1.7). Default. Allows JPEG2000 / JBIG2 / transparency.</summary>
        Level2B,

        /// <summary>PDF/A-2u. As 2b, but every glyph must be Unicode-mapped (ToUnicode).</summary>
        Level2U,

        /// <summary>PDF/A-3b. As 2b, but permits arbitrary embedded (associated) files.</summary>
        Level3B
    }

    /// <summary>Projects a <see cref="PdfAConformance"/> onto the ISO metadata values used by the OutputIntent/XMP pass and the veraPDF CLI.</summary>
    public static class PdfAConformanceInfo
    {
        /// <summary>The PDF/A part number (1, 2 or 3) for <c>pdfaid:part</c>.</summary>
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

        /// <summary>The conformance letter ("B" or "U") for <c>pdfaid:conformance</c>.</summary>
        public static string ConformanceLetter(this PdfAConformance level)
        {
            return level == PdfAConformance.Level2U ? "U" : "B";
        }

        /// <summary>True when every glyph must carry a ToUnicode mapping.</summary>
        public static bool RequiresUnicodeMapping(this PdfAConformance level)
        {
            return level == PdfAConformance.Level2U;
        }

        /// <summary>True when the level forbids JPEG2000 and transparency (PDF/A-1 only).</summary>
        public static bool ForbidsJpeg2000(this PdfAConformance level)
        {
            return level == PdfAConformance.Level1B;
        }

        /// <summary>Human/veraPDF label, e.g. "2b" (the veraPDF <c>--flavour</c> value) or "PDF/A-2b" via <see cref="DisplayName"/>.</summary>
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
