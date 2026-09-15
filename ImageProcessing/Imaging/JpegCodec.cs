using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageProcessing.Imaging
{
    /// <summary>Shared GDI+ JPEG encode helper (quality-parameterized), used by both the page
    /// writer and the PDF/A page builder so there is exactly one place that talks to
    /// <see cref="ImageCodecInfo"/>.</summary>
    internal static class JpegCodec
    {
        // The system's registered codec list never changes during process lifetime; querying it
        // via GetImageEncoders() walks and allocates a fresh array every call, so for a
        // high-throughput (web) caller resolving it once per process is a meaningful saving over
        // doing it on every single page saved.
        private static readonly ImageCodecInfo JpegEncoderInfo = ResolveEncoder();

        public static void SaveToFile(Bitmap bmp, string destPath, long quality)
        {
            using (EncoderParameters encoderParams = BuildParams(quality))
                bmp.Save(destPath, JpegEncoderInfo, encoderParams);
        }

        public static byte[] EncodeToBytes(Bitmap bmp, long quality)
        {
            using (MemoryStream ms = new MemoryStream())
            using (EncoderParameters encoderParams = BuildParams(quality))
            {
                bmp.Save(ms, JpegEncoderInfo, encoderParams);
                return ms.ToArray();
            }
        }

        private static EncoderParameters BuildParams(long quality)
        {
            EncoderParameters p = new EncoderParameters(1);
            p.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            return p;
        }

        private static ImageCodecInfo ResolveEncoder()
        {
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()
                .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
            if (codec == null)
                throw new InvalidOperationException("No JPEG encoder registered on this machine.");
            return codec;
        }
    }
}
