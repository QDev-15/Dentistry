using System.Collections.Generic;
using System.Drawing;

namespace ImageProcessing.Documents
{
    /// <summary>Enumerates the frames of a raster file - each page of a multi-page TIFF, or the single frame of anything else.</summary>
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
