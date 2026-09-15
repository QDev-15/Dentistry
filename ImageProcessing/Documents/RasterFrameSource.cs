using System.Collections.Generic;
using System.Drawing;

namespace ImageProcessing.Documents
{
    /// <summary>Liệt kê các khung hình của 1 file raster - mỗi trang của TIFF nhiều trang, hoặc khung hình duy nhất của các loại file khác.</summary>
    internal static class RasterFrameSource
    {
        public static IEnumerable<Bitmap> EnumerateFrames(string imageFile)
        {
            if (TiffCodec.IsReadableTiff(imageFile) && TiffCodec.IsMultiPage(imageFile))
            {
                foreach (Bitmap page in TiffCodec.ReadAllPages(imageFile))
                    yield return page;
            }
            else
            {
                yield return new Bitmap(imageFile);
            }
        }
    }
}
