using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ImageProcessing.Archiving;
using Tesseract;

namespace ImageProcessing.Ocr
{
    /// <summary>
    /// Runs a Tesseract engine over a page image and returns its recognized words (text + pixel
    /// bounding box), which <see cref="ImageProcessing.Archiving.PdfArchiver"/> places as an
    /// invisible searchable text layer via <c>OcrTextOverlay</c>.
    ///
    /// tessdata: the language data (eng, vie, ...) must live directly under
    /// <paramref name="tessDataPath"/> (the folder that CONTAINS the *.traineddata files) - see
    /// the constructor.
    ///
    /// Deployment note: this class only needs the managed Tesseract.dll to compile; at runtime it
    /// also needs Tesseract's native libraries (leptonica-*.dll / tesseract*.dll) present under an
    /// "x86"/"x64" subfolder of the consuming application's output directory - see the library's
    /// README for how to deploy them (they are not embedded in this DLL, matching how the
    /// original OpenImaging project relied on the host EXE to deploy them).
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

        /// <summary>Runs OCR over <paramref name="image"/> and returns its recognized words, in image-pixel/top-left-origin coordinates.</summary>
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

        /// <summary>Points Tesseract's native-library probing at this assembly's own directory, so OCR works regardless of the host's working directory.</summary>
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
                // Fall back to Tesseract's default native-library probing.
            }
        }
    }
}
