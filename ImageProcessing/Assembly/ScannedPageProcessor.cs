using System;
using System.IO;
using ImageProcessing.Effects;

namespace ImageProcessing.Assembly
{
    /// <summary>
    /// Turns one freshly captured (raw) scanned page into its final stored form: runs the
    /// effect pipeline in place, then relocates the result into the destination folder under a
    /// new name (CCITT G4 TIFF for bitonal pages, JPEG otherwise).
    /// </summary>
    public static class ScannedPageProcessor
    {
        public static string Process(string sourceImagePath, EffectPipelineOptions effects, string destinationFolder)
        {
            if (string.IsNullOrEmpty(sourceImagePath)) throw new ArgumentException("sourceImagePath is required.", nameof(sourceImagePath));
            if (string.IsNullOrEmpty(destinationFolder)) throw new ArgumentException("destinationFolder is required.", nameof(destinationFolder));
            effects = effects ?? EffectPipelineOptions.None;

            ImageEffectFileOps.ApplyPipelineInPlace(sourceImagePath, effects);

            string extension = effects.ColorImage ? ".jpg" : ".tif";
            string finalPath = Path.Combine(destinationFolder, Guid.NewGuid().ToString("N") + extension);
            File.Move(sourceImagePath, finalPath);
            return finalPath;
        }
    }
}
