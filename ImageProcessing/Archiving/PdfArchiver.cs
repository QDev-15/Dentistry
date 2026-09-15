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
    /// Đóng gói tài liệu ảnh PDF/A giữ nguyên đúng codec bằng PdfSharp 6.x: dữ liệu đã mã hoá sẵn
    /// của mỗi trang được nhúng nguyên trạng dưới dạng image XObject cấp thấp (đúng bộ lọc PDF
    /// tương ứng), để nén CCITT Group 4 / JPEG / JPEG2000 được giữ nguyên thay vì bị mã hoá lại
    /// qua DrawImage cấp cao của PdfSharp (vốn giải mã rồi vẽ lại từng điểm ảnh).
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
                // PDF 1.7 (nền tảng của PDF/A-2), và phải >= 1.4 thì PdfSharp mới xử lý đúng phần
                // tô trong suốt (alpha 0) mà lớp OCR vô hình cần dùng.
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

            // Ẩn bằng thứ tự vẽ (z-order): vẽ text tìm-kiếm-được trước, sau đó vẽ ảnh không
            // trong suốt đè lên trên. Không làm gì nếu trang không có từ OCR nào.
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

        /// <summary>Tạo 1 image XObject có luồng dữ liệu chính là bytes đã mã hoá sẵn của trang, giữ nguyên trạng, gắn đúng bộ lọc PDF tương ứng.</summary>
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
                    // JPXDecode tự mang theo thông tin không gian màu ngay trong header JP2.
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
            dp.Elements.SetInteger("/K", -1); // Group 4 thuần (2 chiều)
            dp.Elements.SetInteger("/Columns", page.WidthPx);
            dp.Elements.SetInteger("/Rows", page.HeightPx);
            // Mọi luồng G4 mà thư viện này tạo ra (ImageProcessing.Documents.TiffCodec) đều được
            // ghi/mã hoá lại theo chuẩn MinIsWhite (0 = trắng) bất kể ảnh gốc dùng cực tính bit
            // nào - xem BitonalPolarityDetector - nên BlackIs1=false luôn đúng ở đây.
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
