using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ImageProcessing.Assembly;
using PdfiumViewer;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfiumDocument = PdfiumViewer.PdfDocument;
using PdfSharpDocument = PdfSharp.Pdf.PdfDocument;

namespace ImageProcessing.Documents
{
    /// <summary>PDF page rendering/inspection (Pdfium) and plain (non-archival) PDF assembly (PdfSharp).</summary>
    internal static class PdfRaster
    {
        // Pdfium (the native engine behind PdfiumViewer) is not safe to call concurrently from
        // multiple threads, even across independent PdfiumDocument instances - it has been
        // observed to corrupt state or crash the process outright under concurrent access. A web
        // host serves requests concurrently by default, so every native Pdfium call in this
        // library is serialized behind this single lock. Rendering pages is eager (not a lazy
        // iterator) specifically so the whole "open + render every page" sequence can run under
        // one lock acquisition instead of re-entering the lock per page.
        private static readonly object PdfiumLock = new object();

        /// <summary>Renders every page of a PDF to a <see cref="Bitmap"/> at the given DPI, in order.</summary>
        public static List<Bitmap> RenderPages(string pdfPath, int dpi)
        {
            lock (PdfiumLock)
            {
                List<Bitmap> pages = new List<Bitmap>();
                using (PdfiumDocument doc = PdfiumDocument.Load(pdfPath))
                {
                    for (int i = 0; i < doc.PageCount; i++)
                    {
                        using (Image img = doc.Render(i, dpi, dpi, PdfRenderFlags.CorrectFromDpi | PdfRenderFlags.Annotations))
                            pages.Add(new Bitmap(img));
                    }
                }
                return pages;
            }
        }

        public static int GetPageCount(string pdfPath)
        {
            lock (PdfiumLock)
            using (PdfiumDocument doc = PdfiumDocument.Load(pdfPath))
                return doc.PageCount;
        }

        public static void GetFirstPageSizePoints(string pdfPath, out float heightPt, out float widthPt)
        {
            using (PdfSharpDocument doc = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Import))
            {
                PdfPage page = doc.Pages[0];
                heightPt = (float)page.Height.Point;
                widthPt = (float)page.Width.Point;
            }
        }

        /// <summary>
        /// True when the PDF is a dynamic XFA form (Adobe LiveCycle): its real content lives only
        /// in the XFA template, not the page content stream, so a raster renderer (Pdfium included)
        /// can only capture the "please upgrade your viewer" placeholder page.
        /// Detected via the catalog's /NeedsRendering flag, or an /AcroForm carrying /XFA with no
        /// ordinary /Fields fallback layer.
        /// </summary>
        public static bool IsDynamicXfaUnsupported(string pdfPath)
        {
            try
            {
                using (PdfSharpDocument doc = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Import))
                {
                    PdfDictionary catalog = doc.Internals.Catalog;
                    if (catalog == null) return false;
                    if (catalog.Elements.GetBoolean("/NeedsRendering")) return true;

                    PdfDictionary acroForm = catalog.Elements.GetDictionary("/AcroForm");
                    if (acroForm == null || !acroForm.Elements.ContainsKey("/XFA")) return false;

                    PdfArray fields = acroForm.Elements.GetArray("/Fields");
                    return fields == null || fields.Elements.Count == 0;
                }
            }
            catch
            {
                // Let the normal render path run and surface its own error instead of blocking here.
                return false;
            }
        }

        /// <summary>
        /// Builds a plain (non-PDF/A) PDF with one full-page image per source file/frame. For
        /// archival-grade PDF/A output (codec-preserving, optional OCR), use
        /// <c>ImageProcessing.Archiving.PdfADocumentBuilder</c> instead.
        /// </summary>
        public static void CreateImagePdf(IEnumerable<string> imageFiles, string destFile, DocumentMetadata metadata)
        {
            using (PdfSharpDocument pdf = new PdfSharpDocument())
            {
                metadata = metadata ?? new DocumentMetadata();
                pdf.Info.Title = metadata.Title ?? "";
                pdf.Info.Author = metadata.Author ?? "";
                pdf.Info.Subject = metadata.Subject ?? "";
                pdf.Info.Keywords = metadata.Keywords ?? "";
                pdf.Info.Creator = metadata.Creator ?? "";

                foreach (string imageFile in imageFiles)
                    foreach (Bitmap frame in RasterFrameSource.EnumerateFrames(imageFile))
                        using (frame)
                            AddImagePage(pdf, frame);

                pdf.Save(destFile);
            }
        }

        private static void AddImagePage(PdfSharpDocument pdf, Bitmap frame)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // PdfSharp 6.x (netstandard2.0 build) has no direct GDI+ Image import;
                // round-trip through a lossless PNG stream instead.
                frame.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                using (XImage xImage = XImage.FromStream(ms))
                {
                    PdfPage page = pdf.AddPage();
                    page.Width = XUnit.FromPoint(xImage.PointWidth);
                    page.Height = XUnit.FromPoint(xImage.PointHeight);
                    using (XGraphics gfx = XGraphics.FromPdfPage(page))
                        gfx.DrawImage(xImage, 0, 0, page.Width.Point, page.Height.Point);
                }
            }
        }

    }
}
