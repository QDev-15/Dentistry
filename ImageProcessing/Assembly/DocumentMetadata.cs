using System;

namespace ImageProcessing.Assembly
{
    /// <summary>
    /// Document metadata shared by both the plain PDF writer (<see cref="Documents.PdfRaster"/>)
    /// and the PDF/A archiver (<c>ImageProcessing.Archiving.PdfADocumentBuilder</c>). For PDF/A,
    /// a single source of truth matters: a Title/Author/date mismatch between the Info dictionary
    /// and the XMP packet is a frequent veraPDF conformance failure, so both consumers set both
    /// from these same values.
    /// </summary>
    public sealed class DocumentMetadata
    {
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Keywords { get; set; } = string.Empty;

        public string Creator { get; set; } = "ImageProcessing";

        public string Producer { get; set; } = "ImageProcessing";

        /// <summary>Null = use "now" at build time.</summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>Null = use "now" at build time.</summary>
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
