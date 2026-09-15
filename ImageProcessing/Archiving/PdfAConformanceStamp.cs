using System;
using System.Text;
using PdfSharp.Pdf;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Turns a built PdfSharp document into a PDF/A-conformant one.
    ///
    /// PdfSharp 6.2.x has undocumented PDF/A support via <see cref="PdfDocument.SetPdfA"/>: at
    /// save it injects an sRGB OutputIntent with an embedded ICC profile and generates an XMP
    /// packet (from the Info dictionary) that includes a pdfaid identification block - but it
    /// hardcodes that block to PDF/A-1a (part 1, conformance A). So PdfSharp is left to do the
    /// heavy lifting (OutputIntent, ICC embed, XMP-from-Info) and the conformance tag on the
    /// saved bytes is then corrected with a size-preserving patch (part 1-&gt;2/3, A-&gt;B/U):
    /// pdfaid part/conformance are single characters in an uncompressed XMP metadata stream, so
    /// no xref surgery is needed.
    ///
    /// Pinned to PdfSharp 6.2.4: <see cref="Apply"/> throws if the expected pdfaid block is
    /// absent, so a PdfSharp upgrade that changes the format fails loudly instead of silently
    /// shipping a mis-tagged file.
    /// </summary>
    internal static class PdfAConformanceStamp
    {
        private const string PartFrom = "<pdfaid:part>1</pdfaid:part>";
        private const string ConformanceFrom = "<pdfaid:conformance>A</pdfaid:conformance>";

        /// <summary>Enables PdfSharp's PDF/A output for the document. Call once, after pages/Info are set, before saving.</summary>
        public static void Enable(PdfDocument document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            document.SetPdfA();
        }

        /// <summary>Rewrites the saved PDF's pdfaid identification from PdfSharp's hardcoded PDF/A-1a to <paramref name="target"/>. Size-preserving.</summary>
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
