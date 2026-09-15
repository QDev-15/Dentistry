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
    /// <summary>Kết xuất/kiểm tra trang PDF (dùng Pdfium) và đóng gói PDF thường - không phải PDF/A (dùng PdfSharp).</summary>
    internal static class PdfRaster
    {
        // Pdfium (bộ máy native đứng sau PdfiumViewer) không an toàn khi gọi đồng thời từ nhiều
        // luồng, kể cả giữa các instance PdfiumDocument độc lập với nhau - đã từng quan sát thấy
        // hiện tượng hỏng trạng thái hoặc crash hẳn tiến trình khi bị gọi đồng thời. Một ứng dụng
        // web mặc định xử lý nhiều request cùng lúc, nên mọi lệnh gọi Pdfium native trong thư
        // viện này đều phải xếp hàng qua đúng 1 lock này. Việc render trang được làm ngay (không
        // dùng iterator trễ) chính là để cả chuỗi "mở file + render hết mọi trang" chạy trong 1
        // lần giữ lock, thay vì phải giành lại lock cho từng trang.
        private static readonly object PdfiumLock = new object();

        /// <summary>Render mọi trang của 1 PDF ra <see cref="Bitmap"/> theo đúng DPI cho trước, theo đúng thứ tự.</summary>
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
        /// True khi PDF là form XFA động (Adobe LiveCycle): nội dung thật của nó chỉ nằm trong
        /// template XFA, không nằm trong luồng nội dung của trang, nên bất kỳ bộ kết xuất ảnh nào
        /// (kể cả Pdfium) cũng chỉ chụp được đúng trang giữ chỗ kiểu "vui lòng nâng cấp trình xem
        /// PDF của bạn".
        /// Phát hiện qua cờ /NeedsRendering trong catalog, hoặc /AcroForm có /XFA mà không có lớp
        /// dự phòng /Fields thông thường.
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
                // Để luồng render bình thường tự chạy và tự báo lỗi của nó, thay vì chặn ở đây.
                return false;
            }
        }

        /// <summary>
        /// Tạo 1 PDF thường (không phải PDF/A), mỗi file/khung hình nguồn chiếm trọn 1 trang.
        /// Muốn xuất PDF/A đạt chuẩn lưu trữ (giữ nguyên codec, tuỳ chọn OCR) thì dùng
        /// <c>ImageProcessing.Archiving.PdfADocumentBuilder</c> thay cho hàm này.
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
                // Bản PdfSharp 6.x (build netstandard2.0) không có cách nhập ảnh GDI+ trực tiếp;
                // phải vòng qua 1 luồng PNG không mất dữ liệu thay thế.
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
