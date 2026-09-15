using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ImageProcessing.Archiving;
using Tesseract;

namespace ImageProcessing.Ocr
{
    /// <summary>
    /// Chạy engine Tesseract trên 1 ảnh trang và trả về các từ nhận dạng được (nội dung chữ +
    /// khung toạ độ pixel), sau đó được đặt làm lớp text ẩn có thể tìm kiếm thông qua
    /// <c>OcrTextOverlay</c>.
    ///
    /// tessdata: dữ liệu ngôn ngữ (eng, vie, ...) phải nằm trực tiếp trong
    /// <paramref name="tessDataPath"/> (thư mục CHỨA các file *.traineddata) - xem constructor.
    ///
    /// Lưu ý khi triển khai: lớp này chỉ cần Tesseract.dll (managed) để build; lúc chạy còn cần
    /// các thư viện native của Tesseract (leptonica-*.dll / tesseract*.dll) nằm trong thư mục con
    /// "x86"/"x64" của thư mục output ứng dụng sử dụng - xem README của thư viện để biết cách
    /// triển khai (các file này không được nhúng sẵn trong DLL này).
    /// </summary>
    public sealed class OcrWordExtractor : IDisposable
    {
        private const string DefaultLanguages = "eng+vie";

        private readonly TesseractEngine _engine;

        public OcrWordExtractor(string tessDataPath, string languages = DefaultLanguages)
        {
            if (string.IsNullOrWhiteSpace(tessDataPath))
                throw new ArgumentException("tessDataPath is required for OCR.", nameof(tessDataPath));
            if (!Directory.Exists(tessDataPath))
                throw new DirectoryNotFoundException("tessdata folder not found: " + tessDataPath);

            TryPointNativeLoaderAtAppBase();

            _engine = new TesseractEngine(tessDataPath, languages, EngineMode.Default);
        }

        /// <summary>Chạy OCR trên <paramref name="image"/> và trả về các từ nhận dạng được, theo toạ độ pixel ảnh gốc-trên-trái.</summary>
        public IReadOnlyList<OcrWord> Extract(Bitmap image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            List<OcrWord> words = new List<OcrWord>();
            using (Pix pix = PixConverter.ToPix(image))
            using (Page page = _engine.Process(pix))
            using (ResultIterator iter = page.GetIterator())
            {
                iter.Begin();
                do
                {
                    string text = iter.GetText(PageIteratorLevel.Word);
                    if (string.IsNullOrWhiteSpace(text))
                        continue;

                    Rect rect;
                    if (!iter.TryGetBoundingBox(PageIteratorLevel.Word, out rect))
                        continue;

                    words.Add(new OcrWord(text.Trim(), new RectangleF(rect.X1, rect.Y1, rect.Width, rect.Height)));
                }
                while (iter.Next(PageIteratorLevel.Word));
            }
            return words;
        }

        public void Dispose()
        {
            _engine.Dispose();
        }

        /// <summary>Trỏ cơ chế tìm thư viện native của Tesseract vào thư mục của chính assembly này, để OCR hoạt động bất kể thư mục làm việc của ứng dụng dùng nó.</summary>
        private static void TryPointNativeLoaderAtAppBase()
        {
            try
            {
                string nativeDir = AppDomain.CurrentDomain.BaseDirectory;
                bool hasArchFolder = Directory.Exists(Path.Combine(nativeDir, Environment.Is64BitProcess ? "x64" : "x86"));
                if (string.IsNullOrEmpty(nativeDir) || !hasArchFolder)
                    nativeDir = Path.GetDirectoryName(typeof(OcrWordExtractor).Assembly.Location);
                if (!string.IsNullOrEmpty(nativeDir))
                    InteropDotNet.LibraryLoader.Instance.CustomSearchPath = nativeDir;
            }
            catch
            {
                // Nếu lỗi, quay về cơ chế tìm thư viện native mặc định của Tesseract.
            }
        }
    }
}
