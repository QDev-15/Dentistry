using System.Collections.Generic;
using System.IO;
using ImageProcessing.Assembly;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Abstraction boundary for PDF/A generation - the rest of the library depends on this,
    /// never on a concrete PDF engine, so swapping engines stays a one-adapter change.
    /// </summary>
    public interface IPdfArchiver
    {
        /// <param name="pages">Prepared pages, in document order.</param>
        /// <param name="conformance">Target conformance level.</param>
        /// <param name="metadata">Document metadata (Info + XMP, set consistently).</param>
        /// <param name="output">Destination stream; the archiver writes the PDF and does not close it.</param>
        PdfArchiveResult CreatePdfA(IEnumerable<ArchivePage> pages, PdfAConformance conformance, DocumentMetadata metadata, Stream output);
    }

    /// <summary>Outcome of <see cref="IPdfArchiver.CreatePdfA"/>.</summary>
    public sealed class PdfArchiveResult
    {
        public PdfArchiveResult(bool success, int pageCount, PdfAConformance conformance, string message = null)
        {
            Success = success;
            PageCount = pageCount;
            Conformance = conformance;
            Message = message ?? string.Empty;
        }

        /// <summary>True when a document was produced. (Conformance is proven separately by a validator.)</summary>
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
