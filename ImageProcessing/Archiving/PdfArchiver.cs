using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ImageProcessing.Assembly;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Builds codec-faithful PDF/A image documents with PdfSharp 6.x: each page's already-encoded
    /// bytes are embedded verbatim as a low-level image XObject (matching PDF filter), so CCITT
    /// Group 4 / JPEG / JPEG2000 compression survives instead of being re-encoded through
    /// PdfSharp's high-level DrawImage (which round-trips pixels).
    /// </summary>
    public sealed class PdfArchiver : IPdfArchiver
    {
        public PdfArchiveResult CreatePdfA(IEnumerable<ArchivePage> pages, PdfAConformance conformance, DocumentMetadata metadata, Stream output)
        {
            if (pages == null) throw new ArgumentNullException(nameof(pages));
            if (output == null) throw new ArgumentNullException(nameof(output));

            DocumentMetadata meta = (metadata ?? new DocumentMetadata()).WithResolvedDates(DateTime.Now);

            int count = 0;
            using (PdfDocument document = new PdfDocument())
            {
                // PDF 1.7 (what PDF/A-2 is based on), and >= 1.4 so PdfSharp realizes the
                // transparent (alpha 0) fill the invisible OCR layer relies on.
                document.Version = 17;
                ApplyInfo(document, meta);

                foreach (ArchivePage page in pages)
                {
                    if (page == null) continue;
                    if (conformance.ForbidsJpeg2000() && page.Codec == PageCodec.Jpeg2000)
                        throw new NotSupportedException(conformance.DisplayName() + " forbids JPEG2000; re-encode the page (CCITT/JPEG/Flate) first.");

                    AddImagePage(document, page);
                    count++;
                }

                if (count == 0)
                    throw new InvalidOperationException("No pages supplied to the archiver.");

                PdfAConformanceStamp.Enable(document);

                using (MemoryStream buffer = new MemoryStream())
                {
                    document.Save(buffer, false);
                    byte[] pdfBytes = PdfAConformanceStamp.Apply(buffer.ToArray(), conformance);
                    output.Write(pdfBytes, 0, pdfBytes.Length);
                }
            }

            return PdfArchiveResult.Ok(count, conformance);
        }

        private static void ApplyInfo(PdfDocument document, DocumentMetadata meta)
        {
            document.Info.Title = meta.Title ?? string.Empty;
            document.Info.Author = meta.Author ?? string.Empty;
            document.Info.Subject = meta.Subject ?? string.Empty;
            document.Info.Keywords = meta.Keywords ?? string.Empty;
            document.Info.Creator = meta.Creator ?? string.Empty;
            if (!string.IsNullOrEmpty(meta.Producer))
                document.Info.Elements.SetString("/Producer", meta.Producer);
            if (meta.CreateDate.HasValue)
                document.Info.CreationDate = meta.CreateDate.Value;
            if (meta.ModifyDate.HasValue)
                document.Info.ModificationDate = meta.ModifyDate.Value;
        }

        private static void AddImagePage(PdfDocument document, ArchivePage page)
        {
            PdfPage pdfPage = document.AddPage();
            pdfPage.Width = PdfSharp.Drawing.XUnit.FromPoint(page.WidthPt);
            pdfPage.Height = PdfSharp.Drawing.XUnit.FromPoint(page.HeightPt);

            // Z-order invisibility: draw searchable text first, opaque image over it. No-op
            // when the page carries no OCR words.
            OcrTextOverlay.Draw(pdfPage, page);

            PdfDictionary image = BuildImageXObject(document, page);
            document.Internals.AddObject(image);

            const string name = "/Im0";
            XObjectResources(document, pdfPage).Elements[name] = image.Reference;

            string content = string.Format(CultureInfo.InvariantCulture,
                "q\n{0:0.####} 0 0 {1:0.####} 0 0 cm\n{2} Do\nQ\n", page.WidthPt, page.HeightPt, name);

            PdfContent pdfContent = pdfPage.Contents.AppendContent();
            pdfContent.CreateStream(Encoding.ASCII.GetBytes(content));
        }

        private static PdfDictionary XObjectResources(PdfDocument document, PdfPage page)
        {
            PdfDictionary xobjects = page.Resources.Elements.GetDictionary("/XObject");
            if (xobjects == null)
            {
                xobjects = new PdfDictionary(document);
                page.Resources.Elements["/XObject"] = xobjects;
            }
            return xobjects;
        }

        /// <summary>Builds an image XObject whose stream is the page's encoded bytes verbatim, tagged with the matching PDF filter.</summary>
        private static PdfDictionary BuildImageXObject(PdfDocument document, ArchivePage page)
        {
            PdfDictionary image = new PdfDictionary(document);
            image.CreateStream(page.ImageData);

            var e = image.Elements;
            e.SetName("/Type", "/XObject");
            e.SetName("/Subtype", "/Image");
            e.SetInteger("/Width", page.WidthPx);
            e.SetInteger("/Height", page.HeightPx);

            switch (page.Codec)
            {
                case PageCodec.Jpeg:
                    e.SetName("/Filter", "/DCTDecode");
                    e.SetInteger("/BitsPerComponent", 8);
                    e.SetName("/ColorSpace", DeviceColorSpace(page.ColorSpace));
                    break;

                case PageCodec.CcittGroup4:
                    e.SetName("/Filter", "/CCITTFaxDecode");
                    e.SetInteger("/BitsPerComponent", 1);
                    e.SetName("/ColorSpace", "/DeviceGray");
                    e["/DecodeParms"] = CcittParms(document, page);
                    break;

                case PageCodec.Jpeg2000:
                    e.SetName("/Filter", "/JPXDecode");
                    e.SetInteger("/BitsPerComponent", 8);
                    // JPXDecode carries its own colour space in the JP2 header.
                    break;

                case PageCodec.Flate:
                    e.SetName("/Filter", "/FlateDecode");
                    e.SetInteger("/BitsPerComponent", page.ColorSpace == PageColorSpace.Bitonal ? 1 : 8);
                    e.SetName("/ColorSpace", DeviceColorSpace(page.ColorSpace));
                    break;

                default:
                    throw new NotSupportedException("Unsupported image codec: " + page.Codec);
            }

            return image;
        }

        private static PdfDictionary CcittParms(PdfDocument document, ArchivePage page)
        {
            PdfDictionary dp = new PdfDictionary(document);
            dp.Elements.SetInteger("/K", -1); // pure Group 4 (2-D)
            dp.Elements.SetInteger("/Columns", page.WidthPx);
            dp.Elements.SetInteger("/Rows", page.HeightPx);
            // Every G4 stream this library produces (ImageProcessing.Documents.TiffCodec) is
            // written/re-encoded to MinIsWhite (0 = white) regardless of the source's own bit
            // polarity - see BitonalPolarityDetector - so BlackIs1=false always matches here.
            dp.Elements.SetBoolean("/BlackIs1", false);
            return dp;
        }

        private static string DeviceColorSpace(PageColorSpace colorSpace)
        {
            switch (colorSpace)
            {
                case PageColorSpace.Bitonal:
                case PageColorSpace.Gray:
                    return "/DeviceGray";
                case PageColorSpace.Rgb:
                    return "/DeviceRGB";
                case PageColorSpace.Cmyk:
                    return "/DeviceCMYK";
                default:
                    throw new NotSupportedException("Unsupported colour space: " + colorSpace);
            }
        }
    }
}
