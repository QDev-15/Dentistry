using System;
using System.Drawing;

namespace ImageProcessing.Imaging
{
    /// <summary>
    /// Which bit value represents ink in a 1bpp-indexed <see cref="Bitmap"/>. GDI+ does not
    /// force a fixed convention: it preserves whatever palette the source format implied
    /// (e.g. a MinIsWhite-tagged TIFF decodes with palette[0]=White, palette[1]=Black), so
    /// code that inspects raw packed bits must resolve this per-bitmap rather than assume
    /// a fixed "index 0 is black" layout.
    /// </summary>
    internal enum BitonalPolarity
    {
        /// <summary>Bit value 0 is ink (dark); bit value 1 is paper (light).</summary>
        ZeroIsInk,

        /// <summary>Bit value 1 is ink (dark); bit value 0 is paper (light).</summary>
        OneIsInk
    }

    internal static class BitonalPolarityDetector
    {
        /// <summary>
        /// Inspects the two palette entries of a 1bpp-indexed bitmap and returns which bit
        /// value is the darker (ink) one. Falls back to <see cref="BitonalPolarity.OneIsInk"/>
        /// (GDI+'s own default palette for a freshly allocated 1bpp bitmap) when the palette
        /// is missing or degenerate.
        /// </summary>
        public static BitonalPolarity Detect(Bitmap bitonal)
        {
            if (bitonal == null) throw new ArgumentNullException(nameof(bitonal));

            Color[] entries = bitonal.Palette?.Entries;
            if (entries == null || entries.Length < 2)
                return BitonalPolarity.OneIsInk;

            double lum0 = Luminance(entries[0]);
            double lum1 = Luminance(entries[1]);

            // Degenerate/identical palette: keep the conventional default rather than guess.
            if (Math.Abs(lum0 - lum1) < 1.0)
                return BitonalPolarity.OneIsInk;

            return lum0 < lum1 ? BitonalPolarity.ZeroIsInk : BitonalPolarity.OneIsInk;
        }

        private static double Luminance(Color c)
        {
            return (c.R + c.G + c.B) / 3.0;
        }
    }
}
